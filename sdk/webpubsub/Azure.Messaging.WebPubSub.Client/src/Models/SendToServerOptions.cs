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
    public class SendToServerOptions
    {
        /// <summary>
        /// Specify whether the message need to be fire-and-forget.
        /// </summary>
        public bool FireAndForget { get; set; }

        public WebPubSubRetryPolicy MessageRetryPolicy { get; set; }
    }
}
