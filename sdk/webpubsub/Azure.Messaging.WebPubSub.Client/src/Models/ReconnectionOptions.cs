// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// The set of options that used to specify reconnection behaviors
    /// </summary>
    public class ReconnectionOptions
    {
        /// <summary>
        /// Whether to auto reconnect
        /// </summary>
        public bool AutoReconnect { get; set; } = true;
    }
}
