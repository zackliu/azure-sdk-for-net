// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client.Protocols
{
    /// <summary>
    /// The WebPubSub client protocol
    /// </summary>
    public interface IWebPubSubProtocol
    {
        /// <summary>
        /// Gets the name of the protocol. The name is used by Web PubSub client to resolve the protocol between the client and server.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates a new <see cref="WebPubSubMessage"/> from the specified serialized representation。
        /// </summary>
        /// <param name="input">The serialized representation of the message.</param>
        /// <returns>A <see cref="WebPubSubMessage"/></returns>
        WebPubSubMessage ParseMessage(ReadOnlySequence<byte> input);

        /// <summary>
        /// Writes the specified <see cref="WebPubSubMessage"/> to a writer.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="output">The output writer.</param>
        void WriteMessage(WebPubSubMessage message, IBufferWriter<byte> output);

        /// <summary>
        /// Converts the specified <see cref="WebPubSubMessage"/> to its serialized representation.
        /// </summary>
        /// <param name="message">The message to convert.</param>
        /// <returns>The serialized representation of the message.</returns>
        ReadOnlyMemory<byte> GetMessageBytes(WebPubSubMessage message);

        /// <summary>
        /// Get whether the protocol using a reliable subprotocl.
        /// </summary>
        bool IsReliableSubProtocol { get; }
    }
}
