// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.WebPubSub.Client.Models;

namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    /// <summary>
    /// The message representing SequenceAck.
    /// </summary>
    public class SequenceAckMessage : WebPubSubMessage
    {
        /// <summary>
        /// The sequenceId
        /// </summary>
        public ulong SequenceId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceAckMessage"/> class.
        /// </summary>
        /// <param name="sequenceId">The sequenceId</param>
        public SequenceAckMessage(ulong sequenceId)
        {
            SequenceId = sequenceId;
        }
    }
}
