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
    /// The message representing the response from server.
    /// </summary>
    public class ServerResponseMessage : WebPubSubMessage
    {
        /// <summary>
        /// Type of the data
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// The data content
        /// </summary>
        public ReadOnlySequence<byte> Data { get; }

        /// <summary>
        /// The sequence id. Only availble in reliable protocol.
        /// </summary>
        public ulong? SequenceId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerResponseMessage"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data</param>
        /// <param name="data">The data content</param>
        /// <param name="sequenceId">The sequence id. Only availble in reliable protocol.</param>
        protected ServerResponseMessage(DataType dataType, ReadOnlySequence<byte> data, ulong? sequenceId)
        {
            DataType = dataType;
            Data = data;
            SequenceId = sequenceId;
        }
    }
}
