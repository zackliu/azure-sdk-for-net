// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for stopped
    /// </summary>
    public class WebPubSubStoppedEventArgs : SyncAsyncEventArgs
    {
        internal WebPubSubStoppedEventArgs(bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
        }
    }
}
