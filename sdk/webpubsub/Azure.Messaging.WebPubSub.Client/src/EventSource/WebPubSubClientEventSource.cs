// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using Azure.Core.Diagnostics;

namespace Azure.Messaging.WebPubSub.Client
{
    [EventSource(Name = EventSourceName)]
    internal class WebPubSubClientEventSource: AzureEventSource
    {
        private const string EventSourceName = "Azure-Messaging-WebPubSub-Client";

        private WebPubSubClientEventSource() : base(EventSourceName)
        {
        }

        // Having event ids defined as const makes it easy to keep track of them
        private const int ClientStartingId = 1;
        private const int ClientStateChangesId = 2;
        private const int ConnectionStartingId = 3;
        private const int ConnectionClosedId = 4;
        private const int FailedToHandleMessageId = 5;

        public static WebPubSubClientEventSource Log { get; } = new WebPubSubClientEventSource();

        [Event(1, Level = EventLevel.Verbose, Message = "Client starting.")]
        public virtual void ClientStarting()
        {
            if (IsEnabled())
            {
                WriteEvent(ClientStartingId);
            }
        }

        [Event(2, Level = EventLevel.Verbose, Message = "Client state changes to {0}.")]
        public virtual void ClientStateChanges(WebPubSubClientState state)
        {
            if (IsEnabled())
            {
                WriteEvent(ClientStateChangesId, state.ToString());
            }
        }

        [Event(3, Level = EventLevel.Informational, Message = "Connection start to connect with subprotocol {2}.")]
        public virtual void ConnectionStarting(string subprotocol)
        {
            if (IsEnabled())
            {
                WriteEvent(ConnectionStartingId, subprotocol);
            }
        }

        [Event(4, Level = EventLevel.Informational, Message = "Connection closed.")]
        public virtual void ConnectionClosed()
        {
            if (IsEnabled())
            {
                WriteEvent(ConnectionClosedId);
            }
        }

        [Event(5, Level = EventLevel.Warning, Message = "An exception occurred while handling message from the service.")]
        public virtual void FailedToHandleMessage(string errorMessage)
        {
            if (IsEnabled())
            {
                WriteEvent(FailedToHandleMessageId, errorMessage);
            }
        }
    }
}
