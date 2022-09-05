// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The event args for message from server or groups
    /// </summary>
    public class MessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The data message
        /// </summary>
        public DataMessage DataMessage { get; }

        internal MessageEventArgs(DataMessage message, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            DataMessage = message;
        }
    }
}
