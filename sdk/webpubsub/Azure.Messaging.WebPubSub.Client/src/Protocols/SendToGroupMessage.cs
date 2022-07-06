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
    /// The message representing sending message to group.
    /// </summary>
    public class SendToGroupMessage : WebPubSubMessage
    {
        /// <summary>
        /// The group name
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// The optional ack-id
        /// </summary>
        public ulong? AckId { get; }

        /// <summary>
        /// Optional. If set to true, this message is not echoed back to the same connection.
        /// </summary>
        public bool NoEcho { get; }

        /// <summary>
        /// Type of the data
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// The data content
        /// </summary>
        public ReadOnlySequence<byte> Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendToGroupMessage"/> class.
        /// </summary>
        /// <param name="group">The group name</param>
        /// <param name="data">The data content</param>
        /// <param name="dataType">Type of the data</param>
        /// <param name="ackId">The optional ack-id</param>
        /// <param name="noEcho">Optional. If set to true, this message is not echoed back to the same connection.</param>
        public SendToGroupMessage(string group, ReadOnlySequence<byte> data, DataType dataType, ulong? ackId, bool noEcho)
        {
            Group = group;
            AckId = ackId;
            NoEcho = noEcho;
            DataType = dataType;
            Data = data;
        }
    }
}
