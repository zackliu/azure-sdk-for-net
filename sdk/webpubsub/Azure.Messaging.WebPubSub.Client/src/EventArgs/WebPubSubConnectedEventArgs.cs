// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The event args for connected
    /// </summary>
    public class WebPubSubConnectedEventArgs : SyncAsyncEventArgs
    {
        /// <summary>
        /// The connected message.
        /// </summary>
        public ConnectedMessage ConnectedMessage { get; }

        /// <summary>
        /// Groups that currently the client should in from client sdk's perspective. Groups that join or leave from server won't be taken into consideration.
        /// E.g. Client A:  Join Group A ----------------> Leave Group A ------------> Join Group B ----------------> Reconnect
        ///      Server:                                                                             Leave Group B
        /// Then you will get Group B in the List. Because client can't recognize the operation from server.
        /// </summary>
        public IReadOnlyDictionary<string, Exception> GroupRestoreState { get; }

        internal WebPubSubConnectedEventArgs(ConnectedMessage connectedMessage, IReadOnlyDictionary<string, Exception> groupRestoreState, bool isRunningSynchronously, CancellationToken cancellationToken = default) : base(isRunningSynchronously, cancellationToken)
        {
            ConnectedMessage = connectedMessage;
            GroupRestoreState = groupRestoreState;
        }
    }
}
