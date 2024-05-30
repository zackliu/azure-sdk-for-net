// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.WebPubSubForSocketIO;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Extension methods for Azure Web PubSub for Socket.IO integration.
    /// </summary>
    public static class WebPubSubJobsBuilderExtensions
    {
        /// <summary>
        /// Adds the Web PubSub for Socket.IO extensions to the provided <see cref="IWebJobsBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IWebJobsBuilder"/> to configure.</param>
        /// <returns><see cref="IWebJobsBuilder"/>.</returns>
        public static IWebJobsBuilder AddWebPubSubForSocketIO(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<WebPubSubForSocketIOConfigProvider>()
                .ConfigureOptions<WebPubSubFunctionsOptions>(ApplyConfiguration);
            return builder;
        }

        private static void ApplyConfiguration(IConfiguration config, WebPubSubFunctionsOptions options)
        {
            if (config == null)
            {
                return;
            }

            config.Bind(options);
        }
    }
}
