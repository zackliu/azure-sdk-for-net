using System;
using System.Buffers;
using System.Collections;
using System.Net.WebSockets;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.SignalR.WebSockets;

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
            switch (message)
            {
                case JoinGroupMessage joinGroup:
                    var joinGroupMessage = new UpstreamMessage.Types.JoinGroupMessage { Group = joinGroup.Group };
                    if (joinGroup.AckId.HasValue)
                    {
                        joinGroupMessage.AckId = joinGroup.AckId.Value;
                    }
                    new UpstreamMessage { JoinGroupMessage = joinGroupMessage }.WriteTo(output);
                    break;
                case LeaveGroupMessage leaveGroup:
                    var leaveGroupMessage = new UpstreamMessage.Types.LeaveGroupMessage { Group = leaveGroup.Group };
                    if (leaveGroup.AckId.HasValue)
                    {
                        leaveGroupMessage.AckId = leaveGroup.AckId.Value;
                    }
                    new UpstreamMessage { LeaveGroupMessage = leaveGroupMessage}.WriteTo(output);
                    break;
                case SequenceAckMessage sequenceAck:
                    new UpstreamMessage { SequenceAckMessage = new UpstreamMessage.Types.SequenceAckMessage { SequenceId = sequenceAck.SequenceId } }.WriteTo(output);
                    break;
                case SendToGroupMessage sendToGroup:
                    var sendToGroupMessage = new UpstreamMessage.Types.SendToGroupMessage
                    {
                        Group = sendToGroup.Group,
                        NoEcho = sendToGroup.NoEcho,
                        Data = WriteMessageData(sendToGroup.DataType, sendToGroup.Data)
                    };
                    if (sendToGroup.AckId.HasValue)
                    {
                        sendToGroupMessage.AckId = sendToGroup.AckId.Value;
                    }
                    new UpstreamMessage { SendToGroupMessage = sendToGroupMessage }.WriteTo(output);
                    break;
                case SendEventMessage sendEvent:
                    var sendEventMessage = new UpstreamMessage.Types.EventMessage
                    {
                        Event = sendEvent.EventName,
                        Data = WriteMessageData(sendEvent.DataType, sendEvent.Data),
                    };
                    if (sendEvent.AckId.HasValue)
                    {
                        sendEventMessage.AckId = sendEvent.AckId.Value;
                    }
                    new UpstreamMessage { EventMessage = sendEventMessage }.WriteTo(output);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private static MessageData WriteMessageData(WebPubSubDataType dataType, BinaryData data)
        {
            switch (dataType)
            {
                case WebPubSubDataType.Binary:
                    return new MessageData
                    {
                        BinaryData = UnsafeByteOperations.UnsafeWrap(data.ToMemory())
                    };
                case WebPubSubDataType.Json:
                case WebPubSubDataType.Text:
                    return new MessageData
                    {
                        TextData = data.ToString()
                    };
                case WebPubSubDataType.Protobuf:
                    var any = Any.Parser.ParseFrom(data.ToArray());
                    return new MessageData
                    {
                        ProtobufData = any,
                    };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
