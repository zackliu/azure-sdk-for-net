// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Batch.Protocol
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// PoolOperations operations.
    /// </summary>
    public partial interface IPoolOperations
    {
        /// <summary>
        /// Lists the usage metrics, aggregated by Pool across individual time
        /// intervals, for the specified Account.
        /// </summary>
        /// <remarks>
        /// If you do not specify a $filter clause including a poolId, the
        /// response includes all Pools that existed in the Account in the time
        /// range of the returned aggregation intervals. If you do not specify
        /// a $filter clause including a startTime or endTime these filters
        /// default to the start and end times of the last aggregation interval
        /// currently available; that is, only the last aggregation interval is
        /// returned.
        /// </remarks>
        /// <param name='poolListUsageMetricsOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<PoolUsageMetrics>,PoolListUsageMetricsHeaders>> ListUsageMetricsWithHttpMessagesAsync(PoolListUsageMetricsOptions poolListUsageMetricsOptions = default(PoolListUsageMetricsOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Adds a Pool to the specified Account.
        /// </summary>
        /// <remarks>
        /// When naming Pools, avoid including sensitive information such as
        /// user names or secret project names. This information may appear in
        /// telemetry logs accessible to Microsoft Support engineers.
        /// </remarks>
        /// <param name='pool'>
        /// The Pool to be added.
        /// </param>
        /// <param name='poolAddOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolAddHeaders>> AddWithHttpMessagesAsync(PoolAddParameter pool, PoolAddOptions poolAddOptions = default(PoolAddOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists all of the Pools which be mounted
        /// </summary>
        /// <param name='poolListOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<CloudPool>,PoolListHeaders>> ListWithHttpMessagesAsync(PoolListOptions poolListOptions = default(PoolListOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Deletes a Pool from the specified Account.
        /// </summary>
        /// <remarks>
        /// When you request that a Pool be deleted, the following actions
        /// occur: the Pool state is set to deleting; any ongoing resize
        /// operation on the Pool are stopped; the Batch service starts
        /// resizing the Pool to zero Compute Nodes; any Tasks running on
        /// existing Compute Nodes are terminated and requeued (as if a resize
        /// Pool operation had been requested with the default requeue option);
        /// finally, the Pool is removed from the system. Because running Tasks
        /// are requeued, the user can rerun these Tasks by updating their Job
        /// to target a different Pool. The Tasks can then run on the new Pool.
        /// If you want to override the requeue behavior, then you should call
        /// resize Pool explicitly to shrink the Pool to zero size before
        /// deleting the Pool. If you call an Update, Patch or Delete API on a
        /// Pool in the deleting state, it will fail with HTTP status code 409
        /// with error code PoolBeingDeleted.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool to delete.
        /// </param>
        /// <param name='poolDeleteOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolDeleteHeaders>> DeleteWithHttpMessagesAsync(string poolId, PoolDeleteOptions poolDeleteOptions = default(PoolDeleteOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets basic properties of a Pool.
        /// </summary>
        /// <param name='poolId'>
        /// The ID of the Pool to get.
        /// </param>
        /// <param name='poolExistsOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<bool,PoolExistsHeaders>> ExistsWithHttpMessagesAsync(string poolId, PoolExistsOptions poolExistsOptions = default(PoolExistsOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets information about the specified Pool.
        /// </summary>
        /// <param name='poolId'>
        /// The ID of the Pool to get.
        /// </param>
        /// <param name='poolGetOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<CloudPool,PoolGetHeaders>> GetWithHttpMessagesAsync(string poolId, PoolGetOptions poolGetOptions = default(PoolGetOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates the properties of the specified Pool.
        /// </summary>
        /// <remarks>
        /// This only replaces the Pool properties specified in the request.
        /// For example, if the Pool has a StartTask associated with it, and a
        /// request does not specify a StartTask element, then the Pool keeps
        /// the existing StartTask.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool to update.
        /// </param>
        /// <param name='poolPatchParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='poolPatchOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolPatchHeaders>> PatchWithHttpMessagesAsync(string poolId, PoolPatchParameter poolPatchParameter, PoolPatchOptions poolPatchOptions = default(PoolPatchOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Disables automatic scaling for a Pool.
        /// </summary>
        /// <param name='poolId'>
        /// The ID of the Pool on which to disable automatic scaling.
        /// </param>
        /// <param name='poolDisableAutoScaleOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolDisableAutoScaleHeaders>> DisableAutoScaleWithHttpMessagesAsync(string poolId, PoolDisableAutoScaleOptions poolDisableAutoScaleOptions = default(PoolDisableAutoScaleOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Enables automatic scaling for a Pool.
        /// </summary>
        /// <remarks>
        /// You cannot enable automatic scaling on a Pool if a resize operation
        /// is in progress on the Pool. If automatic scaling of the Pool is
        /// currently disabled, you must specify a valid autoscale formula as
        /// part of the request. If automatic scaling of the Pool is already
        /// enabled, you may specify a new autoscale formula and/or a new
        /// evaluation interval. You cannot call this API for the same Pool
        /// more than once every 30 seconds.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool on which to enable automatic scaling.
        /// </param>
        /// <param name='poolEnableAutoScaleParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='poolEnableAutoScaleOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolEnableAutoScaleHeaders>> EnableAutoScaleWithHttpMessagesAsync(string poolId, PoolEnableAutoScaleParameter poolEnableAutoScaleParameter, PoolEnableAutoScaleOptions poolEnableAutoScaleOptions = default(PoolEnableAutoScaleOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets the result of evaluating an automatic scaling formula on the
        /// Pool.
        /// </summary>
        /// <remarks>
        /// This API is primarily for validating an autoscale formula, as it
        /// simply returns the result without applying the formula to the Pool.
        /// The Pool must have auto scaling enabled in order to evaluate a
        /// formula.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool on which to evaluate the automatic scaling
        /// formula.
        /// </param>
        /// <param name='autoScaleFormula'>
        /// The formula for the desired number of Compute Nodes in the Pool.
        /// The formula is validated and its results calculated, but it is not
        /// applied to the Pool. To apply the formula to the Pool, 'Enable
        /// automatic scaling on a Pool'. For more information about specifying
        /// this formula, see Automatically scale Compute Nodes in an Azure
        /// Batch Pool
        /// (https://azure.microsoft.com/documentation/articles/batch-automatic-scaling).
        /// </param>
        /// <param name='poolEvaluateAutoScaleOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<AutoScaleRun,PoolEvaluateAutoScaleHeaders>> EvaluateAutoScaleWithHttpMessagesAsync(string poolId, string autoScaleFormula, PoolEvaluateAutoScaleOptions poolEvaluateAutoScaleOptions = default(PoolEvaluateAutoScaleOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Changes the number of Compute Nodes that are assigned to a Pool.
        /// </summary>
        /// <remarks>
        /// You can only resize a Pool when its allocation state is steady. If
        /// the Pool is already resizing, the request fails with status code
        /// 409. When you resize a Pool, the Pool's allocation state changes
        /// from steady to resizing. You cannot resize Pools which are
        /// configured for automatic scaling. If you try to do this, the Batch
        /// service returns an error 409. If you resize a Pool downwards, the
        /// Batch service chooses which Compute Nodes to remove. To remove
        /// specific Compute Nodes, use the Pool remove Compute Nodes API
        /// instead.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool to resize.
        /// </param>
        /// <param name='poolResizeParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='poolResizeOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolResizeHeaders>> ResizeWithHttpMessagesAsync(string poolId, PoolResizeParameter poolResizeParameter, PoolResizeOptions poolResizeOptions = default(PoolResizeOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Stops an ongoing resize operation on the Pool.
        /// </summary>
        /// <remarks>
        /// This does not restore the Pool to its previous state before the
        /// resize operation: it only stops any further changes being made, and
        /// the Pool maintains its current state. After stopping, the Pool
        /// stabilizes at the number of Compute Nodes it was at when the stop
        /// operation was done. During the stop operation, the Pool allocation
        /// state changes first to stopping and then to steady. A resize
        /// operation need not be an explicit resize Pool request; this API can
        /// also be used to halt the initial sizing of the Pool when it is
        /// created.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool whose resizing you want to stop.
        /// </param>
        /// <param name='poolStopResizeOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolStopResizeHeaders>> StopResizeWithHttpMessagesAsync(string poolId, PoolStopResizeOptions poolStopResizeOptions = default(PoolStopResizeOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates the properties of the specified Pool.
        /// </summary>
        /// <remarks>
        /// This fully replaces all the updatable properties of the Pool. For
        /// example, if the Pool has a StartTask associated with it and if
        /// StartTask is not specified with this request, then the Batch
        /// service will remove the existing StartTask.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool to update.
        /// </param>
        /// <param name='poolUpdatePropertiesParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='poolUpdatePropertiesOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolUpdatePropertiesHeaders>> UpdatePropertiesWithHttpMessagesAsync(string poolId, PoolUpdatePropertiesParameter poolUpdatePropertiesParameter, PoolUpdatePropertiesOptions poolUpdatePropertiesOptions = default(PoolUpdatePropertiesOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Removes Compute Nodes from the specified Pool.
        /// </summary>
        /// <remarks>
        /// This operation can only run when the allocation state of the Pool
        /// is steady. When this operation runs, the allocation state changes
        /// from steady to resizing. Each request may remove up to 100 nodes.
        /// </remarks>
        /// <param name='poolId'>
        /// The ID of the Pool from which you want to remove Compute Nodes.
        /// </param>
        /// <param name='nodeRemoveParameter'>
        /// The parameters for the request.
        /// </param>
        /// <param name='poolRemoveNodesOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<PoolRemoveNodesHeaders>> RemoveNodesWithHttpMessagesAsync(string poolId, NodeRemoveParameter nodeRemoveParameter, PoolRemoveNodesOptions poolRemoveNodesOptions = default(PoolRemoveNodesOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists the usage metrics, aggregated by Pool across individual time
        /// intervals, for the specified Account.
        /// </summary>
        /// <remarks>
        /// If you do not specify a $filter clause including a poolId, the
        /// response includes all Pools that existed in the Account in the time
        /// range of the returned aggregation intervals. If you do not specify
        /// a $filter clause including a startTime or endTime these filters
        /// default to the start and end times of the last aggregation interval
        /// currently available; that is, only the last aggregation interval is
        /// returned.
        /// </remarks>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='poolListUsageMetricsNextOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<PoolUsageMetrics>,PoolListUsageMetricsHeaders>> ListUsageMetricsNextWithHttpMessagesAsync(string nextPageLink, PoolListUsageMetricsNextOptions poolListUsageMetricsNextOptions = default(PoolListUsageMetricsNextOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists all of the Pools which be mounted
        /// </summary>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='poolListNextOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<CloudPool>,PoolListHeaders>> ListNextWithHttpMessagesAsync(string nextPageLink, PoolListNextOptions poolListNextOptions = default(PoolListNextOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
