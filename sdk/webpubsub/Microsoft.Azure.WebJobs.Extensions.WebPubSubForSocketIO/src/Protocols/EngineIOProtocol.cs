// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    internal class EngineIOProtocol
    {
        public static ReadOnlyMemory<byte> EncodePacket(SocketIOPacket packet)
        {
            return Encoding.UTF8.GetBytes($"4{SocketIOProtocol.EncodePacket(packet)}");
        }
    }
}
