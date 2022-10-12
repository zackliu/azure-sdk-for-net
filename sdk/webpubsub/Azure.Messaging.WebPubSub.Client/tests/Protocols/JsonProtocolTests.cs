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
            yield return GetData(new { sequenceId = 738476327894u, type = "message", from = "group", group = "groupname", dataType = "text", data = "xyz" }, message =>
            {
                Assert.True(message is GroupDataMessage);
                var groupDataMessage = message as GroupDataMessage;
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
