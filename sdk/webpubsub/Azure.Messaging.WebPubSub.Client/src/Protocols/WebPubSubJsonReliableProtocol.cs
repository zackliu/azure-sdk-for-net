// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    internal class WebPubSubJsonReliableProtocol : WebPubSubJsonProtocolBase
    {
        public override string Name => "json.reliable.webpubsub.azure.v1";

        public override bool IsReliableSubProtocol => true;
    }
}
