// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    public class ConnectedEventArgs : SyncAsyncEventArgs
    {
        public ConnectedMessage ConnectedMessage { get; }

        public ConnectedEventArgs(ConnectedMessage connectedMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ConnectedMessage = connectedMessage;
        }
    }
}
