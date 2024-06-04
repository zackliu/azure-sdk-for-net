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
        /// <param name="namespace">Target namespace.</param>
        /// <param name="socketId">Target socketId.</param>
        /// <param name="room">Target roome.</param>
        /// <returns>An instance of <see cref="AddSocketToRoomAction"></see>.</returns>
        public static AddSocketToRoomAction CreateAddSocketToRoomAction(string @namespace, string socketId, string room)
        {
            return new AddSocketToRoomAction
            {
                Namespace = @namespace,
                SocketId = socketId,
                Room = room
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="RemoveSocketFromRoomAction"></see> for output binding.
        /// </summary>
        /// <param name="namespace">Target namespace.</param>
        /// <param name="socketId">Target socketId.</param>
        /// <param name="room">Target roome.</param>
        /// <returns>An instance of <see cref="RemoveSocketFromRoomAction"></see>.</returns>
        public static RemoveSocketFromRoomAction CreateRemoveSocketFromRoomAction(string @namespace, string socketId, string room)
        {
            return new RemoveSocketFromRoomAction
            {
                Namespace = @namespace,
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
        public static SendToNamespaceAction CreateSendToNamespaceAction(IEnumerable<object> data, string @namespace)
        {
            return new SendToNamespaceAction
            {
                Data = data.ToArray(),
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
        public static SendToRoomsAction CreateSendToRoomsAction(IEnumerable<object> data, string @namespace, IEnumerable<string> rooms)
        {
            return new SendToRoomsAction
            {
                Data = data.ToArray(),
                Namespace = @namespace,
                Rooms = rooms.ToArray(),
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToSocketAction"></see> for output binding.
        /// </summary>
        /// <param name="data">SocketIO data.</param>
        /// <param name="socketId">Target socket</param>
        /// <param name="namespace">Target namespace</param>
        /// <returns>An instance of <see cref="SendToSocketAction"></see>.</returns>
        public static SendToSocketAction CreateSendToSocketAction(IEnumerable<object> data, string @namespace, string socketId)
        {
            return new SendToSocketAction
            {
                Data = data.ToArray(),
                SocketId = socketId,
                Namespace = @namespace,
            };
        }
    }
}
