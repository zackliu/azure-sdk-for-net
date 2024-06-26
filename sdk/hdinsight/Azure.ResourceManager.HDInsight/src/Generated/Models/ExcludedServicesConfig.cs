// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.HDInsight.Models
{
    /// <summary> The configuration that services will be excluded when creating cluster. </summary>
    public partial class ExcludedServicesConfig
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

        /// <summary> Initializes a new instance of <see cref="ExcludedServicesConfig"/>. </summary>
        public ExcludedServicesConfig()
        {
        }

        /// <summary> Initializes a new instance of <see cref="ExcludedServicesConfig"/>. </summary>
        /// <param name="excludedServicesConfigId"> The config id of excluded services. </param>
        /// <param name="excludedServicesList"> The list of excluded services. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal ExcludedServicesConfig(string excludedServicesConfigId, string excludedServicesList, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            ExcludedServicesConfigId = excludedServicesConfigId;
            ExcludedServicesList = excludedServicesList;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> The config id of excluded services. </summary>
        public string ExcludedServicesConfigId { get; set; }
        /// <summary> The list of excluded services. </summary>
        public string ExcludedServicesList { get; set; }
    }
}
