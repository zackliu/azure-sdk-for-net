using Microsoft.Azure.WebPubSub.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Trigger.Model
{
    internal class SocketIOEventRequest
    {
        internal const string ConnectionContextProperty = "connectionContext";

        /// <summary>
        /// Connection context contains connection metadata following CloudEvents.
        /// </summary>
        [DataMember(Name = ConnectionContextProperty)]
        [JsonPropertyName(ConnectionContextProperty)]
        public WebPubSubConnectionContext ConnectionContext { get; }
    }

    public class SocketIOConnectEventRequest : SocketIOEventRequest
    {
        internal const string ClaimsProperty = "claims";
        internal const string QueryProperty = "query";
        internal const string HeadersProperty = "headers";
        internal const string ClientCertificatesProperty = "clientCertificates";

        /// <summary>
        /// User Claims.
        /// </summary>
        [JsonPropertyName(ClaimsProperty)]
        [DataMember(Name = ClaimsProperty)]
        public IReadOnlyDictionary<string, string[]> Claims { get; }

        /// <summary>
        /// Request query.
        /// </summary>
        [JsonPropertyName(QueryProperty)]
        [DataMember(Name = QueryProperty)]
        public IReadOnlyDictionary<string, string[]> Query { get; }

        /// <summary>
        /// Request headers.
        /// </summary>
        [JsonPropertyName(HeadersProperty)]
        [DataMember(Name = HeadersProperty)]
        public IReadOnlyDictionary<string, string[]> Headers { get; }

        /// <summary>
        /// Client certificates.
        /// </summary>
        [JsonPropertyName(ClientCertificatesProperty)]
        [DataMember(Name = ClientCertificatesProperty)]
        public IReadOnlyList<WebPubSubClientCertificate> ClientCertificates { get; }
    }
}
