// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Sdk;

namespace Azure.Messaging.WebPubSub.Clients.Tests.Protocols
{
    public class JsonProtocolTests
    {
        private class JsonData
        {
            public string Value { get; set; }
        }

        public static IEnumerable<object[]> GetTestData()
        {
            static object[] GetData(object jsonPayload, Action<WebPubSubMessage> assert)
            {
                var converter = JsonSerializer.Serialize(jsonPayload);
                return new object[] { Encoding.UTF8.GetBytes(converter), assert };
            }

            yield return GetData(new { type="ack", ackId = 123, success=true }, message =>
            {
                Assert.True(message is AckMessage);
                var ackMessage = message as AckMessage;
                Assert.Equal(123u, ackMessage.AckId);
                Assert.True(ackMessage.Success);
                Assert.Null(ackMessage.Error);
            });
            yield return GetData(new { type = "ack", ackId = 123, success = false, error = new { name = "Forbidden", message = "message"} }, message =>
            {
                Assert.True(message is AckMessage);
                var ackMessage = message as AckMessage;
                Assert.Equal(123u, ackMessage.AckId);
                Assert.False(ackMessage.Success);
                Assert.Equal("Forbidden", ackMessage.Error.Name);
                Assert.Equal("message", ackMessage.Error.Message);
            });
            yield return GetData(new { sequenceId = 738476327894u, type = "message", from = "group", group = "groupname", dataType = "text", data = "xyz", fromUserId = "user" }, message =>
            {
                Assert.True(message is GroupDataMessage);
                var groupDataMessage = message as GroupDataMessage;
                Assert.Equal("groupname", groupDataMessage.Group);
                Assert.Equal(738476327894u, groupDataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Text, groupDataMessage.DataType);
                Assert.Equal("user", groupDataMessage.FromUserId);
                Assert.Equal("xyz", groupDataMessage.Data.ToString());
            });
            yield return GetData(new { type = "message", from = "group", group = "groupname", dataType = "json", data = new JsonData { Value = "xyz" } }, message =>
            {
                Assert.True(message is GroupDataMessage);
                var groupDataMessage = message as GroupDataMessage;
                Assert.Equal("groupname", groupDataMessage.Group);
                Assert.Null(groupDataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Json, groupDataMessage.DataType);
                var obj = groupDataMessage.Data.ToObjectFromJson<JsonData>();
                Assert.Equal("xyz", obj.Value);
            });
            yield return GetData(new { type = "message", from = "group", group = "groupname", dataType = "binary", data = "eHl6" }, message =>
            {
                Assert.True(message is GroupDataMessage);
                var groupDataMessage = message as GroupDataMessage;
                Assert.Equal("groupname", groupDataMessage.Group);
                Assert.Null(groupDataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Binary, groupDataMessage.DataType);
                Assert.Equal("eHl6", Convert.ToBase64String(groupDataMessage.Data.ToArray()));
            });
            yield return GetData(new { sequenceId = 738476327894u, type = "message", from = "server", dataType = "text", data = "xyz" }, message =>
            {
                Assert.True(message is ServerDataMessage);
                var dataMessage = message as ServerDataMessage;
                Assert.Equal(738476327894u, dataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Text, dataMessage.DataType);
                Assert.Equal("xyz", dataMessage.Data.ToString());
            });
            yield return GetData(new { type = "message", from = "server", dataType = "json", data = new JsonData { Value = "xyz" } }, message =>
            {
                Assert.True(message is ServerDataMessage);
                var dataMessage = message as ServerDataMessage;;
                Assert.Null(dataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Json, dataMessage.DataType);
                var obj = dataMessage.Data.ToObjectFromJson<JsonData>();
                Assert.Equal("xyz", obj.Value);
            });
            yield return GetData(new { type = "message", from = "server", dataType = "binary", data = "eHl6" }, message =>
            {
                Assert.True(message is ServerDataMessage);
                var dataMessage = message as ServerDataMessage;
                Assert.Null(dataMessage.SequenceId);
                Assert.Equal(WebPubSubDataType.Binary, dataMessage.DataType);
                Assert.Equal("eHl6", Convert.ToBase64String(dataMessage.Data.ToArray()));
            });
            yield return GetData(new { type = "system", @event = "connected", userId = "user", connectionId = "connection" }, message =>
            {
                Assert.True(message is ConnectedMessage);
                var connectedMessage = message as ConnectedMessage;
                Assert.Equal("user", connectedMessage.UserId);
                Assert.Equal("connection", connectedMessage.ConnectionId);
                Assert.Null(connectedMessage.ReconnectionToken);
            });
            yield return GetData(new { type = "system", @event = "connected", userId = "user", connectionId = "connection", reconnectionToken = "rec" }, message =>
            {
                Assert.True(message is ConnectedMessage);
                var connectedMessage = message as ConnectedMessage;
                Assert.Equal("user", connectedMessage.UserId);
                Assert.Equal("connection", connectedMessage.ConnectionId);
                Assert.Equal("rec", connectedMessage.ReconnectionToken);
            });
            yield return GetData(new { type = "system", @event = "disconnected", message = "msg" }, message =>
            {
                Assert.True(message is DisconnectedMessage);
                var disconnectedMessage = message as DisconnectedMessage;
                Assert.Equal("msg", disconnectedMessage.Reason);
            });
        }

        [MemberData(nameof(GetTestData))]
        [Theory]
        public void ParseMessageTest(byte[] payload, Action<WebPubSubMessage> messageAssert)
        {
            var protocol = new WebPubSubJsonProtocol();
            var resolvedMessage = protocol.ParseMessage(new ReadOnlySequence<byte>(payload));
            messageAssert(resolvedMessage);
        }
    }
}
