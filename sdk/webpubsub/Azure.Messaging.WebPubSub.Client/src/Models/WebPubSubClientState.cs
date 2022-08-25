// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    internal enum WebPubSubClientState
    {
        Disconnected,
        Connecting,
        Connected,
        Suspended,
    }
}
