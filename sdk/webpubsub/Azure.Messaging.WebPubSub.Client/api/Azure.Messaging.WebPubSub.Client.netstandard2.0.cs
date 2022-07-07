namespace Azure.Messaging.WebPubSub.Client
{
    public sealed partial class RetryContext
    {
        public RetryContext() { }
        public System.TimeSpan ElapsedTime { get { throw null; } set { } }
        public long PreviousRetryCount { get { throw null; } set { } }
        public System.Exception RetryReason { get { throw null; } set { } }
    }
    public partial class WebPubSubClient : System.IAsyncDisposable
    {
        protected WebPubSubClient() { }
        public WebPubSubClient(System.Func<System.Uri> uriProvider, Azure.Messaging.WebPubSub.Client.Models.WebPubSubClientOptions clientOptions) { }
        public WebPubSubClient(System.Uri uri) { }
        public event System.Func<Azure.Messaging.WebPubSub.Client.Protocols.ConnectedMessage, System.Threading.Tasks.Task> OnConnected { add { } remove { } }
        public event System.Func<Azure.Messaging.WebPubSub.Client.Protocols.DisconnectedMessage, System.Threading.Tasks.Task> OnDisconnected { add { } remove { } }
        public event System.Func<Azure.Messaging.WebPubSub.Client.Protocols.DisconnectedMessage, System.Threading.Tasks.Task> OnSuspended { add { } remove { } }
        public virtual System.Threading.Tasks.Task ConnectAsync(System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.ValueTask DisposeAsync() { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.Protocols.AckMessage> JoinGroupAsync(string groupName, System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.Protocols.AckMessage> LeaveGroupAsync(string groupName, System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task SendEventAsync(Azure.Core.RequestContent content, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.Protocols.AckMessage> SendEventWithAckAsync(Azure.Core.RequestContent content, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, ulong? ackId = default(ulong?), System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task SendToGroupAsync(string groupName, Azure.Core.RequestContent content, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, Azure.Messaging.WebPubSub.Client.Models.SendToGroupOptions operations = null, System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Messaging.WebPubSub.Client.Protocols.AckMessage> SendToGroupWithAckAsync(string groupName, Azure.Core.RequestContent content, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, Azure.Messaging.WebPubSub.Client.Models.SendToGroupOptions operations = null, ulong? ackId = default(ulong?), System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken token = default(System.Threading.CancellationToken)) { throw null; }
    }
    public abstract partial class WebPubSubRetryPolicy
    {
        protected WebPubSubRetryPolicy() { }
        public abstract System.TimeSpan? NextRetryDelay(Azure.Messaging.WebPubSub.Client.RetryContext retryContext);
    }
}
namespace Azure.Messaging.WebPubSub.Client.Models
{
    public partial class DataType : System.IEquatable<Azure.Messaging.WebPubSub.Client.Models.DataType>
    {
        public static readonly Azure.Messaging.WebPubSub.Client.Models.DataType Binary;
        public static readonly Azure.Messaging.WebPubSub.Client.Models.DataType Json;
        public static readonly Azure.Messaging.WebPubSub.Client.Models.DataType Text;
        protected DataType(Azure.Messaging.WebPubSub.Client.Models.DataType original) { }
        protected virtual System.Type EqualityContract { get { throw null; } }
        public virtual bool Equals(Azure.Messaging.WebPubSub.Client.Models.DataType? other) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Azure.Messaging.WebPubSub.Client.Models.DataType? left, Azure.Messaging.WebPubSub.Client.Models.DataType? right) { throw null; }
        public static bool operator !=(Azure.Messaging.WebPubSub.Client.Models.DataType? left, Azure.Messaging.WebPubSub.Client.Models.DataType? right) { throw null; }
        protected virtual bool PrintMembers(System.Text.StringBuilder builder) { throw null; }
        public override string ToString() { throw null; }
        public virtual Azure.Messaging.WebPubSub.Client.Models.DataType <Clone>$() { throw null; }
    }
    public partial class SendToGroupOptions
    {
        public SendToGroupOptions() { }
        public bool NoEcho { get { throw null; } set { } }
    }
    public partial class WebPubSubClientOptions
    {
        public WebPubSubClientOptions() { }
        public Azure.Messaging.WebPubSub.Client.Protocols.IWebPubSubProtocol Protocol { get { throw null; } set { } }
        public Azure.Messaging.WebPubSub.Client.WebPubSubRetryPolicy ReconnectionPolicy { get { throw null; } set { } }
        public Azure.Messaging.WebPubSub.Client.WebPubSubRetryPolicy RecoveryPolicy { get { throw null; } set { } }
    }
}
namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    public partial class AckMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public AckMessage(ulong ackId, bool success, Azure.Messaging.WebPubSub.Client.Protocols.ErrorDetail error) { }
        public ulong AckId { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.Protocols.ErrorDetail Error { get { throw null; } }
        public bool Success { get { throw null; } }
    }
    public partial class ConnectedMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public ConnectedMessage(string userId, string connectionId, string reconnectionToken) { }
        public string ConnectionId { get { throw null; } }
        public string ReconnectionToken { get { throw null; } }
        public string UserId { get { throw null; } }
    }
    public partial class DisconnectedMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
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
    public partial class GroupResponseMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        protected GroupResponseMessage(string group, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, System.Buffers.ReadOnlySequence<byte> data, ulong? sequenceId) { }
        public System.Buffers.ReadOnlySequence<byte> Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.Models.DataType DataType { get { throw null; } }
        public string Group { get { throw null; } }
        public ulong? SequenceId { get { throw null; } }
    }
    public partial interface IWebPubSubProtocol
    {
        bool IsReliableSubProtocol { get; }
        string Name { get; }
        System.ReadOnlyMemory<byte> GetMessageBytes(Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage message);
        bool TryParseMessage(ref System.Buffers.ReadOnlySequence<byte> input, out Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage message);
        void WriteMessage(Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage message, System.Buffers.IBufferWriter<byte> output);
    }
    public partial class JoinGroupMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public JoinGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class LeaveGroupMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public LeaveGroupMessage(string group, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public string Group { get { throw null; } }
    }
    public partial class SendEventMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public SendEventMessage(System.Buffers.ReadOnlySequence<byte> data, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, ulong? ackId) { }
        public ulong? AckId { get { throw null; } }
        public System.Buffers.ReadOnlySequence<byte> Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.Models.DataType DataType { get { throw null; } }
    }
    public partial class SendToGroupMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public SendToGroupMessage(string group, System.Buffers.ReadOnlySequence<byte> data, Azure.Messaging.WebPubSub.Client.Models.DataType dataType, ulong? ackId, bool noEcho) { }
        public ulong? AckId { get { throw null; } }
        public System.Buffers.ReadOnlySequence<byte> Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.Models.DataType DataType { get { throw null; } }
        public string Group { get { throw null; } }
        public bool NoEcho { get { throw null; } }
    }
    public partial class SequenceAckMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        public SequenceAckMessage(ulong sequenceId) { }
        public ulong SequenceId { get { throw null; } }
    }
    public partial class ServerResponseMessage : Azure.Messaging.WebPubSub.Client.Protocols.WebPubSubMessage
    {
        protected ServerResponseMessage(Azure.Messaging.WebPubSub.Client.Models.DataType dataType, System.Buffers.ReadOnlySequence<byte> data, ulong? sequenceId) { }
        public System.Buffers.ReadOnlySequence<byte> Data { get { throw null; } }
        public Azure.Messaging.WebPubSub.Client.Models.DataType DataType { get { throw null; } }
        public ulong? SequenceId { get { throw null; } }
    }
    public abstract partial class WebPubSubMessage
    {
        protected WebPubSubMessage() { }
    }
}
