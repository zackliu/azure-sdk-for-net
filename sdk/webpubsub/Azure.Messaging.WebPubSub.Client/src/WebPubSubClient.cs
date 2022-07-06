// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Messaging.WebPubSub.Client.Models;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The WebPubSubService PubSub client.
    /// </summary>
    [SuppressMessage("Usage", "AZC0007:DO provide a minimal constructor that takes only the parameters required to connect to the service.", Justification = "WebPubSub clients are Websocket based and don't use ClientOptions functionality")]
    [SuppressMessage("Usage", "AZC0004:DO provide both asynchronous and synchronous variants for all service methods.", Justification = "Synchronous methods doesn't make sense in the scenario of WebPubSub client")]
    [SuppressMessage("Usage", "AZC0015:Unexpected client method return type.", Justification = "WebPubSubClient is a pure data plane client that don't need to return type as a management client does.")]
    public class WebPubSubClient : IAsyncDisposable
    {
        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="uri">The uri to connect to the service.</param>
        public WebPubSubClient(Uri uri): this(() => uri, null)
        {
        }

        /// <summary>
        /// Initializes a PubSub client.
        /// </summary>
        /// <param name="uriProvider">A uri provider that will be called to return the uri for each connecting or reconnecting.</param>
        /// <param name="clientOptions">A option for the client.</param>
        public WebPubSubClient(Func<Uri> uriProvider, WebPubSubClientOptions clientOptions)
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
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task ConnectAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop and close the client to the service
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Join the target group.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task JoinGroupAsync(string groupName, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Leave the target group.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task LeaveGroupAsync(string groupName, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Publish data to group with fire-and-forget.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="operations">A set of options used while sending to group.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendToGroupAsync(string groupName, RequestContent content, DataType dataType, SendToGroupOptions operations = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Publish data to group and wait for the ack.
        /// </summary>
        /// <param name="groupName">The group name.</param>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="operations">A set of options used while sending to group.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendToGroupWithAckAsync(string groupName, RequestContent content, DataType dataType, SendToGroupOptions operations = null, ulong? ackId = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send custom event with fire-and-forget.
        /// </summary>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendEventAsync(RequestContent content, DataType dataType, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send custom event and wait for the ack.
        /// </summary>
        /// <param name="content">The data content.</param>
        /// <param name="dataType">The data type.</param>
        /// <param name="ackId">The ack-id for the operation. The message with the same ack-id is treated as the same message. Leave it omitted to generate by library.</param>
        /// <param name="token">An optional <see cref="CancellationToken" /> instance to signal the request to cancel the operation.</param>
        /// <returns></returns>
        public virtual Task SendEventWithAckAsync(RequestContent content, DataType dataType, ulong? ackId = null, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An event triggered when the connection is connected
        /// </summary>
        public event Func<ConnectedMessage, Task> OnConnected;

        /// <summary>
        /// An event triggered when the connection is disconnected
        /// </summary>
        public event Func<DisconnectedMessage, Task> OnDisconnected;

        /// <summary>
        /// An event triggered when the connection is suspended
        /// </summary>
        public event Func<DisconnectedMessage, Task> OnSuspended;

        /// <summary>
        /// Dispose and close the client.
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return default;
        }
    }
}
