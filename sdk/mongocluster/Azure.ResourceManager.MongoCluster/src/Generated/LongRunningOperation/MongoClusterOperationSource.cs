// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.ResourceManager.MongoCluster
{
    internal class MongoClusterOperationSource : IOperationSource<MongoClusterResource>
    {
        private readonly ArmClient _client;

        internal MongoClusterOperationSource(ArmClient client)
        {
            _client = client;
        }

        MongoClusterResource IOperationSource<MongoClusterResource>.CreateResult(Response response, CancellationToken cancellationToken)
        {
            using var document = JsonDocument.Parse(response.ContentStream);
            var data = MongoClusterData.DeserializeMongoClusterData(document.RootElement);
            return new MongoClusterResource(_client, data);
        }

        async ValueTask<MongoClusterResource> IOperationSource<MongoClusterResource>.CreateResultAsync(Response response, CancellationToken cancellationToken)
        {
            using var document = await JsonDocument.ParseAsync(response.ContentStream, default, cancellationToken).ConfigureAwait(false);
            var data = MongoClusterData.DeserializeMongoClusterData(document.RootElement);
            return new MongoClusterResource(_client, data);
        }
    }
}
