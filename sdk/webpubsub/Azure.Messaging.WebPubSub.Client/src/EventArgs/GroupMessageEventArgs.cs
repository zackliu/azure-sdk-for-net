// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// ServerResponseMessage
    /// </summary>
    public class GroupMessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// ServerResponseMessage
        /// </summary>
        public GroupResponseMessage GroupResponseMessage { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="groupResponseMessage"></param>
        /// <param name="isRunningSynchronously"></param>
        /// <param name="cancellationToken"></param>
        public GroupMessageEventArgs(GroupResponseMessage groupResponseMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            GroupResponseMessage = groupResponseMessage;
        }
    }
}
