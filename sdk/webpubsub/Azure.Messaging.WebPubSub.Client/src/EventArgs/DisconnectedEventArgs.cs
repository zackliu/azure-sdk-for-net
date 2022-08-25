// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// DisconnectedEventArgs
    /// </summary>
    public class DisconnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// DisconnectedMessage
        /// </summary>
        public DisconnectedMessage DisconnectedMessage { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="disconnectedMessage"></param>
        /// <param name="isRunningSynchronously"></param>
        /// <param name="cancellationToken"></param>
        public DisconnectedEventArgs(DisconnectedMessage disconnectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            DisconnectedMessage = disconnectedMessage;
        }
    }
}
