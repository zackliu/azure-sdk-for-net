using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Clients
{
    internal interface IWebSocketClientFactory
    {
        IWebSocketClient CreateWebSocketClient(Uri uri, string protocol);
    }
}
