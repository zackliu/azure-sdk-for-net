// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Buffers;

namespace Azure.Messaging.WebPubSub.Clients
{
    internal struct WebSocketReadResult
    {
        public bool IsClosed { get; }

        public ReadOnlySequence<byte> Payload { get; }

        public WebSocketReadResult(ReadOnlySequence<byte> payload, bool isClosed = false)
        {
            Payload = payload;
            IsClosed = isClosed;
        }
    }
}
