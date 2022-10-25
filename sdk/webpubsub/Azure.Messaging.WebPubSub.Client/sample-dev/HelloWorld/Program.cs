using System.Text;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.WebPubSub;
using Azure.Messaging.WebPubSub.Clients;

namespace HelloWorld
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceClient = new WebPubSubServiceClient("", "hub");

            var client = new WebPubSubClient(new WebPubSubClientCredential(token => 
                new ValueTask<Uri>(serviceClient.GetClientAccessUriAsync(roles: new[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" }))));

            client.Connected += new(async e =>
            {
                Console.WriteLine($"Connection {e.ConnectedMessage.ConnectionId} is connected");
            });
            client.Disconnected += new(e =>
            {
                Console.WriteLine($"Connection {client.ConnectionId} is disconnected");
                return Task.CompletedTask;
            });
            client.ServerMessageReceived += new(e =>
            {
                Console.WriteLine($"Receive message: {e.Message.Data}");
                return Task.CompletedTask;
            });
            client.GroupMessageReceived += new(e =>
            {
                Console.WriteLine($"Receive group message from {e.Message.Group}: {e.Message.Data}");
                return Task.CompletedTask;
            });
            
            await client.StartAsync();

            await client.JoinGroupAsync("group1");
            await client.SendToGroupAsync("group1", BinaryData.FromString("hello world"), WebPubSubDataType.Text, fireAndForget:true);
            await client.SendToGroupAsync("group1", BinaryData.FromObjectAsJson(new
            {
                Foo = "Hello World!",
                Bar = 42
            }), WebPubSubDataType.Json);
            await client.SendToGroupAsync("group1", BinaryData.FromBytes(Convert.FromBase64String("YXNkZmFzZGZk")), WebPubSubDataType.Binary);
            await client.SendEventAsync("eventName", BinaryData.FromString("hello world"), WebPubSubDataType.Text);
            await Task.Delay(3000);

            await client.DisposeAsync();
        }
    }
}
