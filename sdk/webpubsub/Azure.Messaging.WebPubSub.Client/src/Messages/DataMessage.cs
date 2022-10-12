// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The message representing the data from groups or from servers
    /// </summary>
    internal abstract class DataMessage : WebPubSubMessage
    {
        /// <summary>
        /// Type of the data
        /// </summary>
        public WebPubSubDataType DataType { get; }

        /// <summary>
        /// The data content
        /// </summary>
        public BinaryData Data { get; }

        /// <summary>
        /// The sequence id. Only availble in reliable protocol.
        /// </summary>
        public ulong? SequenceId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMessage"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data</param>
        /// <param name="data">The data content</param>
        /// <param name="sequenceId">The sequence id. Only availble in reliable protocol.</param>
        protected DataMessage(WebPubSubDataType dataType, BinaryData data, ulong? sequenceId)
        {
            DataType = dataType;
            Data = data;
            SequenceId = sequenceId;
        }
    }
}
