// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// ConnectedEventArgs
    /// </summary>
    public class ConnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// ConnectedMessage
        /// </summary>
        public ConnectedMessage ConnectedMessage { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectedMessage"></param>
        /// <param name="isRunningSynchronously"></param>
        /// <param name="cancellationToken"></param>
        public ConnectedEventArgs(ConnectedMessage connectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ConnectedMessage = connectedMessage;
        }
    }
}
