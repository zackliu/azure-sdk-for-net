// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Messaging.WebPubSub;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    internal class WebPubSubForSocketIOService : IWebPubSubForSocketIOService
    {
        private readonly WebPubSubServiceClient _client;

        public WebPubSubForSocketIOService(string connectionString, string hub)
        {
            _client = new WebPubSubServiceClient(connectionString, hub);
        }

        // For tests.
        public WebPubSubForSocketIOService(WebPubSubServiceClient client)
        {
            _client = client;
        }

        public WebPubSubServiceClient Client => _client;

        internal SocketIONegotiateResult GetClientConnection()
        {
            var url = _client.GetClientAccessUri();

            return new SocketIONegotiateResult(url);
        }
    }
}
