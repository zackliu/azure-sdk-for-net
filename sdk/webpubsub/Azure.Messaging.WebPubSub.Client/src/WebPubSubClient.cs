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
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Messaging.WebPubSub.Client.Protocols;
using Microsoft.AspNetCore.WebUtilities;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The WebPubSubService PubSub client.
    /// </summary>
    [SuppressMessage("Usage", "AZC0007:DO provide a minimal constructor that takes only the parameters required to connect to the service.", Justification = "WebPubSub clients are Websocket based and don't use ClientOptions functionality")]
    [SuppressMessage("Usage", "AZC0004:DO provide both asynchronous and synchronous variants for all service methods.", Justification = "Synchronous methods doesn't make sense in the scenario of WebPubSub client")]
    [SuppressMessage("Usage", "AZC0015:Unexpected client method return type.", Justification = "WebPubSubClient is a pure data plane client that don't need to return type as a management client does.")]
    public class WebPubSubClient : IMessageHandler, IDisposable
    {
        private WebSocketClient _client;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);
        private readonly WebPubSubClientCredential _webPubSubClientCredential;
        private readonly WebPubSubClientOptions _options;
        private readonly IWebPubSubProtocol _protocol;
        private readonly SequenceId _sequenceId = new SequenceId();
        private readonly ConcurrentDictionary<string, WebPubSubGroup> _groups = new();
        private readonly WebPubSubRetryPolicy _reconnectRetryPolicy;

        private readonly object _ackIdLock = new();
        private readonly object _statusLock = new();

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

        private volatile WebPubSubClientState _clientStatus;
        private volatile bool _disposed;
        private readonly CancellationTokenSource _stoppedCts = new();

        /// <summary>
        /// The status of the client
        /// </summary>
        internal WebPubSubClientState ClientStatus => _clientStatus;

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

            _clientStatus = WebPubSubClientState.Disconnected;

            var reconnectionRetryOptions = Utils.GetRetryOptions();
            reconnectionRetryOptions.MaxRetries = int.MaxValue;
            reconnectionRetryOptions.Delay = TimeSpan.FromSeconds(1);
            reconnectionRetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
            _reconnectRetryPolicy = new WebPubSubRetryPolicy(reconnectionRetryOptions);
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
        public virtual async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (_stoppedCts.IsCancellationRequested)
            {
                throw new InvalidOperationException("Can't start a closed client");
            }

            lock (_statusLock)
            {
                if (_clientStatus == WebPubSubClientState.Disconnected)
                {
                    _clientStatus = WebPubSubClientState.Connecting;
                    WebPubSubClientEventSource.Log.ClientStateChanges(WebPubSubClientState.Connecting);
                }
                else
                {
                    return;
                }
            }

            WebPubSubClientEventSource.Log.ClientStarting();

            try
            {
                // Reset before new connection.
                _sequenceId.Reset();
                _isInitialConnected = false;
                _latestDisconnectedMessage = null;
                _ackCache.Clear();

                var uri = await _webPubSubClientCredential.GetClientAccessUri(default).ConfigureAwait(false);
                _connnectionEndpoint = ParseClientAccessUri(uri);
                await ConnectCoreAsync(_connnectionEndpoint.FullEndpointUrl, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                lock (_statusLock)
                {
                    _clientStatus = WebPubSubClientState.Disconnected;
                    WebPubSubClientEventSource.Log.ClientStateChanges(WebPubSubClientState.Disconnected);
                }
            }
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            try
            {
                await (_client?.StopAsync(cancellationToken) ?? Task.CompletedTask).ConfigureAwait(false);
            }
            finally
            {
                _stoppedCts.Cancel();
            }
        }

        /// <summary>
        /// Get group operations
        /// </summary>
        /// <param name="name">The group name</param>
        /// <returns></returns>
        public virtual WebPubSubGroup Group(string name)
        {
            return _groups.GetOrAdd(name, n => new WebPubSubGroup(n, this, _options.ReconnectionOptions.AutoRejoinGroups));
        }

        /// <summary>
        /// Send custom event and wait for the ack.
        /// </summary>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="optionsBuilder">A set of options used while sending to sever.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<AckMessage> SendToServerAsync(BinaryData content, WebPubSubDataType dataType, ulong? ackId = null, Action<SendToServerOptions> optionsBuilder = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            AssertWritableStatus();

            var options = new SendToServerOptions();
            if (optionsBuilder != null)
            {
                optionsBuilder(options);
            }

            if (options.FireAndForget)
            {
                var message = new SendEventMessage(content, dataType, null);
                await SendMessageAsync(message, cancellationToken).ConfigureAwait(false);
                return null;
            }

            return await SendMessageWithAckIdAsync(id =>
            {
                return new SendEventMessage(content, dataType, id);
            }, ackId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// An event triggered when the connection is connected
        /// </summary>
        public event SyncAsyncEventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// An event triggered when the connection is disconnected
        /// </summary>
        public event SyncAsyncEventHandler<DisconnectedEventArgs> Disconnected;

        /// <summary>
        /// A event triggered when received messages from server.
        /// </summary>
        public event SyncAsyncEventHandler<ServerMessageEventArgs> ServerMessageReceived;

        /// <summary>
        /// Dispose and close the client.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Dispose and close the client
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _stoppedCts.Cancel();
            _client?.Dispose();
            _stoppedCts.Dispose();
            _sendLock.Dispose();
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

            lock (_statusLock)
            {
                _clientStatus = WebPubSubClientState.Connected;
                WebPubSubClientEventSource.Log.ClientStateChanges(WebPubSubClientState.Connected);
            }

            _ = Task.Run(() => ListenLoop(client), token);
        }

        private async Task ListenLoop(WebSocketClient socket)
        {
            var sequenceAckTask = Task.CompletedTask;
            var sequenceAckCts = new CancellationTokenSource();
            if (_protocol.IsReliableSubProtocol)
            {
                sequenceAckTask = Task.Run(() => SequenceAckLoop(sequenceAckCts.Token), CancellationToken.None);
            }

            using var buffer = new MemoryBufferWriter();
            try
            {
                await socket.StartReceive(this, _stoppedCts.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

                if (socket.CloseStatus == WebSocketCloseStatus.PolicyViolation)
                {
                    _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                }
                else
                {
                    lock (_statusLock)
                    {
                        _clientStatus = WebPubSubClientState.Suspended;
                    }
                    _ = Task.Run(() => TryRecovery(), CancellationToken.None);
                }
            }
        }

        private static ConnectionEndpoint ParseClientAccessUri(Uri clientAccessUri)
        {
            var builder = new UriBuilder(clientAccessUri);
            builder.Query = null;
            var baseUri = builder.Uri.AbsoluteUri;

            var query = QueryHelpers.ParseQuery(clientAccessUri.Query);
            if (query.TryGetValue("hub", out var hub))
            {
                baseUri = QueryHelpers.AddQueryString(baseUri, "hub", hub);
            }

            return new ConnectionEndpoint { BasePath = baseUri, FullEndpointUrl = clientAccessUri, Host = builder.Host };
        }

        private async Task SendCoreAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            var client = _client;
            if (client == null)
            {
                throw new SendMessageFailedException("Client is not started");
            }
            await client.SendAsync(buffer, messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
        }

        internal Task SendMessageAsync(WebPubSubMessage message, CancellationToken cancellationToken)
        {
            return SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken);
        }

        internal async Task<AckMessage> SendMessageWithAckIdAsync(Func<ulong, WebPubSubMessage> GetMessage, ulong? ackId, CancellationToken token)
        {
            var id = ackId ?? NextAckId();
            var entity = CreateAckEntity(id);
            var message = GetMessage(id);
            await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, token).ConfigureAwait(false);
            return await entity.Task.ConfigureAwait(false);
        }

        private async Task RaiseDisconnected(DisconnectedMessage disconnectedMessage)
        {
            lock (_statusLock)
            {
                _clientStatus = WebPubSubClientState.Disconnected;
            }

            foreach (var entity in _ackCache)
            {
                if (_ackCache.TryRemove(entity.Key, out var value))
                {
                    value.SetException(new SendMessageFailedException("Connection is disconnected before receive ack from the service"));
                }
            }

            try
            {
                await Disconnected.RaiseAsync(new DisconnectedEventArgs(disconnectedMessage, false), nameof(DisconnectedEventArgs), nameof(Disconnected)).ConfigureAwait(false);
            }
            catch
            {
            }

            if (_options.ReconnectionOptions.AutoReconnect)
            {
                _ = Task.Run(() => ExecuteAutoReconnection());
            }
        }

        internal class ConnectedState
        {
            public ConcurrentDictionary<string, Exception> FailedAutoRejoinedGroups { get; } = new();
        }

        private async Task RaiseConnectedAsync(ConnectedMessage connectedMessage, CancellationToken token)
        {
            var state = new ConnectedState();
            var tasks = new List<Task>();
            foreach (var g in _groups)
            {
                tasks.Add(InitiazeGroup(g.Key, g.Value));
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);

            await Connected.RaiseAsync(new ConnectedEventArgs(connectedMessage, false, token), nameof(ConnectedEventArgs), nameof(Connected)).ConfigureAwait(false);

            async Task InitiazeGroup(string groupName, WebPubSubGroup group)
            {
                try
                {
                    await group.InitializeAsync(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    state.FailedAutoRejoinedGroups.TryAdd(groupName, ex);
                }
            }
        }

        private Task RaiseServerMessageReceivedAsync(ServerResponseMessage serverResponseMessage, CancellationToken token)
        {
            return ServerMessageReceived.RaiseAsync(new ServerMessageEventArgs(serverResponseMessage, false, token), nameof(ServerMessageEventArgs), nameof(ServerMessageReceived));
        }

        private async Task ExecuteAutoReconnection()
        {
            if (_stoppedCts.IsCancellationRequested)
            {
                return;
            }

            var retryAttempt = 0;
            while (true)
            {
                try
                {
                    await ConnectAsync().ConfigureAwait(false);
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

        private async Task TryRecovery()
        {
            // Called StopAsync, don't recover or restart.
            if (_stoppedCts.IsCancellationRequested)
            {
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            // Unrecoverable protocol
            if (!_protocol.IsReliableSubProtocol)
            {
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            var uri = BuildRecoveryUri();

            // Can't recovery
            if (uri == null)
            {
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }

            // Totally timeout 30s as service will remove the connection if it's not recovered in 30s
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
                    catch
                    {
                        await Task.Delay(1000).ConfigureAwait(false);
                    }
                }
            }
            catch
            {
                _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                return;
            }
        }

        async Task IMessageHandler.HandleMessageAsync(WebPubSubMessage message, CancellationToken token)
        {
            switch (message)
            {
                case ConnectedMessage connectedMessage:
                    await HandleConnectedMessage(connectedMessage, token).ConfigureAwait(false);
                    break;
                case DisconnectedMessage disconnectedMessage:
                    await HandleDisconnectedMessage(disconnectedMessage, token).ConfigureAwait(false);
                    break;
                case GroupResponseMessage groupResponseMessage:
                    await HandleGroupMessage(groupResponseMessage, token).ConfigureAwait(false);
                    break;
                case ServerResponseMessage serverResponseMessage:
                    await HandleServerMessage(serverResponseMessage, token).ConfigureAwait(false);
                    break;
                case AckMessage ackMessage:
                    await HandleAckMessage(ackMessage, token).ConfigureAwait(false);
                    break;
                default:
                    throw new InvalidDataException($"Received unknown type of message {message.GetType()}");
            }

            async Task HandleConnectedMessage(ConnectedMessage connectedMessage, CancellationToken token)
            {
                _connectionId = connectedMessage.ConnectionId;
                _reconnectionToken = connectedMessage.ReconnectionToken;

                if (!_isInitialConnected)
                {
                    _isInitialConnected = true;
                    await RaiseConnectedAsync(connectedMessage, token).ConfigureAwait(false);
                }
            }

            Task HandleDisconnectedMessage(DisconnectedMessage disconnectedMessage, CancellationToken _)
            {
                _latestDisconnectedMessage = disconnectedMessage;
                return Task.CompletedTask;
            }

            async Task HandleGroupMessage(GroupResponseMessage groupResponseMessage, CancellationToken token)
            {
                if (groupResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(groupResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return;
                    }
                }

                if (_groups.TryGetValue(groupResponseMessage.Group, out var group))
                {
                    await group.HandleMessageAsync(groupResponseMessage, token).ConfigureAwait(false);
                }
            }

            Task HandleServerMessage(ServerResponseMessage serverResponseMessage, CancellationToken token)
            {
                if (serverResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(serverResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return Task.CompletedTask;
                    }
                }

                return RaiseServerMessageReceivedAsync(serverResponseMessage, token);
            }

            Task HandleAckMessage(AckMessage ackMessage, CancellationToken _)
            {
                if (_ackCache.TryGetValue(ackMessage.AckId, out var entity))
                {
                    if (ackMessage.Success ||
                        ackMessage.Error?.Name == "Duplicate")
                    {
                        entity.SetResult(ackMessage);
                    }

                    entity.SetException(new SendMessageFailedException("Send message failed", ackMessage));
                }

                return Task.CompletedTask;
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
                var builder = new UriBuilder(_connnectionEndpoint.BasePath);
                builder.Query = $"awps_connection_id={_connectionId}&awps_reconnection_token={_reconnectionToken}";
                return builder.Uri;
            }
            return null;
        }

        private AckEntity CreateAckEntity(ulong ackId)
        {
            return _ackCache.AddOrUpdate(ackId, new AckEntity(), (_, oldEntity) => oldEntity);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("The client is already disposed");
            }
        }

        private void AssertWritableStatus()
        {
            lock (_statusLock)
            {
                if (_clientStatus != WebPubSubClientState.Connected)
                {
                    throw new InvalidOperationException($"Client is not in the status to perform operations: {_clientStatus.ToString()}");
                }
            }
        }

        private class AckEntity
        {
            private TaskCompletionSource<AckMessage> _tcs = new TaskCompletionSource<AckMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            public void SetResult(AckMessage message) => _tcs.TrySetResult(message);
            public void SetCancelled() => _tcs.TrySetException(new OperationCanceledException());
            public void SetException(Exception ex) => _tcs.TrySetException(ex);
            public Task<AckMessage> Task => _tcs.Task;
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

        private class ConnectionEndpoint
        {
            public Uri FullEndpointUrl { get; set; }

            public string BasePath { get; set; }

            public string Host { get; set; }
        }
    }
}
