// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// Exception for process message failed
    /// </summary>
    public class ProcessMessageFailedException : Exception
    {
        internal ProcessMessageFailedException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
