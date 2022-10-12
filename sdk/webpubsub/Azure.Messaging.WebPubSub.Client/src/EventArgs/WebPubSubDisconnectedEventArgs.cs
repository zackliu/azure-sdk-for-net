// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for disconnected
    /// </summary>
    public class WebPubSubDisconnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The disconnected message
        /// </summary>
        public DisconnectedMessage DisconnectedMessage { get; }

        internal WebPubSubDisconnectedEventArgs(DisconnectedMessage disconnectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            DisconnectedMessage = disconnectedMessage;
        }
    }
}
