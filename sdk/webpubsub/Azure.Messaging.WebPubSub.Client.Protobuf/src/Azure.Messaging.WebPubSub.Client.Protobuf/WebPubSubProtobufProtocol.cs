using System;
using System.Buffers;
using System.Net.WebSockets;

namespace Azure.Messaging.WebPubSub.Client.Protobuf
{
    public class WebPubSubProtobufProtocol : IWebPubSubProtocol
    {
        public string Name => "protobuf.webpubsub.azure.v1";

        public WebSocketMessageType WebSocketMessageType => WebSocketMessageType.Binary;

        public bool IsReliableSubProtocol => false;

        public ReadOnlyMemory<byte> GetMessageBytes(WebPubSubMessage message)
        {
            throw new NotImplementedException();
        }

        public WebPubSubMessage ParseMessage(ReadOnlySequence<byte> input)
        {
            throw new NotImplementedException();
        }

        public void WriteMessage(WebPubSubMessage message, IBufferWriter<byte> output)
        {
            throw new NotImplementedException();
        }
    }
}
