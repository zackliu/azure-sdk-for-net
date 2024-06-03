// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebPubSub.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

using SystemJson = System.Text.Json;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Tests
{
    public class JObjectTests
    {
        public JObjectTests()
        {
            WebPubSubForSocketIOConfigProvider.RegisterJsonConverter();
        }

        [TestCase(nameof(SendToNamespaceAction))]
        [TestCase(nameof(SendToRoomsAction))]
        [TestCase(nameof(SendToSocketAction))]
        [TestCase(nameof(AddSocketToRoomAction))]
        [TestCase(nameof(RemoveSocketFromRoomAction))]
        [TestCase(nameof(DisconnectSocketsAction))]
        public void TestOutputConvert(string actionName)
        {
            var input = @"{ ""actionName"":""{0}"",""namespace"":""ns"", ""data"":[""test""]}";

            var replacedInput = input.Replace("{0}", actionName.Substring(0, actionName.IndexOf("Action")));

            var jObject = JObject.Parse(replacedInput);

            var converted = WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(jObject);

            Assert.AreEqual(actionName, converted.ActionName.ToString());
        }

        [TestCase("webpubsuboperation")]
        [TestCase("unknown")]
        public void TestInvalidWebPubSubOperationConvert(string actionName)
        {
            var input = @"{ ""actionName"":""{0}"",""userId"":""user"", ""group"":""group1"",""connectionId"":""connection"",""data"":""test"",""dataType"":""text"", ""reason"":""close"", ""excluded"":[""aa"",""bb""]}";

            var replacedInput = input.Replace("{0}", actionName);

            var jObject = JObject.Parse(replacedInput);

            // Throws excpetion of not able to de-serialize to abstract class.
            var ex = Assert.Throws<ArgumentException>(() => WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(jObject));
            Assert.AreEqual($"Not supported WebPubSubOperation: {actionName}.", ex.Message);
        }

        [TestCase]
        public void TestSendToNamespace_Valid()
        {
            var input = @"{ actionName : ""sendToNamespaceAction"", data: [""data1"", ""data2""]}";

            var converted = JObject.Parse(input).ToObject<SendToNamespaceAction>();

            Assert.AreEqual("data1", converted.Data[0]);
            Assert.AreEqual("data2", converted.Data[1]);
        }

        [TestCase]
        public void TestSendToNamespace_ValidNumberData()
        {
            var input = @"{ actionName : ""sendToNamespaceAction"", data: [1, 2]}";
            //var p = JObject.FromObject(1);
            var b = BinaryData.FromObjectAsJson(new {a = 1, v = "1" });
            var a = BinaryData.FromObjectAsJson(1);
            var a1 = BinaryData.FromObjectAsJson("1");

            var e = b.ToString();

            var converted = JObject.Parse(input).ToObject<SendToNamespaceAction>();

            Assert.AreEqual(1, converted.Data[0]);
            Assert.AreEqual(2, converted.Data[1]);
        }

        [TestCase]
        public void TestSendToNamespace_InvalidData()
        {
            var input = @"{ actionName : ""sendToAll"", dataType: ""text"", data: 2}";
            var jObject = JObject.Parse(input);

            Assert.Throws<ArgumentException>(() => WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(jObject), "Message data should be string, please stringify object.");
        }

        [TestCase]
        public void TestSendToRooms_Valid()
        {
            var input = @"{ actionName : ""sendToRoomsAction"", rooms: [""rma"", ""rmb""], data: [""data1"", ""data2""]}";

            var converted = JObject.Parse(input).ToObject<SendToRoomsAction>();

            Assert.AreEqual("rma", converted.Rooms[0]);
            Assert.AreEqual("rmb", converted.Rooms[1]);
            Assert.AreEqual("data1", converted.Data[0]);
            Assert.AreEqual("data2", converted.Data[1]);
        }

        [TestCase]
        public void TestSendToRooms_InvalidRoom()
        {
            var input = @"{ actionName : ""sendToRoomsAction"", rooms: ""abc"", data: [""data1"", ""data2""]}";
            Assert.Throws<ArgumentException>(() => JObject.Parse(input).ToObject<SendToRoomsAction>(), "Message data should be string, please stringify object.");
        }

        [TestCase]
        public void TestSendToSocket_Valid()
        {
            var input = @"{ actionName : ""sendToSocketAction"", socketId: ""sid"" ,data: [""data1"", ""data2""]}";

            var converted = JObject.Parse(input).ToObject<SendToSocketAction>();

            Assert.AreEqual("sid", converted.SocketId);
            Assert.AreEqual("data1", converted.Data[0]);
            Assert.AreEqual("data2", converted.Data[1]);
        }
    }
}
