// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.WebPubSub.Client.Protocols;

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor for mock.
        /// </summary>
        protected WebPubSubClient()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start connecting to the service.
        /// </summary>
        /// <param name="cancellationToken">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
        /// <returns></returns>
        public virtual ValueTask Dispose()
        {
            GC.SuppressFinalize(this);
            return default;
        }
    }
}
