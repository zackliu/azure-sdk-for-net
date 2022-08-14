using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    internal abstract class WebPubSubJsonProtocolBase : IWebPubSubProtocol
    {
        private const string TypePropertyName = "type";
        private static readonly JsonEncodedText TypePropertyNameBytes = JsonEncodedText.Encode(TypePropertyName);
        private const string GroupPropertyName = "group";
        private static readonly JsonEncodedText GroupPropertyNameBytes = JsonEncodedText.Encode(GroupPropertyName);
        private const string AckIdPropertyName = "ackId";
        private static readonly JsonEncodedText AckIdPropertyNameBytes = JsonEncodedText.Encode(AckIdPropertyName);
        private const string DataTypePropertyName = "dataType";
        private static readonly JsonEncodedText DataTypePropertyNameBytes = JsonEncodedText.Encode(DataTypePropertyName);
        private const string DataPropertyName = "data";
        private static readonly JsonEncodedText DataPropertyNameBytes = JsonEncodedText.Encode(DataPropertyName);
        private const string EventPropertyName = "event";
        private static readonly JsonEncodedText EventPropertyNameBytes = JsonEncodedText.Encode(EventPropertyName);
        private const string NoEchoPropertyName = "noEcho";
        private static readonly JsonEncodedText NoEchoPropertyNameBytes = JsonEncodedText.Encode(NoEchoPropertyName);

        private const string SuccessPropertyName = "success";
        private static readonly JsonEncodedText SuccessPropertyNameBytes = JsonEncodedText.Encode(SuccessPropertyName);
        private const string MessagePropertyName = "message";
        private static readonly JsonEncodedText MessagePropertyNameBytes = JsonEncodedText.Encode(MessagePropertyName);
        private const string ErrorPropertyName = "error";
        private static readonly JsonEncodedText ErrorPropertyNameBytes = JsonEncodedText.Encode(ErrorPropertyName);
        private const string ErrorNamePropertyName = "name";
        private static readonly JsonEncodedText ErrorNamePropertyNameBytes = JsonEncodedText.Encode(ErrorNamePropertyName);
        private const string FromPropertyName = "from";
        private static readonly JsonEncodedText FromPropertyNameBytes = JsonEncodedText.Encode(FromPropertyName);
        private const string FromUserIdPropertyName = "fromUserId";
        private static readonly JsonEncodedText FromUserIdPropertyNameBytes = JsonEncodedText.Encode(FromUserIdPropertyName);
        private const string UserIdPropertyName = "userId";
        private static readonly JsonEncodedText UserIdPropertyNameBytes = JsonEncodedText.Encode(UserIdPropertyName);
        private const string ConnectionIdPropertyName = "connectionId";
        private static readonly JsonEncodedText ConnectionIdPropertyNameBytes = JsonEncodedText.Encode(ConnectionIdPropertyName);
        private const string ReconnectionTokenPropertyName = "reconnectionToken";
        private static readonly JsonEncodedText ReconnectionTokenPropertyNameBytes = JsonEncodedText.Encode(ReconnectionTokenPropertyName);
        private const string SequenceIdPropertyName = "sequenceId";
        private static readonly JsonEncodedText SequenceIdPropertyNameBytes = JsonEncodedText.Encode(SequenceIdPropertyName);

        private static readonly JsonEncodedText JoinGroupTypeBytes = JsonEncodedText.Encode("joinGroup");
        private static readonly JsonEncodedText LeaveGroupTypeBytes = JsonEncodedText.Encode("leaveGroup");
        private static readonly JsonEncodedText SendToGroupTypeBytes = JsonEncodedText.Encode("sendToGroup");
        private static readonly JsonEncodedText SendEventTypeBytes = JsonEncodedText.Encode("event");
        private static readonly JsonEncodedText SequenceAckTypeBytes = JsonEncodedText.Encode("sequenceAck");

        public abstract string Name { get; }

        public abstract bool IsReliableSubProtocol { get; }

        public ReadOnlyMemory<byte> GetMessageBytes(WebPubSubMessage message)
        {
            throw new NotImplementedException();
        }

        public bool TryParseMessage(ref ReadOnlySequence<byte> input, out WebPubSubMessage webPubSubMessage)
        {
            try
            {
                string type = null;
                string groupName = null;
                string eventName = null;
                SystemEventType systemEventType = SystemEventType.Connected;
                ulong? ackId = null;
                ulong? sequenceId = null;
                bool? success = null;
                string from = null;
                FromType fromType = FromType.Server;
                ErrorDetail errorDetail = null;
                DownstreamEventType eventType = DownstreamEventType.Ack;
                DataType dataType = null;
                string userId = null;
                string connectionId = null;
                string reconnectionToken = null;
                string message = null;
                string fromUserId = null;

                var completed = false;
                bool hasDataToken = false;
                BinaryData data = null;
                SequencePosition dataStart = default;
                Utf8JsonReader dataReader = default;

                var reader = new Utf8JsonReader(input, isFinalBlock: true, state: default);

                reader.CheckRead();

                // We're always parsing a JSON object
                reader.EnsureObjectStart();

                do
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.PropertyName:
                            if (reader.ValueTextEquals(TypePropertyNameBytes.EncodedUtf8Bytes))
                            {
                                type = reader.ReadAsString(TypePropertyName);
                                if (type == null)
                                {
                                    throw new InvalidDataException($"Expected '{TypePropertyName}' to be of type {JsonTokenType.String}.");
                                }
                                if (!Enum.TryParse(type, true, out eventType))
                                {
                                    throw new InvalidDataException($"Unknown '{TypePropertyName}': {type}.");
                                }
                            }
                            else if (reader.ValueTextEquals(GroupPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                groupName = reader.ReadAsString(GroupPropertyName);
                            }
                            else if (reader.ValueTextEquals(EventPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                eventName = reader.ReadAsString(EventPropertyName);
                                if (!Enum.TryParse(eventName, true, out systemEventType))
                                {
                                    throw new InvalidDataException($"Unknown '{EventPropertyName}': {eventName}.");
                                }
                            }
                            else if (reader.ValueTextEquals(DataTypePropertyNameBytes.EncodedUtf8Bytes))
                            {
                                var dataTypeValue = reader.ReadAsString(DataTypePropertyName);
                                if (!Enum.TryParse<MessageDataType>(dataTypeValue, true, out var messageDataType))
                                {
                                    throw new InvalidDataException($"Unknown '{DataTypePropertyName}': {dataTypeValue}.");
                                }
                                switch (messageDataType)
                                {
                                    case MessageDataType.Text:
                                        dataType = DataType.Text;
                                        break;
                                    case MessageDataType.Json:
                                        dataType = DataType.Json;
                                        break;
                                    case MessageDataType.Binary:
                                        dataType = DataType.Binary;
                                        break;
                                    case MessageDataType.Protobuf:
                                        dataType = DataType.Protobuf;
                                        break;
                                    default:
                                        throw new InvalidDataException($"Unknown '{DataTypePropertyName}': {dataTypeValue}.");
                                }
                            }
                            else if (reader.ValueTextEquals(AckIdPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                try
                                {
                                    ackId = reader.ReadAsUlong(AckIdPropertyName);
                                }
                                catch (FormatException)
                                {
                                    throw new InvalidDataException($"'{AckIdPropertyName}' is not a valid uint64 value.");
                                }
                            }
                            else if (reader.ValueTextEquals(DataPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                hasDataToken = true;
                                dataStart = reader.Position;
                                reader.Skip();
                                dataReader = reader;
                            }
                            else if (reader.ValueTextEquals(SequenceIdPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                try
                                {
                                    sequenceId = reader.ReadAsUlong(SequenceIdPropertyName);
                                }
                                catch (FormatException)
                                {
                                    throw new InvalidDataException($"'{SequenceIdPropertyName}' is not a valid uint64 value.");
                                }
                            }
                            else if (reader.ValueTextEquals(SuccessPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                success = reader.ReadAsBoolean(SuccessPropertyName);
                            }
                            else if (reader.ValueTextEquals(ErrorPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                errorDetail = ReadErrorDetail(reader);
                            }
                            else if (reader.ValueTextEquals(FromPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                from = reader.ReadAsString(FromPropertyName);
                                if (!Enum.TryParse(from, true, out fromType))
                                {
                                    throw new InvalidDataException($"Unknown '{FromPropertyName}': {from}.");
                                }
                            }
                            else if (reader.ValueTextEquals(UserIdPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                userId = reader.ReadAsString(UserIdPropertyName);
                            }
                            else if (reader.ValueTextEquals(ConnectionIdPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                connectionId = reader.ReadAsString(ConnectionIdPropertyName);
                            }
                            else if (reader.ValueTextEquals(ReconnectionTokenPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                reconnectionToken = reader.ReadAsString(ReconnectionTokenPropertyName);
                            }
                            else if (reader.ValueTextEquals(MessagePropertyNameBytes.EncodedUtf8Bytes))
                            {
                                message = reader.ReadAsString(MessagePropertyName);
                            }
                            else if (reader.ValueTextEquals(FromUserIdPropertyNameBytes.EncodedUtf8Bytes))
                            {
                                fromUserId = reader.ReadAsString(FromUserIdPropertyName);
                            }
                            else
                            {
                                reader.CheckRead();
                                reader.Skip();
                            }
                            break;
                        case JsonTokenType.EndObject:
                            completed = true;
                            break;
                    }
                }
                while (!completed && reader.CheckRead());

                if (type == null)
                {
                    throw new InvalidDataException($"Missing required propety '{TypePropertyName}'.");
                }

                if (hasDataToken)
                {
                    if (dataType == DataType.Binary ||
                        dataType == DataType.Protobuf ||
                        dataType == DataType.Text)
                    {
                        if (dataReader.TokenType != JsonTokenType.String)
                        {
                            throw new InvalidDataException($"'data' should be a string when 'dataType' is 'binary,text,protobuf'.");
                        }

                        if (dataType == DataType.Binary ||
                            dataType == DataType.Protobuf)
                        {
                            if (!dataReader.TryGetBytesFromBase64(out var bytes))
                            {
                                throw new InvalidDataException($"'data' is not a valid base64 encoded string.");
                            }
                            data = new BinaryData(bytes);
                        }
                        else
                        {
                            data = new BinaryData(dataReader.ReadAsString(DataTypePropertyName));
                        }
                    }
                    else if (dataType == DataType.Json)
                    {
                        if (dataReader.TokenType == JsonTokenType.Null)
                        {
                            throw new InvalidDataException($"Invalid value for '{DataPropertyName}': null.");
                        }

                        var end = dataReader.Position;
                        data = new BinaryData(input.Slice(dataStart, end).ToArray());
                    }
                }

                switch (eventType)
                {
                    case DownstreamEventType.Ack:
                        AssertNotNull(ackId, AckIdPropertyName);
                        AssertNotNull(success, SuccessPropertyName);
                        webPubSubMessage = new AckMessage(ackId.Value, success.Value, errorDetail);
                        return true;

                    case DownstreamEventType.Message:
                        AssertNotNull(from, FromPropertyName);
                        AssertNotNull(dataType, FromPropertyName);
                        AssertNotNull(data, DataPropertyName);
                        switch (fromType)
                        {
                            case FromType.Server:
                                webPubSubMessage = new ServerResponseMessage(dataType, data, sequenceId);
                                return true;
                            case FromType.Group:
                                AssertNotNull(groupName, GroupPropertyName);
                                webPubSubMessage = new GroupResponseMessage(groupName, dataType, data, sequenceId, fromUserId);
                                return true;
                        }
                        break;

                    case DownstreamEventType.System:
                        AssertNotNull(eventName, EventPropertyName);

                        switch (systemEventType)
                        {
                            case SystemEventType.Connected:
                                webPubSubMessage = new ConnectedMessage(userId, connectionId, reconnectionToken);
                                return true;
                            case SystemEventType.Disconnected:
                                webPubSubMessage = new DisconnectedMessage(message);
                                return true;
                        }
                        break;

                    default:
                        throw new InvalidDataException($"Unsupported type {eventType}");
                }
            }
            catch (JsonException jrex)
            {
                throw new InvalidDataException("Error reading JSON.", jrex);
            }
        }

        public void WriteMessage(WebPubSubMessage message, IBufferWriter<byte> output)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var jsonWriterLease = ReusableUtf8JsonWriter.Get(output);

            try
            {
                var writer = jsonWriterLease.GetJsonWriter();

                writer.WriteStartObject();

                switch (message)
                {
                    case JoinGroupMessage joinGroupMessage:
                        writer.WriteString(TypePropertyNameBytes, JoinGroupTypeBytes);
                        writer.WriteString(GroupPropertyNameBytes, joinGroupMessage.Group);
                        if (joinGroupMessage.AckId != null)
                        {
                            writer.WriteNumber(AckIdPropertyNameBytes, joinGroupMessage.AckId.Value);
                        }
                        break;
                    case LeaveGroupMessage leaveGroupMessage:
                        writer.WriteString(TypePropertyNameBytes, LeaveGroupTypeBytes);
                        writer.WriteString(GroupPropertyNameBytes, leaveGroupMessage.Group);
                        if (leaveGroupMessage.AckId != null)
                        {
                            writer.WriteNumber(AckIdPropertyNameBytes, leaveGroupMessage.AckId.Value);
                        }
                        break;
                    case SendToGroupMessage sendToGroupMessage:
                        writer.WriteString(TypePropertyNameBytes, SendToGroupTypeBytes);
                        writer.WriteString(GroupPropertyNameBytes, sendToGroupMessage.Group);
                        if (sendToGroupMessage.AckId != null)
                        {
                            writer.WriteNumber(AckIdPropertyNameBytes, sendToGroupMessage.AckId.Value);
                        }
                        writer.WriteBoolean(NoEchoPropertyNameBytes, sendToGroupMessage.NoEcho);
                        writer.WriteString(DataTypePropertyNameBytes, sendToGroupMessage.DataType.Name);
                        writer.WriteBase64String(DataPropertyNameBytes, sendToGroupMessage.Data);
                        break;
                    case SendEventMessage sendEventMessage:
                        writer.WriteString(TypePropertyNameBytes, SendEventTypeBytes);
                        if (sendEventMessage.AckId != null)
                        {
                            writer.WriteNumber(AckIdPropertyNameBytes, sendEventMessage.AckId.Value);
                        }
                        writer.WriteString(DataTypePropertyNameBytes, sendEventMessage.DataType.Name);
                        writer.WriteBase64String(DataPropertyNameBytes, sendEventMessage.Data);
                        break;
                    case SequenceAckMessage sequenceAckMessage:
                        writer.WriteString(TypePropertyNameBytes, SequenceAckTypeBytes);
                        writer.WriteNumber(SequenceIdPropertyNameBytes, sequenceAckMessage.SequenceId);
                        break;
                    default:
                        throw new InvalidDataException($"{message.GetType()} is not supported.");
                }

                writer.WriteEndObject();
                writer.Flush();
            }
            finally
            {
                ReusableUtf8JsonWriter.Return(jsonWriterLease);
            }
        }

        private static ErrorDetail ReadErrorDetail(Utf8JsonReader reader)
        {
            string errorName = null;
            string errorMessage = null;

            var completed = false;
            reader.CheckRead();
            // Error detail should start with object
            reader.EnsureObjectStart();
            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        if (reader.ValueTextEquals(ErrorNamePropertyNameBytes.EncodedUtf8Bytes))
                        {
                            errorName = reader.ReadAsString(ErrorNamePropertyName);
                        }
                        else if (reader.ValueTextEquals(MessagePropertyNameBytes.EncodedUtf8Bytes))
                        {
                            errorMessage = reader.ReadAsString(MessagePropertyName);
                        }
                        break;
                    case JsonTokenType.EndObject:
                        completed = true;
                        break;
                }
            }
            while (!completed && reader.CheckRead());

            return new ErrorDetail(errorName, errorMessage);
        }

        private static void AssertNotNull<T>(T value, string propertyName)
        {
            if (value == null)
            {
                throw new InvalidDataException($"Missing required propety '{propertyName}'.");
            }
        }

        private enum DownstreamEventType
        {
            Ack,
            Message,
            System,
        }

        private enum FromType
        {
            Server,
            Group,
        }

        private enum SystemEventType
        {
            Connected,
            Disconnected,
        }

        private enum MessageDataType
        {
            Text,
            Binary,
            Json,
            Protobuf
        }
    }
}
