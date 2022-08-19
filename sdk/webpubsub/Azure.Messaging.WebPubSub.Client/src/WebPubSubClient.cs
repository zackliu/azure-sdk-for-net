// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Core;
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
    public class WebPubSubClient : IDisposable
    {
        private WebSocket _socket;
        private readonly WebPubSubClientCredential _webPubSubClientCredential;
        private readonly WebPubSubClientOptions _options;
        private readonly IWebPubSubProtocol _protocol;

        private readonly object _ackIdLock = new();
        private readonly object _statusLock = new();

        // Fields per connection-id
        private ConnectionEndpoint _connnectionEndpoint;
        private string _connectionId;
        private string _reconnectionToken;
        private SequenceId _sequenceId;
        private bool _isInitialConnected;
        private DisconnectedMessage _latestDisconnectedMessage;
        private ConcurrentDictionary<ulong, AckEntity> _ackCache = new();
        private ConcurrentDictionary<string, Func<GroupResponseMessage, Task>> _groupEventHandlers = new();
        private ulong _nextAckId;

        private ulong NextAckId()
        {
            lock (_ackIdLock)
            {
                _nextAckId = _nextAckId + 1;
                return _nextAckId;
            }
        }

        private volatile WebPubSubClientStatus _clientStatus;
        private volatile bool _disposed;
        private readonly CancellationTokenSource _stopCts = new();
        private readonly TaskCompletionSource<object> _stopTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        /// <summary>
        /// The status of the client
        /// </summary>
        internal WebPubSubClientStatus ClientStatus => _clientStatus;

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
                _options = BuildDefaultClientOptions();
            }
            else
            {
                _options = options;
            }
            _protocol = _options.Protocol ?? throw new ArgumentNullException(nameof(options));

            _clientStatus = WebPubSubClientStatus.Disconnected;
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

            if (_stopCts.IsCancellationRequested)
            {
                throw new InvalidOperationException("Can't start a closed client");
            }

            lock (_statusLock)
            {
                if (_clientStatus != WebPubSubClientStatus.Disconnected)
                {
                    return;
                }

                _clientStatus = WebPubSubClientStatus.Connecting;
            }

            try
            {
                var uri = await _webPubSubClientCredential.GetClientAccessUri(default).ConfigureAwait(false);
                _connnectionEndpoint = ParseClientAccessUri(uri);
                await ConnectCoreAsync(_connnectionEndpoint.FullEndpointUrl, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                lock (_statusLock)
                {
                    _clientStatus = WebPubSubClientStatus.Disconnected;
                }
            }
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            _stopCts.Cancel();
            return _stopTcs.Task;
        }

        /// <summary>
        /// Join the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="handler">The handler function to handle group message.</param>
        /// <param name="ackId">An optional ack id. It's generated by SDK if not assigned.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation.</returns>
        public virtual async Task<AckMessage> JoinGroupAsync(string group, Func<GroupResponseMessage, Task> handler, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            AssertWritableStatus();

            _groupEventHandlers.AddOrUpdate(group, handler, (_, __) => handler);
            return await SendMessageWithAckId(async (id, token) =>
            {
                var message = new JoinGroupMessage(group, id);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
            }, ackId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Leave the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// /// <param name="ackId">An optional ack id. It's generated by SDK if not assigned.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<AckMessage> LeaveGroupAsync(string group, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            AssertWritableStatus();

            _groupEventHandlers.TryRemove(group, out _);
            return await SendMessageWithAckId(async (id, token) =>
            {
                var message = new LeaveGroupMessage(group, id);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
            }, ackId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Publish data to group and wait for the ack.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="optionsBuilder">A set of options used while sending to group.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual async Task<AckMessage> SendToGroupAsync(string group, BinaryData content, WebPubSubDataType dataType, ulong? ackId = null, Action<SendToGroupOptions> optionsBuilder = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            AssertWritableStatus();

            var options = BuildDefaultSendToGroupOptions();
            if (optionsBuilder != null)
            {
                optionsBuilder(options);
            }

            if (options.FireAndForget)
            {
                var message = new SendToGroupMessage(group, content, dataType, null, options.NoEcho);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
                return null;
            }

            return await SendMessageWithAckId(async (id, token) =>
            {
                var message = new SendToGroupMessage(group, content, dataType, id, options.NoEcho);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
            }, ackId, cancellationToken).ConfigureAwait(false);
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

            var options = BuildDefaultSendToServerOptions();
            if (optionsBuilder != null)
            {
                optionsBuilder(options);
            }

            if (options.FireAndForget)
            {
                var message = new SendEventMessage(content, dataType, null);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
                return null;
            }

            return await SendMessageWithAckId(async (id, token) =>
            {
                var message = new SendEventMessage(content, dataType, id);
                await SendCoreAsync(_protocol.GetMessageBytes(message), _protocol.WebSocketMessageType, true, cancellationToken).ConfigureAwait(false);
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
            _stopCts.Cancel();
            _socket?.Dispose();
            _stopCts.Dispose();
        }

        private async Task ConnectCoreAsync(Uri uri, CancellationToken token)
        {
            var client = new ClientWebSocket();
            client.Options.AddSubProtocol(_options.Protocol.Name);

            await client.ConnectAsync(uri, token).ConfigureAwait(false);
            _socket = client;

            lock (_statusLock)
            {
                _clientStatus = WebPubSubClientStatus.Connected;
            }

            _ = Task.Run(() => ListenLoop(client, CancellationToken.None), token);
        }

        private static WebPubSubClientOptions BuildDefaultClientOptions()
        {
            return new WebPubSubClientOptions
            {
                Protocol = new WebPubSubJsonReliableProtocol(),
            };
        }

        private static SendToGroupOptions BuildDefaultSendToGroupOptions()
        {
            return new SendToGroupOptions
            {
                NoEcho = false,
                FireAndForget = false,
            };
        }

        private static SendToServerOptions BuildDefaultSendToServerOptions()
        {
            return new SendToServerOptions
            {
                FireAndForget = false,
            };
        }

        private async Task ListenLoop(WebSocket socket, CancellationToken token)
        {
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(token, _stopCts.Token).Token;

            using var buffer = new MemoryBufferWriter();
            try
            {
                while (socket.State == WebSocketState.Open && !linkedToken.IsCancellationRequested)
                {
                    buffer.Reset();

                    var type = await ReceiveOneFrameAsync(buffer, socket, linkedToken).ConfigureAwait(false);

                    if (type == WebSocketMessageType.Close)
                    {
                        await socket.CloseOutputAsync(socket.CloseStatus ?? WebSocketCloseStatus.EndpointUnavailable, null, default).ConfigureAwait(false);
                        return;
                    }

                    if (buffer.Length != 0)
                    {
                        try
                        {
                            var message = _protocol.ParseMessage(buffer.AsReadOnlySequence());
                            await HandleMessage(message).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            finally
            {
                if (socket.CloseStatus == WebSocketCloseStatus.PolicyViolation)
                {
                    _ = Task.Run(() => RaiseDisconnected(_latestDisconnectedMessage), CancellationToken.None);
                }
                else
                {
                    lock (_statusLock)
                    {
                        _clientStatus = WebPubSubClientStatus.Suspended;
                    }
                    _ = Task.Run(() => TryRecovery(), CancellationToken.None);
                }
            }
        }

        private static async Task<WebSocketMessageType> ReceiveOneFrameAsync(IBufferWriter<byte> buffer, WebSocket socket, CancellationToken token)
        {
            // Do a 1 byte read so that idle connections don't allocate a buffer when waiting for a read
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var memory = buffer.GetMemory();
            var receiveResult = await socket.ReceiveAsync(memory, token).ConfigureAwait(false);

            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseOutputAsync(socket.CloseStatus ?? WebSocketCloseStatus.EndpointUnavailable, null, default).ConfigureAwait(false);
                return WebSocketMessageType.Close;
            }

            buffer.Advance(receiveResult.Count);

            while (!receiveResult.EndOfMessage)
            {
                memory = buffer.GetMemory();
                receiveResult = await socket.ReceiveAsync(memory, token).ConfigureAwait(false);

                // Need to check again for NetCoreApp2.2 because a close can happen between a 0-byte read and the actual read
                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseOutputAsync(socket.CloseStatus ?? WebSocketCloseStatus.EndpointUnavailable, null, default).ConfigureAwait(false);
                    return WebSocketMessageType.Close;
                }

                buffer.Advance(receiveResult.Count);
            }

            return receiveResult.MessageType;
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

            return new ConnectionEndpoint { BasePath = baseUri, FullEndpointUrl = clientAccessUri };
        }

        private Task SendCoreAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            var socket = _socket;
            if (_clientStatus != WebPubSubClientStatus.Connected)
            {
                throw new SendMessageFailedException("Connection is not in open state");
            }
            return _socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        private async Task SendCoreAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            var socket = _socket;
            if (socket == null || socket.State != WebSocketState.Open)
            {
                throw new SendMessageFailedException("Connection is not in open state");
            }
            await socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
        }

        private async Task<AckMessage> SendMessageWithAckId(Func<ulong, CancellationToken, Task> func, ulong? ackId, CancellationToken token)
        {
            var id = ackId ?? NextAckId();
            var entity = CreateAckEntity(id);
            await func(id, token).ConfigureAwait(false);
            return await entity.Task.ConfigureAwait(false);
        }

        private Task RaiseDisconnected(DisconnectedMessage disconnectedMessage)
        {
            lock (_statusLock)
            {
                _clientStatus = WebPubSubClientStatus.Disconnected;
            }

            foreach (var entity in _ackCache)
            {
                if (_ackCache.TryRemove(entity.Key, out var value))
                {
                    value.SetException(new SendMessageFailedException("Connection is disconnected before receive ack from the service"));
                }
            }

            return Disconnected.RaiseAsync(new DisconnectedEventArgs(disconnectedMessage, false), nameof(DisconnectedEventArgs), nameof(Disconnected));
        }

        private Task RaiseConnected(ConnectedMessage connectedMessage)
        {
            return Connected.RaiseAsync(new ConnectedEventArgs(connectedMessage, false), nameof(ConnectedEventArgs), nameof(Connected));
        }

        private Task RaiseServerMessageReceived(ServerResponseMessage serverResponseMessage)
        {
            return ServerMessageReceived.RaiseAsync(new ServerMessageEventArgs(serverResponseMessage, false), nameof(ServerMessageEventArgs), nameof(ServerMessageReceived));
        }

        private async Task TryRecovery()
        {
            // Called StopAsync, don't recover.
            if (_stopCts.IsCancellationRequested)
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
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    await ConnectCoreAsync(uri, CancellationToken.None).ConfigureAwait(false);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
        }

        private async Task HandleMessage(WebPubSubMessage message)
        {
            switch (message)
            {
                case ConnectedMessage connectedMessage:
                    await HandleConnectedMessage(connectedMessage).ConfigureAwait(false);
                    break;
                case DisconnectedMessage disconnectedMessage:
                    await HandleDisconnectedMessage(disconnectedMessage).ConfigureAwait(false);
                    break;
                case GroupResponseMessage groupResponseMessage:
                    await HandleGroupMessage(groupResponseMessage).ConfigureAwait(false);
                    break;
                case ServerResponseMessage serverResponseMessage:
                    await HandleServerMessage(serverResponseMessage).ConfigureAwait(false);
                    break;
                case AckMessage ackMessage:
                    await HandleAckMessage(ackMessage).ConfigureAwait(false);
                    break;
                default:
                    throw new InvalidDataException();
            }

            async Task HandleConnectedMessage(ConnectedMessage connectedMessage)
            {
                _connectionId = connectedMessage.ConnectionId;
                _reconnectionToken = connectedMessage.ReconnectionToken;

                if (!_isInitialConnected)
                {
                    _isInitialConnected = true;
                    await RaiseConnected(connectedMessage).ConfigureAwait(false);
                }
            }

            Task HandleDisconnectedMessage(DisconnectedMessage disconnectedMessage)
            {
                _latestDisconnectedMessage = disconnectedMessage;
                return Task.CompletedTask;
            }

            async Task HandleGroupMessage(GroupResponseMessage groupResponseMessage)
            {
                if (groupResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(groupResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return;
                    }
                }

                if (_groupEventHandlers.TryGetValue(groupResponseMessage.Group, out var handler))
                {
                    await (handler?.Invoke(groupResponseMessage) ?? Task.CompletedTask).ConfigureAwait(false);
                }
            }

            Task HandleServerMessage(ServerResponseMessage serverResponseMessage)
            {
                if (serverResponseMessage.SequenceId != null)
                {
                    if (!_sequenceId.TryUpdate(serverResponseMessage.SequenceId.Value))
                    {
                        // drop duplicated msg
                        return Task.CompletedTask;
                    }
                }

                return RaiseServerMessageReceived(serverResponseMessage);
            }

            Task HandleAckMessage(AckMessage ackMessage)
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

        private async Task PerformSequenceAckAsync(ulong sequenceId)
        {
            

            if (sequenceId > _latestSequenceId)
            {
                _latestSequenceId = sequenceId;

                var payload = _protocol.GetMessageBytes(new SequenceAckMessage(_latestSequenceId));

                try
                {
                    await SendCoreAsync(payload, _protocol.WebSocketMessageType, true, CancellationToken.None).ConfigureAwait(false);
                }
                catch
                {
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
                if (_clientStatus != WebPubSubClientStatus.Connected)
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

        private class ConnectionEndpoint
        {
            public Uri FullEndpointUrl { get; set; }

            public string BasePath { get; set; }
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
    }
}
