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
    /// WebPubSubClientCredential
    /// </summary>
    public class WebPubSubClientCredential
    {
        private readonly WebPubSubClientCredentialOptions _options;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientAccessUri"></param>
        public WebPubSubClientCredential(Uri clientAccessUri): this(new WebPubSubClientCredentialOptions(_ => Task.FromResult(clientAccessUri)))
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="options"></param>
        public WebPubSubClientCredential(WebPubSubClientCredentialOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// GetClientAccessUri
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Uri> GetClientAccessUri(CancellationToken token)
        {
            return _options.ClientAccessUriProvider.Invoke(token);
        }
    }
}
