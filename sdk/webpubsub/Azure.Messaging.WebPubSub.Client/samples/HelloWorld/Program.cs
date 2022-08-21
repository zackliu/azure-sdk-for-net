using System.Text;
using Azure.Core;
using Azure.Messaging.WebPubSub;
using Azure.Messaging.WebPubSub.Client;

namespace HelloWorld
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceClient = new WebPubSubServiceClient("", "hub");

            var client = new WebPubSubClient(new WebPubSubClientCredential(new WebPubSubClientCredentialOptions(token => serviceClient.GetClientAccessUriAsync(roles: new[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" }))));
            client.Connected += new SyncAsyncEventHandler<ConnectedEventArgs>(e =>
            {
                Console.WriteLine($"Connection {e.ConnectedMessage.ConnectionId} is connected");
                return Task.CompletedTask;
            });
            client.Disconnected += new SyncAsyncEventHandler<DisconnectedEventArgs>(e =>
            {
                Console.WriteLine($"Connection {client.ConnectionId} is disconnected");
                return Task.CompletedTask;
            });
            await client.ConnectAsync();

            await client.JoinGroupAsync("group1", msg =>
            {
                if (msg.DataType == WebPubSubDataType.Text || msg.DataType == WebPubSubDataType.Json)
                {
                    Console.WriteLine($"Receive group message from {msg.Group}: {msg.Data}");
                }
                else if (msg.DataType == WebPubSubDataType.Binary || msg.DataType == WebPubSubDataType.Protobuf)
                {
                    Console.WriteLine($"Receive group message from {msg.Group}: {msg.Data}");
                }
                
                return Task.CompletedTask;
            });

            await client.SendToGroupAsync("group1", BinaryData.FromString("hello world"), WebPubSubDataType.Text);
            await client.SendToGroupAsync("group1", BinaryData.FromObjectAsJson(new
            {
                Foo = "Hello World!",
                Bar = 42
            }), WebPubSubDataType.Json);
            await client.SendToGroupAsync("group1", BinaryData.FromBytes(Convert.FromBase64String("YXNkZmFzZGZk")), WebPubSubDataType.Binary);

            await Task.Delay(10000);

            await client.StopAsync();
        }
    }
}
