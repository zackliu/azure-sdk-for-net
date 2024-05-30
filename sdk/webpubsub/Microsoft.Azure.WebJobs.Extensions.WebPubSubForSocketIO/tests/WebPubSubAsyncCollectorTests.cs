// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Messaging.WebPubSub;
using Moq;
using NUnit.Framework;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO.Tests
{
    public class WebPubSubAsyncCollectorTests
    {
        private static readonly Mock<WebPubSubServiceClient> _service = new();
        private static readonly SocketLifetimeStore _socketLifetimeStore = new();
        private static readonly WebPubSubForSocketIOAsyncCollector _collector = new(new WebPubSubService(_service.Object), _socketLifetimeStore);

        [Test]
        public void NullServiceThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new WebPubSubForSocketIOAsyncCollector(null, null));
        }

        [Test]
        public void NullServiceThrows2()
        {
            Assert.Throws<ArgumentNullException>(() => new WebPubSubForSocketIOAsyncCollector(new WebPubSubService(_service.Object), null));
        }

        [Test]
        public void NullWebPubSubActionThrows()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _collector.AddAsync(null));
        }

        [Test]
        public async Task SendToRoomTest()
        {
            await _collector.AddAsync(WebPubSubForSocketIOAction.CreateSendToRoomsAction(BinaryData.FromString("abc"), "/ns", new[] { "rm" }));
            _service.Verify(x => x.SendToGroupAsync("0~L25z~cm0=", It.Is<RequestContent>(c => AssertContentData(c, "42/ns,[\"abc\"]")), ContentType.TextPlain, null, It.Is<RequestContext>(x => x.CancellationToken == default)), Times.Once);
        }

        [Test]
        public async Task SendToRoomCtsTest()
        {
            using var cts = new CancellationTokenSource(1000);
            await _collector.AddAsync(WebPubSubForSocketIOAction.CreateSendToRoomsAction(BinaryData.FromString("abc"), "/ns", new[] { "rm" }));
            _service.Verify(x => x.SendToGroupAsync("0~L25z~cm0=", It.Is<RequestContent>(c => AssertContentData(c, "42/ns,[\"abc\"]")), ContentType.TextPlain, null, It.Is<RequestContext>(x => x.CancellationToken == cts.Token)), Times.Once);
        }

        private bool AssertContentData(RequestContent content, string expected)
        {
            using var ms = new MemoryStream();
            content.WriteTo(ms, default);
            return expected == Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
