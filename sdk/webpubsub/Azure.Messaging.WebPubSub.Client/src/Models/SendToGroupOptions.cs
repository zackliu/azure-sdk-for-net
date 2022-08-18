// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// A set of options when sending data to group.
    /// </summary>
    public class SendToGroupOptions
    {
        /// <summary>
        /// If set to true, this message is not echoed back to the same connection. If not set, the default value is false.
        /// </summary>
        public bool NoEcho { get; set; }

        /// <summary>
        /// Specify whether the message need to be fire-and-forget.
        /// </summary>
        public bool FireAndForget { get; set; }
    }
}
