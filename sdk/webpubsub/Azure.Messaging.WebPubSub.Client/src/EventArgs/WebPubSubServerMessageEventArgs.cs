// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for message from server or groups
    /// </summary>
    public class WebPubSubServerMessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The server data message
        /// </summary>
        public ServerDataMessage Message { get; }

        internal WebPubSubServerMessageEventArgs(ServerDataMessage message, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            Message = message;
        }
    }
}
