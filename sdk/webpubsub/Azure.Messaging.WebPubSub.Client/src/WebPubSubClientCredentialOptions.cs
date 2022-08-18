// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Messaging.WebPubSub.Client
{
    /// <summary>
    /// WebPubSubClientCredentialOptions
    /// </summary>
    public class WebPubSubClientCredentialOptions
    {
        /// <summary>
        /// ClientAccessUriProvider
        /// </summary>
        public Func<CancellationToken, Task<Uri>> ClientAccessUriProvider { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientAccessUriProvider"></param>
        public WebPubSubClientCredentialOptions(Func<CancellationToken, Task<Uri>> clientAccessUriProvider)
        {
            ClientAccessUriProvider = clientAccessUriProvider;
        }
    }
}
