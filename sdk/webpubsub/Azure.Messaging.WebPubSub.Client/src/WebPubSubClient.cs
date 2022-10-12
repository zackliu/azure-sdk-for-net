// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using Azure.Core;
using Azure.Core.Pipeline;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The WebPubSubService PubSub client.
    /// </summary>
    [SuppressMessage("Usage", "AZC0007:DO provide a minimal constructor that takes only the parameters required to connect to the service.", Justification = "WebPubSub clients are Websocket based and don't use ClientOptions functionality")]
    [SuppressMessage("Usage", "AZC0004:DO provide both asynchronous and synchronous variants for all service methods.", Justification = "Synchronous methods doesn't make sense in the scenario of WebPubSub client")]
    [SuppressMessage("Usage", "AZC0015:Unexpected client method return type.", Justification = "WebPubSubClient is a pure data plane client that don't need to return type as a management client does.")]
    public class WebPubSubClient : IMessageHandler, IAsyncDisposable
    {
        private static readonly UnboundedChannelOptions s_unboundedChannelOptions = new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = true,
        };

#pragma warning disable CA2213 // Disposable fields should be disposed
        private WebSocketClient _client;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);
#pragma warning restore CA2213 // Disposable fields should be disposed
        private readonly WebPubSubClientCredential _webPubSubClientCredential;
        private readonly WebPubSubClientOptions _options;
        private readonly WebPubSubProtocol _protocol;
        private readonly SequenceId _sequenceId = new SequenceId();
        private readonly ConcurrentDictionary<string, WebPubSubGroup> _groups = new();
        private readonly WebPubSubRetryPolicy _reconnectRetryPolicy;
        private readonly ClientState _clientState;
        private readonly Channel<GroupDataMessage> _groupDataChannel = Channel.CreateUnbounded<GroupDataMessage>(s_unboundedChannelOptions);
        private readonly Channel<ServerDataMessage> _serverDataChannel = Channel.CreateUnbounded<ServerDataMessage>(s_unboundedChannelOptions);
        private readonly Task ProcessingServerDataMessageTask;
        private readonly Task ProcessingGroupDataMessageTask;

        private readonly object _ackIdLock = new();

        // Fields per connection-id
        private ConnectionEndpoint _connnectionEndpoint;
        private string _connectionId;
        private string _reconnectionToken;
        private bool _isInitialConnected;
        private DisconnectedMessage _latestDisconnectedMessage;
        private ConcurrentDictionary<ulong, AckEntity> _ackCache = new();

        private ulong _nextAckId;

        private ulong NextAckId()
        {
            lock (_ackIdLock)
            {
                _nextAckId = _nextAckId + 1;
                return _nextAckId;
            }
        }

        private volatile bool _disposed;
#pragma warning disable CA2213 // Disposable fields should be disposed
        private readonly CancellationTokenSource _stoppedCts = new();
