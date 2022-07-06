// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// The context passed to <see cref="WebPubSubRetryPolicy.NextRetryDelay(RetryContext)"/> to help the policy determine
    /// how long to wait before the next retry and whether there should be another retry at all.
    /// </summary>
    public sealed class RetryContext
    {
        /// <summary>
        /// The number of consecutive failed retries so far.
        /// </summary>
        public long PreviousRetryCount { get; set; }

        /// <summary>
        /// The amount of time spent retrying so far.
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// The error precipitating the current retry if any.
        /// </summary>
        public Exception RetryReason { get; set; }
    }
}
