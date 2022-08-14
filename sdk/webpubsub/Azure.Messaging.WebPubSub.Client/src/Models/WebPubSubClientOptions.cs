// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.WebPubSub.Client.Protocols;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The set of options that can be specified when creating <see cref="WebPubSubClient"/> instance.
    /// </summary>
    public class WebPubSubClientOptions
    {
        /// <summary>
        /// Set or get the retry policy used when reconnection. When a connection is disconnected and not recoverable,
        /// the library will use this policy to start a new connection.
        /// </summary>
        public WebPubSubRetryPolicy ReconnectionPolicy { get; set; }

        /// <summary>
        /// Set or get the retry policy used when recovery. For reliable subprotocols, when a connection is temprary dropped,
        /// the library will use this policy to try recovering the connection.
        /// </summary>
        public WebPubSubRetryPolicy RecoveryPolicy { get; set; }

        public WebPubSubRetryPolicy MessageRetryPolicy { get; set; }

        /// <summary>
        /// Get or set the protocol to use.
        /// </summary>
        public IWebPubSubProtocol Protocol { get; set; }
    }
}
