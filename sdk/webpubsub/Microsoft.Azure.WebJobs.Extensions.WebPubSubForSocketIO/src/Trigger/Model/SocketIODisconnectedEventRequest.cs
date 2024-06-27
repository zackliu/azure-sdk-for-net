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
    public class SocketIODisconnectedEventRequest : SocketIOEventRequest
    {
        internal const string ReasonProperty = "reason";

        /// <summary>
        /// Reason of the disconnect event.
        /// </summary>
        [DataMember(Name = ReasonProperty)]
        [JsonPropertyName(ReasonProperty)]
        public string Reason { get; }

        /// <summary>
        /// The disconnected event request
        /// </summary>
        public SocketIODisconnectedEventRequest(WebPubSubConnectionContext context, string @namespace, string socketId, string reason) : base(context, @namespace, socketId)
        {
            Reason = reason;
        }
    }
}
