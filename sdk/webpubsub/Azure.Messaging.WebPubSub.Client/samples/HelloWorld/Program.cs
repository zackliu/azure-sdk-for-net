using System.Text;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.WebPubSub;
using Azure.Messaging.WebPubSub.Client;

namespace HelloWorld
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var serviceClient = new WebPubSubServiceClient("", "hub");
            var serviceClient = new WebPubSubServiceClient("", "hub");

            var client = new WebPubSubClient(new WebPubSubClientCredential(new WebPubSubClientCredentialOptions(token => serviceClient.GetClientAccessUriAsync(roles: new[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" }))));
            client.Connected += new (e =>
            {
                Console.WriteLine($"Connection {e.ConnectedMessage.ConnectionId} is connected");
                return Task.CompletedTask;
            });
            client.Disconnected += new (e =>
            {
                Console.WriteLine($"Connection {client.ConnectionId} is disconnected");
                return Task.CompletedTask;
            });
            client.Group("group1").MessageReceived += new (e =>
            {
                Console.WriteLine($"Receive group message from {e.GroupResponseMessage.Group}: {e.GroupResponseMessage.Data}");
                return Task.CompletedTask;
            });

            await client.ConnectAsync();

            await client.Group("group1").JoinAsync();
            
            //await client.Group("group1").SendAsync(BinaryData.FromString("hello world"), WebPubSubDataType.Text);
            //await client.Group("group1").SendAsync(BinaryData.FromObjectAsJson(new
            //{
            //    Foo = "Hello World!",
            //    Bar = 42
            //}), WebPubSubDataType.Json);
            await client.Group("group1").SendAsync(BinaryData.FromBytes(Convert.FromBase64String("YXNkZmFzZGZk")), WebPubSubDataType.Binary);

            await Task.Delay(3000);

            await client.StopAsync();

            await Task.Delay(100000);
        }
    }
}
