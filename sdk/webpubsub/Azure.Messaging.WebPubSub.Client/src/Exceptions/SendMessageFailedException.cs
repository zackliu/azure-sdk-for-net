using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    public class SendMessageFailedException : Exception
    {
        public AckMessage AckMessage { get; }

        public SendMessageFailedException(string message, AckMessage ackMessage) : base(message)
        {
            AckMessage = ackMessage;
        }

        public SendMessageFailedException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
