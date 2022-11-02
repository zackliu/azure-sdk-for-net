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
            var serviceClient = new WebPubSubServiceClient(Environment.GetEnvironmentVariable("AWPS_CONNECTION_STRING"), "hub");

            var client = new WebPubSubClient(new WebPubSubClientCredential(token => 
                new ValueTask<Uri>(serviceClient.GetClientAccessUriAsync(roles: new[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" }))));

            client.Connected += e =>
            {
                Console.WriteLine($"Connection {e.ConnectionId} is connected");
                return Task.CompletedTask;
            };
            client.Disconnected += e =>
            {
                Console.WriteLine($"Connection {e.ConnectionId} is disconnected");
                return Task.CompletedTask;
            };
            client.RestoreGroupFailed += e =>
            {
                Console.WriteLine($"Restore group {e.Group} failed: {e.Exception}");
                return Task.CompletedTask;
            };
            client.ServerMessageReceived += e =>
            {
                Console.WriteLine($"Receive message: {e.Message.Data}");
                return Task.CompletedTask;
            };
            client.GroupMessageReceived += e =>
            {
                Console.WriteLine($"Receive group message from {e.Message.Group}: {e.Message.Data}");
                return Task.CompletedTask;
            };
            
            await client.StartAsync();

            var groupName = "WebPubSubClientTestGroup";
            await client.JoinGroupAsync(groupName);

            await client.SendToGroupAsync(groupName, BinaryData.FromString("hello world"), WebPubSubDataType.Text);
            await client.SendToGroupAsync(groupName, BinaryData.FromObjectAsJson(new
            {
                Foo = "Hello World!",
                Bar = 42
            }), WebPubSubDataType.Json);
            await client.SendToGroupAsync(groupName, BinaryData.FromBytes(Convert.FromBase64String("YXNkZmFzZGZk")), WebPubSubDataType.Binary);

            await Task.Delay(1000);
            await client.StopAsync();

            await client.StartAsync();
            await client.JoinGroupAsync(groupName);
            await client.SendToGroupAsync(groupName, BinaryData.FromString("hello world"), WebPubSubDataType.Text);

            await Task.Delay(1000);
            await client.StopAsync();
        }
    }
}
