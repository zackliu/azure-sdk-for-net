// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Azure.Messaging.WebPubSub.Clients;
using Moq;
using Xunit;

namespace Azure.Messaging.WebPubSub.Client.Tests
{
    public class WebPubSubClientEventsHandlerTests
    {
        private readonly Mock<IWebSocketClient> _webSocketClientMoc;
        private readonly Mock<IWebSocketClientFactory> _factoryMoc;
        private readonly WebPubSubProtocol _protocol = new WebPubSubJsonReliableProtocol();

        public WebPubSubClientEventsHandlerTests()
        {
            _webSocketClientMoc = new Mock<IWebSocketClient>();
            _webSocketClientMoc.SetReturnsDefault(Task.CompletedTask);
            _webSocketClientMoc.Setup(c => c.ReceiveOneFrameAsync(It.IsAny<CancellationToken>())).Returns<CancellationToken>(async token =>
            {
                await Task.Delay(int.MaxValue).AwaitWithCancellation(token);
                return new WebSocketReadResult(new ReadOnlySequence<byte>(), false);
            });
            _factoryMoc = new Mock<IWebSocketClientFactory>();
            _factoryMoc.Setup(f => f.CreateWebSocketClient(It.IsAny<Uri>(), It.IsAny<string>())).Returns(_webSocketClientMoc.Object);
        }

        [Fact]
        public async Task WebPubSubClientEventHandlerTest()
        {
            var packageSeq = GetTestData().ToList();
            var idx = 0;
            _webSocketClientMoc.Setup(c => c.ReceiveOneFrameAsync(It.IsAny<CancellationToken>())).Returns<CancellationToken>(token =>
            {
                return Task.FromResult(packageSeq[idx++]);
            });
            _webSocketClientMoc.SetupGet(c => c.CloseStatus).Returns(WebSocketCloseStatus.PolicyViolation); // Force to not recover

            var connectedTcs = NewTcs<WebPubSubConnectedEventArgs>();
            var serverMessageTcs = NewTcs<WebPubSubServerMessageEventArgs>();
            var groupMessageTcs = NewTcs<WebPubSubGroupMessageEventArgs>();
            var disconnectedTcs = NewTcs<WebPubSubDisconnectedEventArgs>();
            var stoppedTcs = NewTcs<WebPubSubStoppedEventArgs>();

            var client = new WebPubSubClient(new Uri("wss://test.com"), new WebPubSubClientOptions { AutoReconnect = false});
            client.WebSocketClientFactory = _factoryMoc.Object;
            client.Connected += arg =>
            {
                connectedTcs.TrySetResult(arg);
                return Task.CompletedTask;
            };
            client.ServerMessageReceived += arg =>
            {
                serverMessageTcs.TrySetResult(arg);
                return Task.CompletedTask;
            };
            client.GroupMessageReceived += arg =>
            {
                groupMessageTcs.TrySetResult(arg);
                return Task.CompletedTask;
            };
            client.Disconnected += arg =>
            {
                disconnectedTcs.TrySetResult(arg);
                return Task.CompletedTask;
            };
            client.Stopped += arg =>
            {
                stoppedTcs.TrySetResult(arg);
                return Task.CompletedTask;
            };
            await client.StartAsync();

            var connArg = await connectedTcs.Task.OrTimeout();
            Assert.Equal("conn", connArg.ConnectionId);
            Assert.Equal("user", connArg.UserId);

            var serArg = await serverMessageTcs.Task.OrTimeout();
            Assert.NotNull(serArg.Message);

            var groupArg = await groupMessageTcs.Task.OrTimeout();
            Assert.NotNull(groupArg.Message);

            var disconArg = await disconnectedTcs.Task.OrTimeout();
            Assert.Equal("reason", disconArg.DisconnectedMessage.Reason);

            var stopArg = await stoppedTcs.Task.OrTimeout();
            Assert.NotNull(stopArg);

            IEnumerable<WebSocketReadResult> GetTestData()
            {
                yield return new WebSocketReadResult(TestUtils.GetConnectedPayload("conn", "user"), false);
                yield return new WebSocketReadResult(TestUtils.GetServerMessagePayload(1), false);
                yield return new WebSocketReadResult(TestUtils.GetGroupMessagePayload(2), false);
                yield return new WebSocketReadResult(TestUtils.GetDisconnectedPayload(), false);
                yield return new WebSocketReadResult(default, true);
            }
        }

