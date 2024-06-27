using Microsoft.Azure.WebPubSub.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Trigger.Model
{
    /// <summary>
    /// Connect event
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(SocketIOConnectEventRequest))]
    public class SocketIOMessageEventRequest : SocketIOEventRequest
    {
        internal const string PayloadProperty = "payload";

        /// <summary>
        /// The payload of message
        /// </summary>
        [DataMember(Name = PayloadProperty)]
        [JsonPropertyName(PayloadProperty)]
        public string Payload { get; }

        /// <summary>
        /// The disconnected event request
        /// </summary>
        public SocketIOMessageEventRequest(WebPubSubConnectionContext context, string @namespace, string socketId, string payload) : base(context, @namespace, socketId)
        {
            Payload = payload;
        }
    }
}
