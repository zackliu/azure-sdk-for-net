// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The message representing sending event.
    /// </summary>
    public class SendEventMessage : WebPubSubMessage
    {
        /// <summary>
        /// The optional ack-id
        /// </summary>
        public ulong? AckId { get; }

        /// <summary>
        /// Type of the data
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// The data content
        /// </summary>
        public BinaryData Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEventMessage"/> class.
        /// </summary>
        /// <param name="data">The data content</param>
        /// <param name="dataType">Type of the data</param>
        /// <param name="ackId">The optional ack-id</param>
        public SendEventMessage(BinaryData data, DataType dataType, ulong? ackId)
        {
            AckId = ackId;
            DataType = dataType;
            Data = data;
        }
    }
}
