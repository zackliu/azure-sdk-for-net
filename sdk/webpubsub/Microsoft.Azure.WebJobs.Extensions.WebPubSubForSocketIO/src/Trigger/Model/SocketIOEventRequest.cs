using Microsoft.Azure.WebPubSub.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Trigger.Model
{
    /// <summary>
    /// Base class for Socket.IO events
    /// </summary>
    [DataContract]
    public abstract class SocketIOEventRequest
    {
        internal const string ConnectionContextProperty = "connectionContext";
        internal const string NamespaceProperty = "namespace";
        internal const string SocketIdProperty = "socketId";

        /// <summary>
        /// Connection context contains connection metadata following CloudEvents.
        /// </summary>
        [DataMember(Name = ConnectionContextProperty)]
        [JsonPropertyName(ConnectionContextProperty)]
        public WebPubSubConnectionContext ConnectionContext { get; }

        /// <summary>
        /// The namespace of socket
        /// </summary>
        [DataMember(Name = NamespaceProperty)]
        [JsonPropertyName(NamespaceProperty)]
        public string Namespace { get; }

        /// <summary>
        /// The socket-id of socket
        /// </summary>
        [DataMember(Name = SocketIdProperty)]
        [JsonPropertyName(SocketIdProperty)]
        public string SocketId { get; }

        /// <summary>
        /// Ctor of SocketIOEventRequest
        /// </summary>
        protected SocketIOEventRequest(WebPubSubConnectionContext connectionContext, string @namespace, string socketId)
        {
            ConnectionContext = connectionContext;
            Namespace = @namespace;
            SocketId = socketId;
        }
    }
}
