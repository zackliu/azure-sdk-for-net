namespace Azure.Messaging.WebPubSub.Client
{
    public partial class AckMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public AckMessage(ulong ackId, bool success, Azure.Messaging.WebPubSub.Client.ErrorDetail error) { }
        public ulong AckId { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.ErrorDetail Error { get { throw null; } }
        public bool Success { get { throw null; } }
    }
    public partial class ConnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal ConnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Client.ConnectedMessage ConnectedMessage { get { throw null; } }
    }
    public partial class ConnectedMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public ConnectedMessage(string userId, string connectionId, string reconnectionToken) { }
        public string ConnectionId { get { throw null; } }
        public string ReconnectionToken { get { throw null; } }
        public string UserId { get { throw null; } }
    }
    public abstract partial class DataMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        protected DataMessage(Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, System.BinaryData data, ulong? sequenceId) { }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.WebPubSubDataType DataType { get { throw null; } }
        public ulong? SequenceId { get { throw null; } }
    }
    public partial class DisconnectedEventArgs : Azure.SyncAsyncEventArgs
    {
        internal DisconnectedEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Client.DisconnectedMessage DisconnectedMessage { get { throw null; } }
    }
    public partial class DisconnectedMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
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
    public partial class GroupDataMessage : Azure.Messaging.WebPubSub.Client.DataMessage
    {
        public GroupDataMessage(string group, Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, System.BinaryData data, ulong? sequenceId, string fromUserId) : base (default(Azure.Messaging.WebPubSub.Client.WebPubSubDataType), default(System.BinaryData), default(ulong?)) { }
        public string FromUserId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class GroupMessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal GroupMessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Client.GroupDataMessage GroupDataMessage { get { throw null; } }
    }
    public partial interface IWebPubSubProtocol
    {
        bool IsReliableSubProtocol { get; }
        string Name { get; }
        System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get; }
        System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message);
        Azure.Messaging.WebPubSub.Client.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input);
        void WriteMessage(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output);
    }
    public partial class JoinGroupMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public JoinGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class LeaveGroupMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public LeaveGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class MessageEventArgs : Azure.SyncAsyncEventArgs
    {
        internal MessageEventArgs() : base (default(bool), default(System.Threading.CancellationToken)) { }
        public Azure.Messaging.WebPubSub.Client.DataMessage DataMessage { get { throw null; } }
    }
    public partial class ProcessMessageFailedException : System.Exception
    {
        internal ProcessMessageFailedException() { }
    }
    public partial class ReconnectionOptions
    {
        public ReconnectionOptions() { }
        public bool AutoReconnect { get { throw null; } set { } }
        public bool AutoRejoinGroups { get { throw null; } set { } }
    }
    public partial class SendEventMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public SendEventMessage(string eventName, System.BinaryData data, Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.WebPubSubDataType DataType { get { throw null; } }
        public string EventName { get { throw null; } }
    }
    public partial class SendMessageFailedException : System.Exception
    {
        internal SendMessageFailedException() { }
        public Azure.Messaging.WebPubSub.Client.AckMessage AckMessage { get { throw null; } }
    }
    public partial class SendToGroupMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public SendToGroupMessage(string group, System.BinaryData data, Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, ulong? ackId, bool noEcho) { }
        public ulong? AckId { get { throw null; } }
        public System.BinaryData Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.WebPubSubDataType DataType { get { throw null; } }
        public string Group { get { throw null; } }
        public bool NoEcho { get { throw null; } }
    }
    public partial class SendToGroupOptions
    {
        public SendToGroupOptions() { }
        public bool FireAndForget { get { throw null; } set { } }
        public bool NoEcho { get { throw null; } set { } }
    }
    public partial class SendToServerOptions
    {
        public SendToServerOptions() { }
        public bool FireAndForget { get { throw null; } set { } }
    }
    public partial class SequenceAckMessage : Azure.Messaging.WebPubSub.Client.WebPubSubMessage
    {
        public SequenceAckMessage(ulong sequenceId) { }
        public ulong SequenceId { get { throw null; } }
    }
    public partial class ServerDataMessage : Azure.Messaging.WebPubSub.Client.DataMessage
    {
        public ServerDataMessage(Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, System.BinaryData data, ulong? sequenceId) : base (default(Azure.Messaging.WebPubSub.Client.WebPubSubDataType), default(System.BinaryData), default(ulong?)) { }
    }
    public partial class WebPubSubClient : System.IDisposable
    {
        protected WebPubSubClient() { }
        public WebPubSubClient(Azure.Messaging.WebPubSub.Client.WebPubSubClientCredential credential, Azure.Messaging.WebPubSub.Client.WebPubSubClientOptions options = null) { }
        public WebPubSubClient(System.Uri clientAccessUri) { }
        public string ConnectionId { get { throw null; } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Client.ConnectedEventArgs> Connected { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Client.DisconnectedEventArgs> Disconnected { add { } remove { } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Client.MessageEventArgs> MessageReceived { add { } remove { } }
        public void Abort() { }
        public virtual System.Threading.Tasks.Task ConnectAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public virtual Azure.Messaging.WebPubSub.Client.WebPubSubGroup Group(string name) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.AckMessage> SendToServerAsync(string eventName, System.BinaryData content, Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, ulong? ackId = default(ulong?), System.Action<Azure.Messaging.WebPubSub.Client.SendToServerOptions> optionsBuilder = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class WebPubSubClientCredential
    {
        public WebPubSubClientCredential(Azure.Messaging.WebPubSub.Client.WebPubSubClientCredentialOptions options) { }
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
        public Azure.Messaging.WebPubSub.Client.IWebPubSubProtocol Protocol { get { throw null; } set { } }
        public Azure.Messaging.WebPubSub.Client.ReconnectionOptions ReconnectionOptions { get { throw null; } set { } }
    }
    public enum WebPubSubDataType
    {
        Binary = 0,
        Json = 1,
        Text = 2,
        Protobuf = 3,
    }
    public partial class WebPubSubGroup
    {
        internal WebPubSubGroup() { }
        public string Name { get { throw null; } }
        public event Azure.Core.SyncAsyncEventHandler<Azure.Messaging.WebPubSub.Client.GroupMessageEventArgs> MessageReceived { add { } remove { } }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.AckMessage> JoinAsync(ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.AckMessage> LeaveAsync(ulong? ackId = default(ulong?), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.AckMessage> SendAsync(System.BinaryData content, Azure.Messaging.WebPubSub.Client.WebPubSubDataType dataType, ulong? ackId = default(ulong?), System.Action<Azure.Messaging.WebPubSub.Client.SendToGroupOptions> optionsBuilder = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class WebPubSubJsonProtocol : Azure.Messaging.WebPubSub.Client.IWebPubSubProtocol
    {
        public WebPubSubJsonProtocol() { }
        public bool IsReliableSubProtocol { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get { throw null; } }
        public System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message) { throw null; }
        public Azure.Messaging.WebPubSub.Client.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input) { throw null; }
        public void WriteMessage(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output) { }
    }
    public partial class WebPubSubJsonReliableProtocol : Azure.Messaging.WebPubSub.Client.IWebPubSubProtocol
    {
        public WebPubSubJsonReliableProtocol() { }
        public bool IsReliableSubProtocol { get { throw null; } }
        public string Name { get { throw null; } }
        public System.Net.WebSockets.WebSocketMessageType WebSocketMessageType { get { throw null; } }
        public System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message) { throw null; }
        public Azure.Messaging.WebPubSub.Client.WebPubSubMessage ParseMessage(System.Buffers.ReadOnlySequence<byte> input) { throw null; }
        public void WriteMessage(Azure.Messaging.WebPubSub.Client.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output) { }
    }
    public abstract partial class WebPubSubMessage
    {
        protected WebPubSubMessage() { }
    }
}
