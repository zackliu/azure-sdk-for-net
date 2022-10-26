// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    /// <summary>
    /// Web PubSub Group operations
    /// </summary>
    internal class WebPubSubGroup
    {
        private volatile bool _isJoined;
        private event SyncAsyncEventHandler<WebPubSubGroupMessageEventArgs> _messageReceived;
        private List<SyncAsyncEventHandler<WebPubSubGroupMessageEventArgs>> _allDelegates = new();

        public bool IsJoined { get { return _isJoined; } set { _isJoined = value; } }

        internal WebPubSubGroup(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of group
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A event triggered when received messages from group
        /// </summary>
        public event SyncAsyncEventHandler<WebPubSubGroupMessageEventArgs> MessageReceived
        {
            add { _messageReceived += value; _allDelegates.Add(value); }
            remove { _messageReceived -= value; _allDelegates.Remove(value); }
        }

        public void ClearMessageReceivedEvents()
        {
            foreach (var d in _allDelegates)
            {
                MessageReceived -= d;
            }
            _allDelegates.Clear();
        }

        internal async Task SafeHandleMessageAsync(GroupDataMessage message, CancellationToken token)
        {
            try
            {
                await _messageReceived.RaiseAsync(new WebPubSubGroupMessageEventArgs(message, false, token), nameof(GroupDataMessage), nameof(MessageReceived)).ConfigureAwait(false);
            }
            catch
            {
            }
        }
    }
}
