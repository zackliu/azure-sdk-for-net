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
        /// <param name="namespace">Target namespace</param>
        /// <param name="eventName">Event name.</param>
        /// <param name="arguments">SocketIO data.</param>
        /// <returns>An instance of <see cref="SendToNamespaceAction"></see>.</returns>
        public static SendToNamespaceAction CreateSendToNamespaceAction(string @namespace, string eventName, IEnumerable<object> arguments)
        {
            return new SendToNamespaceAction
            {
                EventName = eventName,
                Arguments = arguments.ToArray(),
                Namespace = @namespace,
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToNamespaceAction"></see> for output binding.
        /// </summary>
        /// <param name="namespace">Target namespace</param>
        /// <param name="rooms">Target rooms</param>
        /// <param name="eventName"> Event name</param>
        /// <param name="arguments">SocketIO data.</param>
        /// <returns>An instance of <see cref="SendToNamespaceAction"></see>.</returns>
        public static SendToRoomsAction CreateSendToRoomsAction(string @namespace, IEnumerable<string> rooms, string eventName, IEnumerable<object> arguments)
        {
            return new SendToRoomsAction
            {
                EventName = eventName,
                Arguments = arguments.ToArray(),
                Namespace = @namespace,
                Rooms = rooms.ToArray(),
            };
        }

        /// <summary>
        /// Creates an instance of <see cref="SendToSocketAction"></see> for output binding.
        /// </summary>
        /// <param name="namespace">Target namespace</param>
        /// <param name="socketId">Target socket</param>
        /// <param name="eventName">Event name</param>
        /// <param name="arguments">SocketIO data.</param>
        /// <returns>An instance of <see cref="SendToSocketAction"></see>.</returns>
        public static SendToSocketAction CreateSendToSocketAction(string @namespace, string socketId, string eventName, IEnumerable<object> arguments)
        {
            return new SendToSocketAction
            {
                EventName = eventName,
                Arguments = arguments.ToArray(),
                SocketId = socketId,
                Namespace = @namespace,
            };
        }
    }
}
