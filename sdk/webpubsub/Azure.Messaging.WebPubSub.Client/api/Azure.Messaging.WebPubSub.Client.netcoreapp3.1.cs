namespace Azure.Messaging.WebPubSub.Clients
{
    public partial class AckMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public AckMessage(ulong ackId, bool success, Azure.Messaging.WebPubSub.Clients.AckMessageError error) { }
        public ulong AckId { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.AckMessageError Error { get { throw null; } }
        public bool Success { get { throw null; } }
    }
    public partial class AckMessageError
    {
        public AckMessageError(string name, string message) { }
        public string Message { get { throw null; } }
        public string Name { get { throw null; } }
    }
    public partial class ConnectedMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public ConnectedMessage(string userId, string connectionId, string reconnectionToken) { }
        public string ConnectionId { get { throw null; } }
        public string ReconnectionToken { get { throw null; } }
        public string UserId { get { throw null; } }
    }
    public partial class DisconnectedMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public DisconnectedMessage(string reason) { }
        public string Reason { get { throw null; } }
    }
    public partial class GroupDataMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public GroupDataMessage(string group, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, System.BinaryData data, ulong? sequenceId, string fromUserId) { }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubDataType DataType { get { throw null; } }
        public string FromUserId { get { throw null; } }
        public string Group { get { throw null; } }
        public ulong? SequenceId { get { throw null; } }
    }
    public partial class JoinGroupMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public JoinGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class LeaveGroupMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public LeaveGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class ReconnectionOptions
    {
        public ReconnectionOptions() { }
        public bool AutoReconnect { get { throw null; } set { } }
    }
    public partial class SendEventMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public SendEventMessage(string eventName, System.BinaryData data, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubDataType DataType { get { throw null; } }
        public string EventName { get { throw null; } }
    }
    public partial class SendMessageFailedException : System.Exception
    {
        internal SendMessageFailedException() { }
        public ulong? AckId { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.AckMessageError Error { get { throw null; } }
    }
    public partial class SendToGroupMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public SendToGroupMessage(string group, System.BinaryData data, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId, bool noEcho) { }
        public ulong? AckId { get { throw null; } }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubDataType DataType { get { throw null; } }
        public string Group { get { throw null; } }
        public bool NoEcho { get { throw null; } }
    }
    public partial class SequenceAckMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public SequenceAckMessage(ulong sequenceId) { }
        public ulong SequenceId { get { throw null; } }
    }
    public partial class ServerDataMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public ServerDataMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, System.BinaryData data, ulong? sequenceId) { }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubDataType DataType { get { throw null; } }
        public ulong? SequenceId { get { throw null; } }
    }
    public partial class WebPubSubClient : System.IAsyncDisposable
    {
        protected WebPubSubClient() { }
        public WebPubSubClient(Azure.Messaging.WebPubSub.Clients.WebPubSubClientCredential credential, Azure.Messaging.WebPubSub.Clients.WebPubSubClientOptions options = null) { }
        public WebPubSubClient(System.Uri clientAccessUri) { }
        public string ConnectionId { get { throw null; } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubConnectedEventArgs> Connected { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubDisconnectedEventArgs> Disconnected { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubGroupMessageEventArgs> GroupMessageReceived { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubServerMessageEventArgs> ServerMessageReceived { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubStoppedEventArgs> Stopped { add { } remove { } }
        public System.Threading.Tasks.ValueTask DisposeAsync() { throw null; }
        protected virtual System.Threading.Tasks.ValueTask DisposeAsyncCore() { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.WebPubSubResult> JoinGroupAsync(string group, Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.WebPubSubGroupMessageEventArgs> handler, ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.WebPubSubResult> JoinGroupAsync(string group, ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.WebPubSubResult> LeaveGroupAsync(string group, ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.WebPubSubResult> SendEventAsync(string eventName, System.BinaryData content, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId = default(ulong?), bool fireAndForget = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.WebPubSubResult> SendToGroupAsync(string group, System.BinaryData content, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId = default(ulong?), bool noEcho = false, bool fireAndForget = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task StopAsync() { throw null; }
    }
    public partial class WebPubSubClientCredential
    {
        public WebPubSubClientCredential(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.ValueTask<System.Uri>> clientAccessUriProvider) { }
        public WebPubSubClientCredential(System.Uri clientAccessUri) { }
        public System.Threading.Tasks.ValueTask<System.Uri> GetClientAccessUriAsync(System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class WebPubSubClientOptions
    {
        public WebPubSubClientOptions() { }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol Protocol { get { throw null; } set { } }
        public Azure.Messaging.WebPubSub.Clients.ReconnectionOptions ReconnectionOptions { get { throw null; } set { } }
    }
    public partial class WebPubSubConnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal WebPubSubConnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public string ConnectionId { get { throw null; } }
        public System.Collections.Generic.IReadOnlyDictionary<string, System.Exception> GroupRestoreState { get { throw null; } }
        public string UserId { get { throw null; } }
    }
    public enum WebPubSubDataType
    {
        Binary = 0,
        Json = 1,
        Text = 2,
        Protobuf = 3,
    }
    public partial class WebPubSubDisconnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal WebPubSubDisconnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.DisconnectedMessage DisconnectedMessage { get { throw null; } }
    }
    public partial class WebPubSubGroupMessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal WebPubSubGroupMessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.GroupDataMessage Message { get { throw null; } }
    }
    public partial class WebPubSubJsonProtocol : Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol
    {
        public WebPubSubJsonProtocol() { }
        public override bool IsReliable { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Azure.Messaging.WebPubSub.Clients.WebPubSubProtocolMessageType WebSocketMessageType { get { throw null; } }
        public override System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message) { throw null; }
        public override Azure.Messaging.WebPubSub.Clients.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input) { throw null; }
        public override void WriteMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output) { }
    }
    public partial class WebPubSubJsonReliableProtocol : Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol
    {
        public WebPubSubJsonReliableProtocol() { }
        public override bool IsReliable { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Azure.Messaging.WebPubSub.Clients.WebPubSubProtocolMessageType WebSocketMessageType { get { throw null; } }
        public override System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message) { throw null; }
        public override Azure.Messaging.WebPubSub.Clients.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input) { throw null; }
        public override void WriteMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output) { }
    }
    public abstract partial class WebPubSubMessage
    {
        protected WebPubSubMessage() { }
    }
    public abstract partial class WebPubSubProtocol
    {
        protected WebPubSubProtocol() { }
        public abstract bool IsReliable { get; }
        public abstract string Name { get; }
        public abstract Azure.Messaging.WebPubSub.Clients.WebPubSubProtocolMessageType WebSocketMessageType { get; }
        public abstract System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message);
        public abstract Azure.Messaging.WebPubSub.Clients.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input);
        public abstract void WriteMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output);
    }
    public enum WebPubSubProtocolMessageType
    {
        Text = 0,
        Binary = 1,
    }
    public partial class WebPubSubResult
    {
        internal WebPubSubResult() { }
        public ulong AckId { get { throw null; } }
    }
    public partial class WebPubSubServerMessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal WebPubSubServerMessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.ServerDataMessage Message { get { throw null; } }
    }
    public partial class WebPubSubStoppedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal WebPubSubStoppedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
    }
}
