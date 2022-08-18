// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    internal enum WebPubSubClientStatus
    {
        Disconnected,
        Connecting,
        Connected,
        Suspended,
    }
}
