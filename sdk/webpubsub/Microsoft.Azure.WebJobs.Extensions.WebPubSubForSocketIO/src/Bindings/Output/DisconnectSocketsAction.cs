// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// Operation to add socket to a room.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class DisconnectSocketsAction : WebPubSubForSocketIOAction
    {
        /// <summary>
        /// Target namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Optional target rooms
        /// </summary>
        public IList<string> Rooms { get; set; } = new List<string>();
    }
}