#pragma warning restore CA2213 // Disposable fields should be disposed
        private Task ReceiveTask;

        /// <summary>
        /// The status of the client
        /// </summary>
        internal WebPubSubClientState ClientStatus => _clientState.CurrentState;

        /// <summary>
        /// The connection id of the client
        /// </summary>
        public string ConnectionId => _connectionId;

        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="clientAccessUri">The uri to connect to the service.</param>
        public WebPubSubClient(Uri clientAccessUri) : this(new WebPubSubClientCredential(clientAccessUri))
        {
        }

        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="credential">A uri provider that will be called to return the uri for each connecting or reconnecting.</param>
        /// <param name="options">A option for the client.</param>
        public WebPubSubClient(WebPubSubClientCredential credential, WebPubSubClientOptions options = null)
        {
            _webPubSubClientCredential = credential ?? throw new ArgumentNullException(nameof(credential));

            if (options == null)
            {
                _options = new WebPubSubClientOptions();
            }
            else
            {
                _options = options;
            }
            _protocol = _options.Protocol ?? throw new ArgumentNullException(nameof(options));

            _clientState = new ClientState();

            var reconnectionRetryOptions = Utils.GetRetryOptions();
            reconnectionRetryOptions.MaxRetries = int.MaxValue;
            reconnectionRetryOptions.Delay = TimeSpan.FromSeconds(1);
            reconnectionRetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
            _reconnectRetryPolicy = new WebPubSubRetryPolicy(reconnectionRetryOptions);

            // Process message
            ProcessingServerDataMessageTask = StartServerProcessingDataMessage();
            ProcessingGroupDataMessageTask = StartGroupProcessingDataMessage();
        }

        /// <summary>
        /// Constructor for mock.
        /// </summary>
        protected WebPubSubClient()
        {
        }

        /// <summary>
        /// Start connecting to the service.
        /// </summary>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual async Task StartAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_stoppedCts.IsCancellationRequested)
            {
                throw new InvalidOperationException("Can't start a closed client");
            }

            _clientState.ChangeState(WebPubSubClientState.Disconnected, WebPubSubClientState.Connecting);

            WebPubSubClientEventSource.Log.ClientStarting();

            try
            {
                // Reset before new connection.
                _sequenceId.Reset();
                _isInitialConnected = false;
                _latestDisconnectedMessage = null;
                _ackCache.Clear();

                var uri = await _webPubSubClientCredential.GetClientAccessUriAsync(default).ConfigureAwait(false);
                _connnectionEndpoint = ParseClientAccessUri(uri);
                await ConnectCoreAsync(_connnectionEndpoint.FullEndpointUrl, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                _clientState.ChangeState(WebPubSubClientState.Disconnected);
                throw;
            }
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        /// <returns></returns>
#pragma warning disable AZC0003 // DO make service methods virtual.
        public async ValueTask DisposeAsync()
#pragma warning restore AZC0003 // DO make service methods virtual.
        {
            // Perform async cleanup.
            await DisposeAsyncCore().ConfigureAwait(false);

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            // Suppress finalization.
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                await (_client?.StopAsync(CancellationToken.None) ?? Task.CompletedTask).ConfigureAwait(false);
            }
            catch
            {
            }

            _serverDataChannel.Writer.TryComplete();
            _groupDataChannel.Writer.TryComplete();

            _stoppedCts.Cancel();
            _client?.Dispose();
            _stoppedCts.Dispose();
            _sendLock.Dispose();
        }

        /// <summary>
        /// Join the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="ackId">An optional ack id. It's generated by SDK if not assigned.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation.</returns>
        public virtual async Task<WebPubSubResult> JoinGroupAsync(string group, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var groupEntity = _groups.GetOrAdd(group, n => new WebPubSubGroup(n));
            var ack = await SendMessageWithAckAsync(id =>
            {
                return new JoinGroupMessage(group, id);
            }, ackId, cancellationToken).ConfigureAwait(false);
            groupEntity.IsJoined = true;
            return ack;
        }

        /// <summary>
        /// Leave the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="ackId">An optional ack id. It's generated by SDK if not assigned.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<WebPubSubResult> LeaveGroupAsync(string group, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            var groupEntity = _groups.GetOrAdd(group, n => new WebPubSubGroup(n));
            var ack = await SendMessageWithAckAsync(id =>
            {
                return new LeaveGroupMessage(group, id);
            }, ackId, cancellationToken).ConfigureAwait(false);
            groupEntity.IsJoined = false;
            return ack;
        }

        /// <summary>
        /// Publish data to group and wait for the ack.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="noEcho">If set to true, this message is not echoed back to the same connection. If not set, the default value is false.</param>
        /// <param name="fireAndForget">If set to true, the service won't return ack for this message. The return value will be Task of null </param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<WebPubSubResult> SendToGroupAsync(string group, BinaryData content, WebPubSubDataType dataType, ulong? ackId = null, bool noEcho = false, bool fireAndForget = false, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (fireAndForget)
            {
                var message = new SendToGroupMessage(group, content, dataType, null, noEcho);
                await SendMessageWithoutAckAsync(message, cancellationToken).ConfigureAwait(false);
                return null;
            }

            return await SendMessageWithAckAsync(id =>
            {
                return new SendToGroupMessage(group, content, dataType, id, noEcho);
            }, ackId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Send custom event and wait for the ack.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="fireAndForget">If set to true, the service won't return ack for this message. The return value will be Task of null </param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<WebPubSubResult> SendEventAsync(string eventName, BinaryData content, WebPubSubDataType dataType, ulong? ackId = null, bool fireAndForget = false, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (fireAndForget)
            {
                var message = new SendEventMessage(eventName, content, dataType, null);
                await SendMessageWithoutAckAsync(message, cancellationToken).ConfigureAwait(false);
                return null;
            }

            return await SendMessageWithAckAsync(id =>
            {
                return new SendEventMessage(eventName, content, dataType, id);
            }, ackId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// An event triggered when the connection is connected
        /// </summary>
        public event SyncAsyncEventHandler<WebPubSubConnectedEventArgs> Connected;

        /// <summary>
        /// An event triggered when the connection is disconnected
        /// </summary>
        public event SyncAsyncEventHandler<WebPubSubDisconnectedEventArgs> Disconnected;

        /// <summary>
        /// An event triggered when the connection is stopped
        /// </summary>
        public event SyncAsyncEventHandler<WebPubSubStoppedEventArgs> Stopped;

        /// <summary>
        /// A event triggered when received server data messages.
        /// </summary>
        public event SyncAsyncEventHandler<ServerMessageEventArgs> ServerMessageReceived;

        /// <summary>
        /// A event triggered when received group data messages.
        /// </summary>
        private event SyncAsyncEventHandler<GroupMessageEventArgs> _groupMessageReceived;

        /// <summary>
        /// A event triggered when received group data messages.
        /// </summary>
        public virtual IDisposable GroupMessageReceived(SyncAsyncEventHandler<GroupMessageEventArgs> handler)
        {
            _groupMessageReceived += handler;
            return new DisposableEvent(() => _groupMessageReceived -= handler);
        }

        /// <summary>
        /// A event triggered when received specific group data messages.
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="handler">The message handler</param>
        /// <returns></returns>
        public virtual IDisposable GroupMessageReceived(string group, SyncAsyncEventHandler<GroupMessageEventArgs> handler)
        {
            var groupEntity = _groups.GetOrAdd(group, n => new WebPubSubGroup(n));
            groupEntity.MessageReceived += handler;
            return new DisposableEvent(() => groupEntity.MessageReceived -= handler);
        }

        private async Task ConnectCoreAsync(Uri uri, CancellationToken token)
        {
            var client = new WebSocketClient(uri, _protocol);

            try
            {
                await client.ConnectAsync(token).ConfigureAwait(false);
            }
            catch
            {
                client.Dispose();
                throw;
            }

            var oldClient = _client;
            _client = client;
            oldClient?.Dispose();

            _clientState.ChangeState(WebPubSubClientState.Connected);

            ReceiveTask = Task.Run(() => ListenLoop(client), token);
        }

        private async Task ListenLoop(WebSocketClient client)
        {
            var sequenceAckTask = Task.CompletedTask;
            var sequenceAckCts = new CancellationTokenSource();
            if (_protocol.IsReliable)
            {
                sequenceAckTask = Task.Run(() => SequenceAckLoop(sequenceAckCts.Token), CancellationToken.None);
            }

            using var buffer = new MemoryBufferWriter();
            try
            {
                await client.StartReceive(this, _stoppedCts.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                WebPubSubClientEventSource.Log.FailedReceivingBytes(ex.Message);
            }
            finally
            {
                try
                {
                    sequenceAckCts.Cancel();
                    sequenceAckCts.Dispose();
                    await sequenceAckTask.ConfigureAwait(false);
                }
                catch
                {
                }

                if (client.CloseStatus == WebSocketCloseStatus.PolicyViolation)
                {
                    WebPubSubClientEventSource.Log.StopRecovery(_connectionId, $"The websocket close with status: {WebSocketCloseStatus.PolicyViolation}");
                    _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                }
                else
                {
                    _ = Task.Run(() => TryRecovery(), CancellationToken.None);
                }
            }
        }

        private static ConnectionEndpoint ParseClientAccessUri(Uri clientAccessUri)
        {
            return new ConnectionEndpoint { FullEndpointUrl = clientAccessUri };
        }

        private async Task SendCoreAsync(ReadOnlyMemory<byte> buffer, WebPubSubProtocolMessageType webPubSubProtocolMessageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            var messageType = webPubSubProtocolMessageType == WebPubSubProtocolMessageType.Text ? WebSocketMessageType.Text : WebSocketMessageType.Binary;
            await _client.SendAsync(buffer, messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
        }

        internal async Task SendMessageWithoutAckAsync(WebPubSubMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SendMessageFailedException("Failed to send message.", null, ex);
            }
        }

        internal async Task<WebPubSubResult> SendMessageWithAckAsync(Func<ulong, WebPubSubMessage> GetMessage, ulong? ackId, CancellationToken token)
        {
            var id = ackId ?? NextAckId();
            var entity = CreateAckEntity(id);
            var message = GetMessage(id);
            try
            {
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (_ackCache.TryRemove(id, out var e))
                {
                    e.SetCancelled();
                }
                throw new SendMessageFailedException("Failed to send message.", id, ex);
            }
            return await entity.Task.ConfigureAwait(false);
        }

        private async Task RaiseDisconnected(DisconnectedMessage disconnectedMessage)
        {
            _clientState.ChangeState(WebPubSubClientState.Disconnected);

            foreach (var entity in _ackCache)
            {
                if (_ackCache.TryRemove(entity.Key, out var value))
                {
                    value.SetException(new SendMessageFailedException("Connection is disconnected before receive ack from the service", entity.Value.AckId));
                }
            }

            try
            {
                await Disconnected.RaiseAsync(new WebPubSubDisconnectedEventArgs(disconnectedMessage, false), nameof(WebPubSubDisconnectedEventArgs), nameof(Disconnected)).ConfigureAwait(false);
            }
            catch
            {
            }

            if (_options.ReconnectionOptions.AutoReconnect)
            {
                _ = Task.Run(() => ExecuteAutoReconnection());
            }
            else
            {
                _ = Task.Run(() => RaiseStopped(default));
            }
        }

        private async void RaiseConnected(ConnectedMessage connectedMessage, CancellationToken token)
        {
            var groupRestoreState = new Dictionary<string, Exception>();
            foreach (var (name, g) in _groups)
            {
                if (g.IsJoined)
                {
                    try
                    {
                        await SendMessageWithAckAsync(id =>
                        {
                            return new JoinGroupMessage(name, id);
                        }, null, token: token).ConfigureAwait(false);
                        groupRestoreState.Add(name, null);
                    }
                    catch (Exception ex)
                    {
                        groupRestoreState.Add(name, ex);
                    }
                }
            }
            _ = Connected.RaiseAsync(new WebPubSubConnectedEventArgs(connectedMessage, groupRestoreState, false, token), nameof(WebPubSubConnectedEventArgs), nameof(Connected));
        }

        private async void RaiseStopped(CancellationToken token)
        {
            await Stopped.RaiseAsync(new WebPubSubStoppedEventArgs(false, token), nameof(WebPubSubStoppedEventArgs), nameof(Stopped)).ConfigureAwait(false);
        }

        private async Task RaiseGroupMessageReceivedAsync(GroupDataMessage message, CancellationToken token)
        {
            try
            {
                await _groupMessageReceived.RaiseAsync(new GroupMessageEventArgs(message, false, token), nameof(GroupMessageEventArgs), nameof(GroupMessageEventArgs)).ConfigureAwait(false);
            }
            catch
            {
            }
        }

        private async Task RaiseServerMessageReceivedAsync(ServerDataMessage message, CancellationToken token)
        {
            try
            {
                await ServerMessageReceived.RaiseAsync(new ServerMessageEventArgs(message, false, token), nameof(ServerMessageEventArgs), nameof(ServerMessageEventArgs)).ConfigureAwait(false);
            }
            catch
            {
            }
        }

        private async Task ExecuteAutoReconnection()
        {
            if (_stoppedCts.IsCancellationRequested)
            {
                _ = Task.Run(() => RaiseStopped(default));
                return;
            }

            var retryAttempt = 0;
            try
            {
                while (!_stoppedCts.IsCancellationRequested)
                {
                    try
                    {
                        await StartAsync().ConfigureAwait(false);
                        return;
                    }
                    catch
                    {
                        retryAttempt++;
                        var delay = _reconnectRetryPolicy.NextRetryDelay(new RetryContext { RetryAttempt = retryAttempt });

                        if (delay == null)
                        {
                            throw;
                        }

                        await Task.Delay(delay.Value).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                // Stop retry
                _ = Task.Run(() => RaiseStopped(default));
            }
        }

        private async Task TryRecovery()
        {
            foreach (var entity in _ackCache)
            {
                if (_ackCache.TryRemove(entity.Key, out var value))
                {
                    value.SetException(new SendMessageFailedException("Connection is disconnected before receive ack from the service", entity.Value.AckId));
                }
            }

            // Called StopAsync, don't recover or restart.
            if (_stoppedCts.IsCancellationRequested)
            {
                WebPubSubClientEventSource.Log.StopRecovery(_connectionId, "The client is stopped");
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            // Unrecoverable protocol
            if (!_protocol.IsReliable)
            {
                WebPubSubClientEventSource.Log.StopRecovery(_connectionId, "The protocol is not reliable, recovery is not applicable");
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            var uri = BuildRecoveryUri();

            // Can't recovery
            if (uri == null)
            {
                WebPubSubClientEventSource.Log.StopRecovery(_connectionId, "Connection id or reonnection token is not availble");
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            // Totally timeout 30s as service will remove the connection if it's not recovered in 30s
            _clientState.ChangeState(WebPubSubClientState.Recovering);
            var cts = new CancellationTokenSource(30 * 1000);
            try
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        await ConnectCoreAsync(uri, CancellationToken.None).ConfigureAwait(false);
                        return;
                    }
                    catch (Exception ex)
                    {
                        WebPubSubClientEventSource.Log.RecoveryAttemptFailed(_connectionId, ex.Message);
                        await Task.Delay(1000).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                WebPubSubClientEventSource.Log.StopRecovery(_connectionId, "Recovery attempts failed more then 30 seconds");
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }
            finally
            {
                cts.Dispose();
            }
        }

        Task IMessageHandler.HandleMessageAsync(WebPubSubMessage message, CancellationToken token)
        {
            switch (message)
            {
                case ConnectedMessage connectedMessage:
                    HandleConnectedMessage(connectedMessage);
                    break;
                case DisconnectedMessage disconnectedMessage:
                    HandleDisconnectedMessage(disconnectedMessage);
                    break;
                case AckMessage ackMessage:
                    HandleAckMessage(ackMessage);
                    break;
                case GroupDataMessage groupResponseMessage:
                    HandleGroupMessage(groupResponseMessage);
                    break;
                case ServerDataMessage serverResponseMessage:
                    HandleServerMessage(serverResponseMessage);
                    break;
                default:
                    throw new InvalidDataException($"Received unknown type of message {message.GetType()}");
            }

            return Task.CompletedTask;

            void HandleConnectedMessage(ConnectedMessage connectedMessage)
            {
                _connectionId = connectedMessage.ConnectionId;
                _reconnectionToken = connectedMessage.ReconnectionToken;

                if (!_isInitialConnected)
                {
                    _isInitialConnected = true;
                    RaiseConnected(connectedMessage, token);
                }
            }

            void HandleDisconnectedMessage(DisconnectedMessage disconnectedMessage)
            {
                _latestDisconnectedMessage = disconnectedMessage;
            }

            void HandleGroupMessage(GroupDataMessage groupResponseMessage)
            {
                if (groupResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(groupResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return;
                    }
                }

                _groupDataChannel.Writer.TryWrite(groupResponseMessage);
            }

            void HandleServerMessage(ServerDataMessage serverResponseMessage)
            {
                if (serverResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(serverResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return;
                    }
                }

                _serverDataChannel.Writer.TryWrite(serverResponseMessage);
            }

            void HandleAckMessage(AckMessage ackMessage)
            {
                if (_ackCache.TryGetValue(ackMessage.AckId, out var entity))
                {
                    if (ackMessage.Success ||
                        ackMessage.Error?.Name == "Duplicate")
                    {
                        entity.SetResult(new WebPubSubResult(ackMessage.AckId));
                    }

                    entity.SetException(new SendMessageFailedException("Received non-success acknowledge from the service", ackMessage.AckId, ackMessage.Error));
                }
            }
        }

        private async Task StartServerProcessingDataMessage()
        {
            var reader = _serverDataChannel.Reader;
            while (await reader.WaitToReadAsync().ConfigureAwait(false))
            {
                while (reader.TryRead(out var message))
                {
                    await RaiseServerMessageReceivedAsync(message, default).ConfigureAwait(false);
                }
            }
        }

        private async Task StartGroupProcessingDataMessage()
        {
            var reader = _groupDataChannel.Reader;
            while (await reader.WaitToReadAsync().ConfigureAwait(false))
            {
                while (reader.TryRead(out var message))
                {
                    if (_groups.TryGetValue(message.Group, out var group))
                    {
                        await group.HandleMessageAsync(message, default).ConfigureAwait(false);
                    }

                    await RaiseGroupMessageReceivedAsync(message, default).ConfigureAwait(false);
                }
            }
        }

        private async Task SequenceAckLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_sequenceId.TryGetSequenceId(out var sequenceId))
                    {
                        var payload = _protocol.GetMessageBytes(new SequenceAckMessage(sequenceId));
                        await SendCoreAsync(payload, _protocol.WebSocketMessageType, true, token).ConfigureAwait(false);
                    }
                }
                catch
                {
                }
                finally
                {
                    await Task.Delay(1000, token).ConfigureAwait(false);
                }
            }
        }

        private Uri BuildRecoveryUri()
        {
            if (_connectionId != null && _reconnectionToken != null)
            {
                var builder = new UriBuilder(_connnectionEndpoint.FullEndpointUrl);
                var query = HttpUtility.ParseQueryString(builder.Query);
                query.Add("awps_connection_id", _connectionId);
                query.Add("awps_reconnection_token", _reconnectionToken);
                builder.Query = query.ToString();
                return builder.Uri;
            }
            return null;
        }

        private AckEntity CreateAckEntity(ulong ackId)
        {
            return _ackCache.AddOrUpdate(ackId, new AckEntity(ackId), (_, oldEntity) => oldEntity);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("The client is already disposed");
            }
        }

        private class AckEntity
        {
            public ulong AckId { get; }

            public AckEntity(ulong ackId)
            {
                AckId = ackId;
            }

            private TaskCompletionSource<WebPubSubResult> _tcs = new TaskCompletionSource<WebPubSubResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            public void SetResult(WebPubSubResult message) => _tcs.TrySetResult(message);
            public void SetCancelled() => _tcs.TrySetException(new OperationCanceledException());
            public void SetException(Exception ex) => _tcs.TrySetException(ex);
            public Task<WebPubSubResult> Task => _tcs.Task;
        }

        private class SequenceId
        {
            private readonly object _lock = new object();
            private ulong _sequenceId;
            private bool _updated;

            public bool TryUpdate(ulong sequenceId)
            {
                lock (_lock)
                {
                    _updated = true;

                    if (sequenceId > _sequenceId)
                    {
                        _sequenceId = sequenceId;
                        return true;
                    }
                    return false;
                }
            }

            public bool TryGetSequenceId(out ulong sequenceId)
            {
                lock (_lock)
                {
                    if (_updated)
                    {
                        sequenceId = _sequenceId;
                        _updated = false;
                        return true;
                    }

                    sequenceId = 0;
                    return false;
                }
            }

            public void Reset()
            {
                lock (_lock)
                {
                    _sequenceId = 0;
                    _updated = false;
                }
            }
        }

        private class ClientState
        {
            private readonly object _lock = new();

            public WebPubSubClientState CurrentState { get; private set; } = WebPubSubClientState.Disconnected;

            public void ChangeState(WebPubSubClientState expectedState, WebPubSubClientState newState)
            {
                if (!TryChangeState(expectedState, newState))
                {
                    throw new InvalidOperationException($"The client failed to transition from the '{expectedState}' state to the '{newState}' state because it was actually in the '{CurrentState}' state.");
                }
            }

            public void ChangeState(WebPubSubClientState newState)
            {
                lock (_lock)
                {
                    if (CurrentState != newState)
                    {
                        WebPubSubClientEventSource.Log.ClientStateChanges(newState.ToString(), CurrentState.ToString());
                        CurrentState = newState;
                    }
                }
            }

            public bool TryChangeState(WebPubSubClientState expectedState, WebPubSubClientState newState)
            {
                lock (_lock)
                {
                    if (CurrentState != expectedState)
                    {
                        WebPubSubClientEventSource.Log.FailedToChangeClientState(expectedState.ToString(), newState.ToString(), CurrentState.ToString());
                        return false;
                    }

                    WebPubSubClientEventSource.Log.ClientStateChanges(newState.ToString(), CurrentState.ToString());
                    CurrentState = newState;
                    return true;
                }
            }
        }

        private sealed class DisposableEvent : IDisposable
        {
            private readonly Action _disposeAction;

            public DisposableEvent(Action disposeAction)
            {
                _disposeAction = disposeAction;
            }

            public void Dispose()
            {
                _disposeAction();
            }
        }

        private class ConnectionEndpoint
        {
            public Uri FullEndpointUrl { get; set; }
        }
    }
}
