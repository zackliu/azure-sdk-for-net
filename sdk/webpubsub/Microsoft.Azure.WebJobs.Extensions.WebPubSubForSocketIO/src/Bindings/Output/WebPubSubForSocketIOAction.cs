// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// Abstract class of operation to invoke service.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class WebPubSubForSocketIOAction
    {
        internal string ActionName
        {
            get
            {
                return GetType().Name;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="AddSocketToRoomAction"></see> for output binding.
        /// </summary>
        /// <param name="socketId">Target socketId.</param>
        /// <param name="room">Target roome.</param>
        /// <returns>An instance of <see cref="AddSocketToRoomAction"></see>.</returns>
        public static AddSocketToRoomAction CreateAddSocketToRoomAction(string socketId, string room)
        {
            return new AddSocketToRoomAction
            {
                SocketId = socketId,
                Room = room
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="RemoveSocketFromRoomAction"></see> for output binding.
        /// </summary>
        /// <param name="socketId">Target socketId.</param>
        /// <param name="room">Target roome.</param>
        /// <returns>An instance of <see cref="RemoveSocketFromRoomAction"></see>.</returns>
        public static RemoveSocketFromRoomAction CreateRemoveSocketFromRoomAction(string socketId, string room)
        {
            return new RemoveSocketFromRoomAction
            {
                SocketId = socketId,
                Room = room
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="DisconnectSocketsAction"></see> for output binding.
        /// </summary>
        /// <param name="namespace">Target namespace.</param>
        /// <param name="rooms">Target rooms</param>
        /// <returns>An instance of <see cref="DisconnectSocketsAction"></see>.</returns>
        public static DisconnectSocketsAction CreateDisconnectSocketsAction(string @namespace, IEnumerable<string> rooms)
        {
            return new DisconnectSocketsAction
            {
                Namespace = @namespace,
                Rooms = rooms.ToArray(),
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToNamespaceAction"></see> for output binding.
        /// </summary>
        /// <param name="data">SocketIO data.</param>
        /// <param name="namespace">Target namespace</param>
        /// <returns>An instance of <see cref="SendToNamespaceAction"></see>.</returns>
        public static SendToNamespaceAction CreateSendToNamespaceAction(BinaryData data, string @namespace)
        {
            return new SendToNamespaceAction
            {
                Data = data,
                Namespace = @namespace,
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToNamespaceAction"></see> for output binding.
        /// </summary>
        /// <param name="data">SocketIO data.</param>
        /// <param name="namespace">Target namespace</param>
        /// <param name="rooms">Target rooms</param>
        /// <returns>An instance of <see cref="SendToNamespaceAction"></see>.</returns>
        public static SendToRoomsAction CreateSendToRoomsAction(BinaryData data, string @namespace, IEnumerable<string> rooms)
        {
            return new SendToRoomsAction
            {
                Data = data,
                Namespace = @namespace,
                Rooms = rooms.ToArray(),
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToSocketAction"></see> for output binding.
        /// </summary>
        /// <param name="data">SocketIO data.</param>
        /// <param name="socketId">Target socket</param>
        /// <returns>An instance of <see cref="SendToSocketAction"></see>.</returns>
        public static SendToSocketAction CreateSendToSocketAction(BinaryData data, string socketId)
        {
            return new SendToSocketAction
            {
                Data = data,
                SocketId = socketId,
            };
        }
    }
}
