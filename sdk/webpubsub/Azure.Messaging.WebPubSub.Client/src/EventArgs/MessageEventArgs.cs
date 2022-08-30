// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// ServerResponseMessage
    /// </summary>
    public class MessageEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// ServerResponseMessage
        /// </summary>
        public DataResponseMessage Message { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isRunningSynchronously"></param>
        /// <param name="cancellationToken"></param>
        public MessageEventArgs(DataResponseMessage message, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            Message = message;
        }
    }
}
