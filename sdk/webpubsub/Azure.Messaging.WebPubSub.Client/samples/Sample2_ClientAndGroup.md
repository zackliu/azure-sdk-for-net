# Create client with WebPubSubClientCredential and subscribe to specific group

This sample demonstrates the uses of WebPubSubClientCredential and only subscribe to messages from specific interested groups.

## Create the clients with WebPubSubClientCredential

When creating client, a common practice is to use a `WebPubSubClientCredential` and pass in a function instread of a `ClientAccessUri`. The client will invoke the function to retreive a new uri the moment when underlying connection drops to avoid token expiration.

```C# Snippet:WebPubSubClient_ClientAndGroup_CreateClient
var serviceClient = new WebPubSubServiceClient("<< CONNECTION STRING >>", "hub");

var client = new WebPubSubClient(new WebPubSubClientCredential(new WebPubSubClientCredentialOptions(token => {
    return Task.FromResult(serviceClient.GetClientAccessUriAsync(roles: new[] { "webpubsub.joinLeaveGroup", "webpubsub.sendToGroup" }));
})));
```

## Subscribe group messages

If you are only interested in some specific groups. You can directly add event handler to some groups instead of a general `MessageReceived`. Note that adding an event handler doesn't means the client is joined the group. You need to use a separate join group operation to actually join the group.

```C# Snippet:WebPubSubClient_ClientAndGroup_GroupSubscribe
client.OnGroupMessage("testGroup", e =>
{
    Console.WriteLine($"Receive group message from {e.GroupDataMessage.Group}: {e.GroupDataMessage.Data}");
    return Task.CompletedTask;
});
```
