# Azure Web PubSub Client library for .NET

[Azure Web PubSub Service](https://aka.ms/awps/doc) is an Azure-managed service that helps developers easily build web applications with real-time features and publish-subscribe pattern. Any scenario that requires real-time publish-subscribe messaging between server and clients or among clients can use Azure Web PubSub service. Traditional real-time features that often require polling from server or submitting HTTP requests can also use Azure Web PubSub service.

You can use this library in your client side to manage the WebSocket client connections, as shown in below diagram:

![overflow](https://user-images.githubusercontent.com/668244/140014067-25a00959-04dc-47e8-ac25-6957bd0a71ce.png)

Use this library to:

- Send messages to groups
- Send event to event handlers
- Join and leave groups
- Listen messages from groups and servers

Details about the terms used here are described in [Key concepts](#key-concepts) section.

[Source code](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/webpubsub/Azure.Messaging.WebPubSub.Client/src) |
[Package](https://www.nuget.org/packages/Azure.Messaging.WebPubSub.Client) |
[API reference documentation](https://aka.ms/awps/sdk/csharp) |
[Product documentation](https://aka.ms/awps/doc) |
[Samples][samples_ref]

## Getting started

### Install the package

Install the client library from [NuGet](https://www.nuget.org/):

```dotnetcli
dotnet add package Azure.Messaging.WebPubSub.Client
```

### Prerequisites

- An [Azure subscription][azure_sub].
- An existing Azure Web PubSub service instance.

### Create and authenticate a `WebPubSubClient`

In order to interact with the service, you'll need to create an instance of the `WebPubSubClient` class. To make this possible, you'll need the access token. You can copy and paste the access token from Azure portal which you can access in the Azure portal.

```C# Snippet:WebPubSubClientConstruct
var client = new WebPubSubClient(new Uri("ClientAccessUri"));
```

And in production, you usually get `ClientAccessUri` from a negotiate server.

```C# Snippet:WebPubSubClientConstruct2
var client = new WebPubSubClient(new WebPubSubClientCredential(async cancellationToken =>
{
    // FetchClientAccessUri(cancellationToken);
}));
```

## Key concepts

### Connection

A connection, also known as a client or a client connection, represents an individual WebSocket connection connected to the Web PubSub service. When successfully connected, a unique connection ID is assigned to this connection by the Web PubSub service.

### Recovery

If using reliable protocols, a new websocket will try to connect and reuse the connection ID of previous dropped connection. If the websocket connection is successfully connected, the connection is recovered. And all group context will be restored and unreceived messages will be resent.

### Hub

A hub is a logical concept for a set of client connections. Usually you use one hub for one purpose, for example, a chat hub, or a notification hub. When a client connection is created, it connects to a hub, and during its lifetime, it belongs to that hub. Different applications can share one Azure Web PubSub service by using different hub names.

### Group

A group is a subset of connections to the hub. You can add a client connection to a group, or remove the client connection from the group, anytime you want. For example, when a client joins a chat room, or when a client leaves the chat room, this chat room can be considered to be a group. A client can join multiple groups, and a group can contain multiple clients.

### User

Connections to Web PubSub can belong to one user. A user might have multiple connections, for example when a single user is connected across multiple devices or multiple browser tabs.

### Message

When a client is connected, it can send messages to the upstream application, or receive messages from the upstream application, through the WebSocket connection. Also, it can send messages to groups and receive message from joined groups.

## Examples

### Send to groups

```C# Snippet:WebPubSubClientSendToGroup
var client = new WebPubSubClient(new Uri("ClientAccessUri"));

await client.SendToGroupAsync("groupName", BinaryData.FromString("hello world"), WebPubSubDataType.Text);
```

### Send events to event handler

```C# Snippet:WebPubSubClientSendEvent
var client = new WebPubSubClient(new Uri("ClientAccessUri"));

await client.SendEventAsync("eventName", BinaryData.FromString("hello world"), WebPubSubDataType.Text);
```

### Join groups and add listener to groups

```C# Snippet:WebPubSubClientSendEvent
var client = new WebPubSubClient(new Uri("ClientAccessUri"));
client.GroupMessageReceived += e =>
{
    Console.WriteLine($"Receive group message from {e.Message.Group}: {e.Message.Data}");
    return Task.CompletedTask;
};
await client.JoinGroupAsync("groupName");
```

### Add listener to server messages

```C# Snippet:WebPubSubClientSendEvent
var client = new WebPubSubClient(new Uri("ClientAccessUri"));
client.ServerMessageReceived += e =>
{
    Console.WriteLine($"Receive server message: {e.Message.Data}");
    return Task.CompletedTask;
};
```

## Troubleshooting

### Setting up console logging

You can also easily [enable console logging](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/Diagnostics.md#logging) if you want to dig deeper into the requests you're making against the service.

## Next steps

Please take a look at the
[samples][samples_ref]
directory for detailed examples on how to use this library.

You can also find [more samples here][awps_sample].

## Contributing

This project welcomes contributions and suggestions.
Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution.
For details, visit <https://cla.microsoft.com.>

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment).
Simply follow the instructions provided by the bot.
You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

![Impressions](https://azure-sdk-impressions.azurewebsites.net/api/impressions/azure-sdk-for-net%2Fsdk%2Ftemplate%2FAzure.Template%2FREADME.png)

[azure_sub]: https://azure.microsoft.com/free/dotnet/
[samples_ref]: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/webpubsub/Azure.Messaging.WebPubSub.Client/sample-dev/HelloWorld
[awps_sample]: https://github.com/Azure/azure-webpubsub/tree/main/samples/csharp
