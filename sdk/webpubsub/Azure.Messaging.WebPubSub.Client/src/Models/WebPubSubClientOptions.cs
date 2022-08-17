// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Azure.Core;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The set of options that can be specified when creating <see cref="WebPubSubClient"/> instance.
    /// </summary>
    public class WebPubSubClientOptions
    {
        /// <summary>
        /// The default retry options for sending messages.
        /// </summary>
        public RetryOptions MessageRetryOptions { get; set; }

        /// <summary>
        /// Get or set the protocol to use.
        /// </summary>
        public IWebPubSubProtocol Protocol { get; set; }
    }
}
