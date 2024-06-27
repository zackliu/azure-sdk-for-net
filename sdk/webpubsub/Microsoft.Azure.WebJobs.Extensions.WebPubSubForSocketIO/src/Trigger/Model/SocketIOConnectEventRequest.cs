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

        /// <summary>
        /// Ctor of SocketIOConnectEventRequest
        /// </summary>
        public SocketIOConnectEventRequest(
            WebPubSubConnectionContext context,
            string @namespace,
            string socketId,
            IReadOnlyDictionary<string, string[]> claims,
            IReadOnlyDictionary<string, string[]> query,
            IEnumerable<WebPubSubClientCertificate> certificates,
            IReadOnlyDictionary<string, string[]> headers) : base(context, @namespace, socketId)
        {
            if (claims != null)
            {
                Claims = claims;
            }
            if (query != null)
            {
                Query = query;
            }
            if (headers != null)
            {
                Headers = headers;
            }
            ClientCertificates = certificates?.ToArray();
        }
    }
}
