// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    public class ServerMessageEventArgs : SyncAsyncEventArgs
    {
        public ServerResponseMessage ServerResponseMessage { get; }

        public ServerMessageEventArgs(ServerResponseMessage serverResponseMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ServerResponseMessage = serverResponseMessage;
        }
    }
}
