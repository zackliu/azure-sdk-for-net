// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The event args for disconnected
    /// </summary>
    public class DisconnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The disconnected message
        /// </summary>
        public DisconnectedMessage DisconnectedMessage { get; }

        internal DisconnectedEventArgs(DisconnectedMessage disconnectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            DisconnectedMessage = disconnectedMessage;
        }
    }
}
