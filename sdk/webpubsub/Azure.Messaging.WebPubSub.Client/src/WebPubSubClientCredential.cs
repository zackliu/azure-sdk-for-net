using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Messaging.WebPubSub.Client
{
    public class WebPubSubClientCredential
    {
        private readonly WebPubSubClientCredentialOptions _options;

        public WebPubSubClientCredential(Uri clientAccessUri): this(new WebPubSubClientCredentialOptions(_ => Task.FromResult(clientAccessUri)))
        {
        }

        public WebPubSubClientCredential(WebPubSubClientCredentialOptions options)
        {
            _options = options;
        }

        public Task<Uri> GetClientAccessUri(CancellationToken token)
        {
            _options.ClientAccessUriProvider.Invoke(token);
        }
    }
}
