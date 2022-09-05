// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The event args for connected
    /// </summary>
    public class ConnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The connected message.
        /// </summary>
        public ConnectedMessage ConnectedMessage { get; }

        internal ConnectedEventArgs(ConnectedMessage connectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ConnectedMessage = connectedMessage;
        }
    }
}
