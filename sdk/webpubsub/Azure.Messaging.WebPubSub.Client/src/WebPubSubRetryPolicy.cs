// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    ///   An abstract representation of a policy to govern retrying.
    /// </summary>
    ///
    /// <remarks>
    ///   It is recommended that developers without advanced needs not implement custom retry
    ///   policies but instead configure the default policy.
    /// </remarks>
    public abstract class WebPubSubRetryPolicy
    {
        /// <summary>
        /// This will be called after the transport loses a connection to determine if and for how long to wait before the next reconnect attempt.
        /// </summary>
        /// <param name="retryContext">
        /// Information related to the next possible reconnect attempt including the number of consecutive failed retries so far, time spent
        /// reconnecting so far and the error that lead to this reconnect attempt.
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> representing the amount of time to wait from now before starting the next reconnect attempt.
        /// <see langword="null" /> tells the client to stop retrying and close.
        /// </returns>
        public abstract TimeSpan? NextRetryDelay(RetryContext retryContext);
    }
}
