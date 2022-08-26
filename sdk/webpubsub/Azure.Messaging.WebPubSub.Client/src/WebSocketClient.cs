// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    internal sealed class WebSocketClient : IDisposable
    {
        private readonly ClientWebSocket _socket;
        private readonly Uri _uri;
        private readonly IWebPubSubProtocol _protocol;

        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1);
        private readonly TaskCompletionSource<object> _stoppedTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 0 = not active, 1 = actively receiving
        private int _isReceiving;

        public WebSocketCloseStatus? CloseStatus => _socket.CloseStatus;

        public WebSocketClient(Uri uri, IWebPubSubProtocol protocol)
        {
            _protocol = protocol;
            _socket = new ClientWebSocket();
            _socket.Options.AddSubProtocol(_protocol.Name);
            _uri = uri;
        }

        public void Dispose()
        {
            _sendLock.Dispose();
            _socket.Dispose();
        }

        public async Task ConnectAsync(CancellationToken token)
        {
            WebPubSubClientEventSource.Log.ConnectionStarting(_protocol.Name);

            await _socket.ConnectAsync(_uri, token).ConfigureAwait(false);
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await _socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public async Task SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await _socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public async Task StartReceive(IMessageHandler messageHandler, CancellationToken token)
        {
            if (Interlocked.CompareExchange(ref _isReceiving, 1, 0) != 0)
            {
                throw new InvalidOperationException("The client is already start receiving");
            }

            using var buffer = new MemoryBufferWriter();
            try
            {
                while (_socket.State == WebSocketState.Open && !token.IsCancellationRequested)
                {
                    buffer.Reset();

                    var type = await ReceiveOneFrameAsync(buffer, _socket, token).ConfigureAwait(false);

                    if (type == WebSocketMessageType.Close)
                    {
                        if (_socket.State == WebSocketState.CloseReceived)
                        {
                            await _socket.CloseOutputAsync(_socket.CloseStatus ?? WebSocketCloseStatus.EndpointUnavailable, null, default).ConfigureAwait(false);
                        }

                        return;
                    }

                    if (buffer.Length != 0)
                    {
                        try
                        {
                            var message = _protocol.ParseMessage(buffer.AsReadOnlySequence());
                            await messageHandler.HandleMessageAsync(message, token).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            WebPubSubClientEventSource.Log.FailedToHandleMessage(ex.Message);
                        }
                    }
                }
            }
            finally
            {
                _stoppedTcs.SetResult(true);
            }
        }

        public async Task StopAsync(CancellationToken token)
        {
            try
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, token).ConfigureAwait(false);
            }
            finally
            {
                if (_isReceiving == 1)
                {
                    await _stoppedTcs.Task.AwaitWithCancellation(token);
                }
            }
        }

        internal void Abort()
        {
            _socket.Abort();
        }

        private static async Task<WebSocketMessageType> ReceiveOneFrameAsync(IBufferWriter<byte> buffer, WebSocket socket, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var memory = buffer.GetMemory();
            var receiveResult = await socket.ReceiveAsync(memory, token).ConfigureAwait(false);

            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
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
                    return WebSocketMessageType.Close;
                }

                buffer.Advance(receiveResult.Count);
            }

            return receiveResult.MessageType;
        }
    }
}
