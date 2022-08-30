// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The message representing the response from server.
    /// </summary>
    public class ServerResponseMessage : DataResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerResponseMessage"/> class.
        /// </summary>
        /// <param name="dataType">Type of the data</param>
        /// <param name="data">The data content</param>
        /// <param name="sequenceId">The sequence id. Only availble in reliable protocol.</param>
        public ServerResponseMessage(WebPubSubDataType dataType, BinaryData data, ulong? sequenceId) : base(dataType, data, sequenceId)
        {
        }
    }
}