        [Fact]
        public async Task WebPubSubClientMultipleConnectedMessageShouldOnlyInvokeOnceTest()
        {
            var packageSeq = GetTestData().ToList();
            var idx = 0;
            _webSocketClientMoc.Setup(c => c.ReceiveOneFrameAsync(It.IsAny<CancellationToken>())).Returns<CancellationToken>(token =>
            {
                return Task.FromResult(packageSeq[idx++]);
            });
            _webSocketClientMoc.SetupGet(c => c.CloseStatus).Returns(WebSocketCloseStatus.PolicyViolation); // Force to not recover

            var connectedTcs = new MultipleTimesTaskCompletionSource<object>(3);

            var client = new WebPubSubClient(new Uri("wss://test.com"), new WebPubSubClientOptions { AutoReconnect = true });
            client.WebSocketClientFactory = _factoryMoc.Object;
            client.Connected += _ =>
            {
                connectedTcs.IncreaseCallTimes();
                return Task.CompletedTask;
            };
            await client.StartAsync();

            await connectedTcs.VerifyCalledTimesAsync(1).OrTimeout();
            await connectedTcs.VerifyCalledTimesAsync(2).OrTimeout();
            TestUtils.AssertTimeout(connectedTcs.VerifyCalledTimesAsync(3));

            IEnumerable<WebSocketReadResult> GetTestData()
            {
                // 3 continous connected message should only invoke once
                yield return new WebSocketReadResult(TestUtils.GetConnectedPayload(), false);
                yield return new WebSocketReadResult(TestUtils.GetConnectedPayload(), false);
                yield return new WebSocketReadResult(TestUtils.GetConnectedPayload(), false);
                yield return new WebSocketReadResult(default, true);
                // after reconnect, there should be another invoke
                yield return new WebSocketReadResult(TestUtils.GetConnectedPayload(), false);
                yield return new WebSocketReadResult(default, true);
            }
        }

        [Fact]
        public async Task RestoreFailedHandlerTest()
        {
            var tcs = new MultipleTimesTaskCompletionSource<WebPubSubRestoreGroupFailedEventArgs>(3);

            var client = new WebPubSubClient(new Uri("wss://test.com"), new WebPubSubClientOptions { AutoReconnect = false });
            client.WebSocketClientFactory = _factoryMoc.Object;
            client.RestoreGroupFailed += arg =>
            {
                tcs.IncreaseCallTimes(arg);
                return Task.CompletedTask;
            };

            // Add two groups
            var groups = client.GetPrivateField<ConcurrentDictionary<string, WebPubSubGroup>>("_groups");
            var group1 = new WebPubSubGroup("group1");
            group1.IsJoined = true;
            groups.TryAdd("group1", group1);
            var group2 = new WebPubSubGroup("group2");
            group2.IsJoined = true;
            groups.TryAdd("group2", group2);

            // Call restore
            client.HandleConnectionConnected(new ConnectedMessage("user", "conn", null), default);

            // verify handler
            var groupSet = new HashSet<string>();
            var rst1 = await tcs.VerifyCalledTimesAsync(1).OrTimeout();
            groupSet.Add(rst1.Group);
            Assert.IsType<SendMessageFailedException>(rst1.Exception);

            var rst2 = await tcs.VerifyCalledTimesAsync(2).OrTimeout();
            groupSet.Add(rst2.Group);
            Assert.IsType<SendMessageFailedException>(rst2.Exception);

            Assert.Contains("group1", groupSet);
            Assert.Contains("group2", groupSet);
        }

        private TaskCompletionSource<T> NewTcs<T>() => new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
    }
}
