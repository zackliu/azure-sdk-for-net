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
            var input = @"{ actionName : ""sendToNamespace"", data: [""data1"", ""data2""]}";
            var converted = (SendToNamespaceAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("data1", converted.Data[0]);
            Assert.AreEqual("data2", converted.Data[1]);
        }

        [TestCase]
        public void TestSendToNamespace_ValidComplexData()
        {
            var input = @"{ actionName : ""sendToNamespace"", data: [1, ""2"", {""a"":true}]}";
            var converted = (SendToNamespaceAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual(1, converted.Data[0]);
            Assert.AreEqual("2", converted.Data[1]);
            Assert.AreEqual(true, (bool)((JObject)converted.Data[2])["a"]);
        }

        [TestCase]
        public void TestSendToNamespace_InvalidData()
        {
            var input = @"{ actionName : ""sendToNamespace"", dataType: ""text"", data: 2}";
            var jObject = JObject.Parse(input);

            Assert.Throws<ArgumentException>(() => WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(jObject));
        }

        [TestCase]
        public void TestSendToRooms_Valid()
        {
            var input = @"{ actionName : ""sendToRooms"", rooms: [""rma"", ""rmb""], data: [1, ""2"", {""a"":true}]}";
            var converted = (SendToRoomsAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("rma", converted.Rooms[0]);
            Assert.AreEqual("rmb", converted.Rooms[1]);
            Assert.AreEqual(1, converted.Data[0]);
            Assert.AreEqual("2", converted.Data[1]);
            Assert.AreEqual(true, (bool)((JObject)converted.Data[2])["a"]);
        }

        [TestCase]
        public void TestSendToRooms_InvalidRoom()
        {
            var input = @"{ actionName : ""sendToRooms"", rooms: ""abc"", data: [""data1"", ""data2""]}";
            var jObject = JObject.Parse(input);
            Assert.Throws<ArgumentException>(() => WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(jObject));
        }

        [TestCase]
        public void TestSendToSocket_Valid()
        {
            var input = @"{ actionName : ""sendToSocket"", socketId: ""sid"" ,data: [""data1"", ""data2""]}";
            var converted = (SendToSocketAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("sid", converted.SocketId);
            Assert.AreEqual("data1", converted.Data[0]);
            Assert.AreEqual("data2", converted.Data[1]);
        }

        [TestCase]
        public void AddSocketToRoom_Valid()
        {
            var input = @"{ actionName : ""addSocketToRoom"", socketId: ""sid"", room: ""rm1"", namespace: ""ns""}";
            var converted = (AddSocketToRoomAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("sid", converted.SocketId);
            Assert.AreEqual("rm1", converted.Room);
            Assert.AreEqual("ns", converted.Namespace);
        }

        [TestCase]
        public void RemoveSocketFromRoom_Valid()
        {
            var input = @"{ actionName : ""removeSocketFromRoom"", socketId: ""sid"", room: ""rm1"", namespace: ""ns""}";
            var converted = (RemoveSocketFromRoomAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("sid", converted.SocketId);
            Assert.AreEqual("rm1", converted.Room);
            Assert.AreEqual("ns", converted.Namespace);
        }

        [TestCase]
        public void DisconnectSocket_Valid()
        {
            var input = @"{ actionName : ""disconnectSockets"", rooms: [ ""rm1"", ""rm2""], namespace: ""ns""}";
            var converted = (DisconnectSocketsAction)WebPubSubForSocketIOConfigProvider.ConvertToWebPubSubOperation(JObject.Parse(input));

            Assert.AreEqual("ns", converted.Namespace);
            Assert.AreEqual("rm1", converted.Rooms[0]);
            Assert.AreEqual("rm2", converted.Rooms[1]);
        }
    }
}
