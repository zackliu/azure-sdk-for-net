namespace Azure.Messaging.WebPubSub.Clients
{
    public partial class AckMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public AckMessage(ulong ackId, bool success, Azure.Messaging.WebPubSub.Clients.ErrorDetail error) { }
        public ulong AckId { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.ErrorDetail Error { get { throw null; } }
        public bool Success { get { throw null; } }
    }
    public partial class AckResult
    {
        internal AckResult() { }
        public Azure.Messaging.WebPubSub.Clients.AckMessage AckMessage { get { throw null; } }
    }
    public partial class ConnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal ConnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.ConnectedMessage ConnectedMessage { get { throw null; } }
        public Azure.Messaging.WebPubSub.Clients.GroupsInfo Groups { get { throw null; } }
    }
    public partial class ConnectedMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public ConnectedMessage(string userId, string connectionId, string reconnectionToken) { }
        public string ConnectionId { get { throw null; } }
        public string ReconnectionToken { get { throw null; } }
        public string UserId { get { throw null; } }
    }
    public partial class DisconnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal DisconnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.DisconnectedMessage DisconnectedMessage { get { throw null; } }
    }
    public partial class DisconnectedMessage : Azure.Messaging.WebPubSub.Clients.WebPubSubMessage
    {
        public DisconnectedMessage(string reason) { }
        public string Reason { get { throw null; } }
    }
    public partial class ErrorDetail
    {
        public ErrorDetail(string name, string message) { }
        public string Message { get { throw null; } }
        public string Name { get { throw null; } }
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
    public partial class GroupMessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal GroupMessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.GroupDataMessage GroupDataMessage { get { throw null; } }
    }
    public partial class GroupsInfo
    {
        internal GroupsInfo() { }
        public System.Collections.Generic.IReadOnlyList<string> List { get { throw null; } }
        public System.Threading.Tasks.Task RestoreAsync(System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
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
    public partial class ProcessMessageFailedException : System.Exception
    {
        internal ProcessMessageFailedException() { }
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
        public Azure.Messaging.WebPubSub.Clients.AckMessage AckMessage { get { throw null; } }
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
    public partial class ServerMessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal ServerMessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Clients.ServerDataMessage ServerDataMessage { get { throw null; } }
    }
    public partial class WebPubSubClient : System.IAsyncDisposable, System.IDisposable
    {
        protected WebPubSubClient() { }
        public WebPubSubClient(Azure.Messaging.WebPubSub.Clients.WebPubSubClientCredential credential, Azure.Messaging.WebPubSub.Clients.WebPubSubClientOptions options = null) { }
        public WebPubSubClient(System.Uri clientAccessUri) { }
        public string ConnectionId { get { throw null; } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.ConnectedEventArgs> Connected { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.DisconnectedEventArgs> Disconnected { add { } remove { } }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public System.Threading.Tasks.ValueTask DisposeAsync() { throw null; }
        protected virtual System.Threading.Tasks.ValueTask DisposeAsyncCore() { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.AckResult> JoinGroupAsync(string group, ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.AckResult> LeaveGroupAsync(string group, ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.IDisposable OnGroupMessage(string group, Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.GroupMessageEventArgs> handler) { throw null; }
        public virtual System.IDisposable OnGroupMessages(Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.GroupMessageEventArgs> handler) { throw null; }
        public virtual System.IDisposable OnServerMessages(Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Clients.ServerMessageEventArgs> handler) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.AckResult> SendEventAsync(string eventName, System.BinaryData content, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId = default(ulong?), bool fireAndForget = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Clients.AckResult> SendToGroupAsync(string group, System.BinaryData content, Azure.Messaging.WebPubSub.Clients.WebPubSubDataType dataType, ulong? ackId = default(ulong?), bool noEcho = false, bool fireAndForget = false, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class WebPubSubClientCredential
    {
        public WebPubSubClientCredential(Azure.Messaging.WebPubSub.Clients.WebPubSubClientCredentialOptions options) { }
        public WebPubSubClientCredential(System.Uri clientAccessUri) { }
        public System.Threading.Tasks.Task<System.Uri> GetClientAccessUri(System.Threading.CancellationToken token) { throw null; }
    }
    public partial class WebPubSubClientCredentialOptions
    {
        public WebPubSubClientCredentialOptions(System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<System.Uri>> clientAccessUriProvider) { }
        public System.Func<System.Threading.CancellationToken, System.Threading.Tasks.Task<System.Uri>> ClientAccessUriProvider { get { throw null; } }
    }
    public partial class WebPubSubClientOptions
    {
        public WebPubSubClientOptions() { }
        public Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol Protocol { get { throw null; } set { } }
        public Azure.Messaging.WebPubSub.Clients.ReconnectionOptions ReconnectionOptions { get { throw null; } set { } }
    }
    public enum WebPubSubDataType
    {
        Binary = 0,
        Json = 1,
        Text = 2,
        Protobuf = 3,
    }
    public partial class WebPubSubJsonProtocol : Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol
    {
        public WebPubSubJsonProtocol() { }
        public override bool IsReliable { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get { throw null; } }
        public override System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message) { throw null; }
        public override Azure.Messaging.WebPubSub.Clients.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input) { throw null; }
        public override void WriteMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output) { }
    }
    public partial class WebPubSubJsonReliableProtocol : Azure.Messaging.WebPubSub.Clients.WebPubSubProtocol
    {
        public WebPubSubJsonReliableProtocol() { }
        public override bool IsReliable { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get { throw null; } }
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
        public abstract System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get; }
        public abstract System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message);
        public abstract Azure.Messaging.WebPubSub.Clients.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input);
        public abstract void WriteMessage(Azure.Messaging.WebPubSub.Clients.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output);
    }
}
