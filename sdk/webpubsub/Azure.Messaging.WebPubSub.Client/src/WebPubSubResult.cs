// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// To represent the result of ack-able operations.
    /// </summary>
    public class WebPubSubResult
    {
        /// <summary>
        /// The ack id of message just sent.
        /// </summary>
        public ulong AckId { get; }

        internal WebPubSubResult(ulong ackId)
        {
            AckId = ackId;
        }
    }
}
