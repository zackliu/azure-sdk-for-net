// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The set of options that can be specified when creating <see cref="WebPubSubClient"/> instance.
    /// </summary>
    public class WebPubSubClientOptions
    {
        /// <summary>
        /// Get or set the protocol to use.
        /// </summary>
        public IWebPubSubProtocol Protocol { get; set; }

        /// <summary>
        /// Get or set the reconnection options
        /// </summary>
        public ReconnectionOptions ReconnectionOptions { get; set; }

        /// <summary>
        /// Construct a WebPubSubClientOptions
        /// </summary>
        public WebPubSubClientOptions()
        {
            Protocol = new WebPubSubJsonReliableProtocol();
            ReconnectionOptions = new ReconnectionOptions();
        }
    }
}
