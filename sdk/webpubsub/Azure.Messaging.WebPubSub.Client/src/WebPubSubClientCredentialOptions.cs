using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Messaging.WebPubSub.Client
{

    public class WebPubSubClientCredentialOptions
    {
        public Func<CancellationToken, Task<Uri>> ClientAccessUriProvider { get; }

        public WebPubSubClientCredentialOptions(Func<CancellationToken, Task<Uri>> clientAccessUriProvider)
        {
            ClientAccessUriProvider = clientAccessUriProvider;
        }
    }
}
