using Microsoft.Azure.WebPubSub.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Trigger.Model
{
    /// <summary>
    /// Connected event
    /// </summary>
    [DataContract]
    [JsonConverter(typeof(SocketIOConnectEventRequest))]
    public class SocketIOConnectedEventRequest : SocketIOEventRequest
    {
        /// <summary>
        /// Construct a SocketIOConnectedEventRequest
        /// </summary>
        public SocketIOConnectedEventRequest(WebPubSubConnectionContext connectionContext, string @namespace, string socketId) : base(connectionContext, @namespace, socketId)
        {
        }
    }
}
