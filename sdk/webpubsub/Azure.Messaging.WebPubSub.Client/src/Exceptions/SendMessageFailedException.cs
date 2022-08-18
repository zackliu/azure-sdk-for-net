// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// Exception for sending message failed
    /// </summary>
    public class SendMessageFailedException : Exception
    {
        /// <summary>
        /// The optional AckMessage if the exception is caused by non-success status
        /// </summary>
        public AckMessage AckMessage { get; }

        internal SendMessageFailedException(string message, AckMessage ackMessage) : base(message)
        {
            AckMessage = ackMessage;
        }

        internal SendMessageFailedException(string message): base(message)
        {
        }
    }
}
