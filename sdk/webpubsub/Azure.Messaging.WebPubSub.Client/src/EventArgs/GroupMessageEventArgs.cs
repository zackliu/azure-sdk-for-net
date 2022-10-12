// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for message from groups
    /// </summary>
    public class GroupMessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The group data message.
        /// </summary>
        public GroupDataMessage Message { get; }

        internal GroupMessageEventArgs(GroupDataMessage groupResponseMessage, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            Message = groupResponseMessage;
        }
    }
}
