// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// Operation to send message to a namespace
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SendToNamespaceAction : WebPubSubForSocketIOAction
    {
        /// <summary>
        /// Target namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Message to send.
        /// </summary>
        public IList<string> Data { get; set; }
    }
}
