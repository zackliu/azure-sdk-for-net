// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// ServerResponseMessage
    /// </summary>
    public class ServerMessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// ServerResponseMessage
        /// </summary>
        public ServerResponseMessage ServerResponseMessage { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serverResponseMessage"></param>
        /// <param name="isRunningSynchronously"></param>
        /// <param name="cancellationToken"></param>
        public ServerMessageEventArgs(ServerResponseMessage serverResponseMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ServerResponseMessage = serverResponseMessage;
        }
    }
}
