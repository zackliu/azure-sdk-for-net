using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    internal class WebPubSubJsonProtocol : WebPubSubJsonProtocolBase
    {
        public override string Name => "json.webpubsub.azure.v1";

        public override bool IsReliableSubProtocol => false;
    }
}
