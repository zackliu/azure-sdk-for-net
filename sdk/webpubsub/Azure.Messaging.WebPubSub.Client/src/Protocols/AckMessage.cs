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
    /// The message representing Ack.
    /// </summary>
    public class AckMessage : WebPubSubMessage
    {
        /// <summary>
        /// The ack-id
        /// </summary>
        public ulong AckId { get; }

        /// <summary>
        /// Representing whether the operation is success.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// The error detial when the operation is not success.
        /// </summary>
        public ErrorDetail Error { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AckMessage"/> class.
        /// </summary>
        /// <param name="ackId">The ack-id</param>
        /// <param name="success">Representing whether the operation is success.</param>
        /// <param name="error">The error detial when the operation is not success.</param>
        public AckMessage(ulong ackId, bool success, ErrorDetail error)
        {
            AckId = ackId;
            Success = success;
            Error = error;
        }
    }
}
