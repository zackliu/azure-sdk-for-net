// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.WebPubSub.Common;
using Microsoft.Azure.WebJobs.Description;

namespace Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO
{
    /// <summary>
    /// Attribute used to bind a parameter to an Azure Web PubSub when a Web PubSub for Socket.IO request is coming.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
#pragma warning disable CS0618 // Type or member is obsolete
    [Binding(TriggerHandlesReturnValue = true)]
#pragma warning restore CS0618 // Type or member is obsolete
    public class WebPubSubForSocketIOTriggerAttribute : Attribute
    {
        /// <summary>
        /// Attribute used to bind a parameter to an Azure Web PubSub for Socket.IO, when an request is from Azure Web PubSub for Socket.IO.
        /// </summary>
        /// <param name="hub">Target hub name of the request.</param>
        /// <param name="eventType">Target event name of the request.</param>
        /// <param name="eventName">Target event type of the request.</param>
        /// <param name="namespace">Target namespace.</param>
        public WebPubSubForSocketIOTriggerAttribute(string hub, WebPubSubEventType eventType, string eventName, string @namespace)
        {
            Hub = hub;
            EventName = eventName;
            EventType = eventType;
            Namespace = @namespace;
        }

        /// <summary>
        /// Attribute used to bind a parameter to an Azure Web PubSub for Socket.IO, when an request is from Azure Web PubSub service for Socket.IO.
        /// </summary>
        /// <param name="hub">Target hub name of the request.</param>
        /// <param name="eventType">Target event name of the request.</param>
        /// <param name="eventName">Target event type of the request.</param>
        public WebPubSubForSocketIOTriggerAttribute(string hub, WebPubSubEventType eventType, string eventName)
            : this(hub, eventType, eventName, "/")
        {
        }

        /// <summary>
        /// The hub of request.
        /// </summary>
        [AutoResolve]
        public string Hub { get; }

        /// <summary>
        /// The event of the request.
        /// </summary>
        [Required]
        public string EventName { get; }

        /// <summary>
        /// The event type, allowed value is system or user.
        /// </summary>
        [Required]
        public WebPubSubEventType EventType { get; }

        /// <summary>
        /// The namespace
        /// </summary>
        [Required]
        public string Namespace { get; }
    }
}
