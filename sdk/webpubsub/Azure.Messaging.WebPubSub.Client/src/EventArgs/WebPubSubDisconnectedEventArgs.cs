// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for disconnected
    /// </summary>
    public class WebPubSubDisconnectedEventArgs
    {
        /// <summary>
        /// The disconnected message
        /// </summary>
        public DisconnectedMessage DisconnectedMessage { get; }

        /// <summary>
        /// Gets a cancellation token related to the original operation that raised the event.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        internal WebPubSubDisconnectedEventArgs(DisconnectedMessage disconnectedMessage, CancellationToken cancellationToken = default)
        {
            DisconnectedMessage = disconnectedMessage;
            CancellationToken = cancellationToken;
        }
    }
}
