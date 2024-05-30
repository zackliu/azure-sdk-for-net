// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// Operation to send message to a socket
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SendToSocketAction : WebPubSubForSocketIOAction
    {
        /// <summary>
        /// Target namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Target socketID.
        /// </summary>
        public string SocketId { get; set; }

        /// <summary>
        /// Message to send.
        /// </summary>
        [JsonConverter(typeof(BinaryDataJsonConverter))]
        public BinaryData Data { get; set; }
    }
}
