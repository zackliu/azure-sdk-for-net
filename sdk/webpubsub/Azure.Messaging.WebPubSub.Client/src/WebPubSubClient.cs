// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
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

        private ConnectionEndpoint _connnectionEndpoint;
        private string _connectionId;
        private string _reconnectionToken;

        private volatile bool _disposing;
        private volatile bool _started;

        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="clientAccessUri">The uri to connect to the service.</param>
        public WebPubSubClient(Uri clientAccessUri) : this(new WebPubSubClientCredential(clientAccessUri), null)
        {
        }

        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="credential">A uri provider that will be called to return the uri for each connecting or reconnecting.</param>
        /// <param name="options">A option for the client.</param>
        public WebPubSubClient(WebPubSubClientCredential credential, WebPubSubClientOptions options)
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
            if (_disposing)
            {
                throw new ObjectDisposedException("The client is already disposed");
            }

            if (_started)
            {
                return;
            }

            var uri = await _webPubSubClientCredential.GetClientAccessUri(default).ConfigureAwait(false);
            _connnectionEndpoint = ParseClientAccessUri(uri);
            var client = await ConnectCoreAsync(_connnectionEndpoint.FullEndpointUrl, cancellationToken).ConfigureAwait(false);
            _started = true;
            _ = Task.Run(() => ListenLoop(client), cancellationToken);
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Join the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="handler">The handler function to handle group message.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation.</returns>
        public virtual Task<AckMessage> JoinGroupAsync(string group, Func<GroupResponseMessage, Task> handler, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Leave the target group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual Task<AckMessage> LeaveGroupAsync(string group, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Publish data to group with fire-and-forget.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="operations">A set of options used while sending to group.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendToGroupAsync(string group, RequestContent content, DataType dataType, SendToGroupOptions operations = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Publish data to group and wait for the ack.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="operations">A set of options used while sending to group.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual Task<AckMessage> SendToGroupWithAckAsync(string group, RequestContent content, DataType dataType, SendToGroupOptions operations = null, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send custom event with fire-and-forget.
        /// </summary>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendToServerAsync(RequestContent content, DataType dataType, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send custom event and wait for the ack.
        /// </summary>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns>The ack for the operation</returns>
        public virtual Task<AckMessage> SendToServerWithAckAsync(RequestContent content, DataType dataType, ulong? ackId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
        /// An event triggered when the connection is suspended
        /// </summary>
        public event SyncAsyncEventHandler<DisconnectedEventArgs> Suspended;

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

        protected virtual void Dispose(bool disposing)
        {
            _socket?.Dispose();
        }

        private async Task<WebSocket> ConnectCoreAsync(Uri uri, CancellationToken token)
        {
            var client = new ClientWebSocket();
            client.Options.AddSubProtocol(_options.Protocol.Name);

            await client.ConnectAsync(uri, token).ConfigureAwait(false);
            _socket = client;
            return client;
        }

        private WebPubSubClientOptions BuildDefaultClientOptions()
        {

        }

        private Task ListenLoop(WebSocket socket)
        {
            var disableReconnection = false;
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var ms = new MemoryStream();
                    Memory<byte> buffer = new byte[1 << 10];
                    // receive loop
                    while (true)
                    {
                        var receiveResult = await _socket.ReceiveAsync(buffer, default);
                        // Need to check again for NetCoreApp2.2 because a close can happen between a 0-byte read and the actual read
                        if (receiveResult.MessageType == WebSocketMessageType.Close)
                        {
                            if (_socket.CloseStatus == WebSocketCloseStatus.PolicyViolation)
                            {
                                disableReconnection = true;
                            }
                            try
                            {
                                _logger.LogInformation($"The connection dropped, connectionId:{_connectionId}, status code:{_socket.CloseStatus},description: {_socket.CloseStatusDescription}");

                                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, default);
                            }
                            catch (Exception e)
                            {
                                // It is possible that the remote is already closed
                                _logger.LogError(e, $"The connection dropped, connectionId:{_connectionId}, status code:{_socket.CloseStatus}, description: {_socket.CloseStatusDescription}");
                            }

                            break;
                        }

                        await ms.WriteAsync(buffer.Slice(0, receiveResult.Count));

                        if (receiveResult.EndOfMessage)
                        {
                            var str = Encoding.UTF8.GetString(ms.ToArray());
                            HandleMessage(str);
                            ms.SetLength(0);
                        }
                    }
                }
            }
            finally
            {
                if (!disableReconnection)
                {
                    _ = Task.Run(() => TryRecover());
                }
                else
                {
                    OnClosed();
                }
            }

            void HandleMessage(string str)
            {
                try
                {
                    var obj = JObject.Parse(str);
                    if (obj.TryGetValue("type", out var type) &&
                        string.Equals(type.Value<string>(), "system", StringComparison.OrdinalIgnoreCase) &&
                        obj.TryGetValue("event", out var @event) &&
                        string.Equals(@event.Value<string>(), "connected", StringComparison.OrdinalIgnoreCase))
                    {
                        // handle connected event
                        var connected = obj.ToObject<ConnectedMessage>();
                        if (!string.IsNullOrEmpty(connected?.connectionId))
                        {
                            _connectionId = connected.connectionId;
                        }
                        if (!string.IsNullOrEmpty(connected?.reconnectionToken))
                        {
                            _reconnectionToken = connected.reconnectionToken;
                        }
                    }
                    else
                    {
                        var response = JsonConvert.DeserializeObject<MessageResponse>(str);

                        if (response.sequenceId != null)
                        {
                            if (!_sequenceId.UpdateSequenceId(response.sequenceId.Value))
                            {
                                // duplicated message
                                return;
                            }
                        }

                        if (response.data != null)
                        {
                            var data = JsonConvert.DeserializeObject<RawWebsocketData>(response.data);
                            OnMessage?.Invoke(data.Ticks, data.Payload);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Handle message failed.");
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

            return new ConnectionEndpoint { BasePath = baseUri, FullEndpointUrl = clientAccessUri };
        }
    }

    internal class ConnectionEndpoint
    {
        public Uri FullEndpointUrl { get; set; }

        public string BasePath { get; set; }
    }
}
