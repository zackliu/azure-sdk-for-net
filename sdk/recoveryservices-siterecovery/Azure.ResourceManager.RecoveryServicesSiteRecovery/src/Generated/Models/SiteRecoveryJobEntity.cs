// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.RecoveryServicesSiteRecovery.Models
{
    /// <summary> This class contains the minimal job details required to navigate to the desired drill down. </summary>
    public partial class SiteRecoveryJobEntity
    {
        /// <summary>
        /// Keeps track of any properties unknown to the library.
        /// <para>
        /// To assign an object to the value of this property use <see cref="BinaryData.FromObjectAsJson{T}(T, System.Text.Json.JsonSerializerOptions?)"/>.
        /// </para>
        /// <para>
        /// To assign an already formatted json string to this property use <see cref="BinaryData.FromString(string)"/>.
        /// </para>
        /// <para>
        /// Examples:
        /// <list type="bullet">
        /// <item>
        /// <term>BinaryData.FromObjectAsJson("foo")</term>
        /// <description>Creates a payload of "foo".</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromString("\"foo\"")</term>
        /// <description>Creates a payload of "foo".</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromObjectAsJson(new { key = "value" })</term>
        /// <description>Creates a payload of { "key": "value" }.</description>
        /// </item>
        /// <item>
        /// <term>BinaryData.FromString("{\"key\": \"value\"}")</term>
        /// <description>Creates a payload of { "key": "value" }.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        private IDictionary<string, BinaryData> _serializedAdditionalRawData;

        /// <summary> Initializes a new instance of <see cref="SiteRecoveryJobEntity"/>. </summary>
        internal SiteRecoveryJobEntity()
        {
        }

        /// <summary> Initializes a new instance of <see cref="SiteRecoveryJobEntity"/>. </summary>
        /// <param name="jobId"> The job id. </param>
        /// <param name="jobFriendlyName"> The job display name. </param>
        /// <param name="targetObjectId"> The object id. </param>
        /// <param name="targetObjectName"> The object name. </param>
        /// <param name="targetInstanceType"> The workflow affected object type. </param>
        /// <param name="jobScenarioName"> The job name. Enum type ScenarioName. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal SiteRecoveryJobEntity(ResourceIdentifier jobId, string jobFriendlyName, string targetObjectId, string targetObjectName, string targetInstanceType, string jobScenarioName, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            JobId = jobId;
            JobFriendlyName = jobFriendlyName;
            TargetObjectId = targetObjectId;
            TargetObjectName = targetObjectName;
            TargetInstanceType = targetInstanceType;
            JobScenarioName = jobScenarioName;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> The job id. </summary>
        public ResourceIdentifier JobId { get; }
        /// <summary> The job display name. </summary>
        public string JobFriendlyName { get; }
        /// <summary> The object id. </summary>
        public string TargetObjectId { get; }
        /// <summary> The object name. </summary>
        public string TargetObjectName { get; }
        /// <summary> The workflow affected object type. </summary>
        public string TargetInstanceType { get; }
        /// <summary> The job name. Enum type ScenarioName. </summary>
        public string JobScenarioName { get; }
    }
}
