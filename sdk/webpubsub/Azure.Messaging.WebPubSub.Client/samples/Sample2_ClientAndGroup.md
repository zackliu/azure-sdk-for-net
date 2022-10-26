# Create client with WebPubSubClientCredential and subscribe to specific group

This sample demonstrates the uses of WebPubSubClientCredential and only subscribe to messages from specific interested groups.

## Subscribe group messages

If you are only interested in some specific groups. You can directly add event handler to some groups instead of a general `MessageReceived`. Note that adding an event handler doesn't means the client is joined the group. You need to use a separate join group operation to actually join the group.

```C# Snippet:WebPubSubClient_ClientAndGroup_GroupSubscribe
await client.JoinGroupAsync("testGroup", e =>
{
    Console.WriteLine($"Receive group message from {e.Message.Group}: {e.Message.Data}");
    return Task.CompletedTask;
});
```
