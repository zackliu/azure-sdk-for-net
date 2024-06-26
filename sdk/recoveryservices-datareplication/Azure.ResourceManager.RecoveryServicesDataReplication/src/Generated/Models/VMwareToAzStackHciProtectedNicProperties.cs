// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.RecoveryServicesDataReplication.Models
{
    /// <summary> VMwareToAzStackHCI NIC properties. </summary>
    public partial class VMwareToAzStackHciProtectedNicProperties
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

        /// <summary> Initializes a new instance of <see cref="VMwareToAzStackHciProtectedNicProperties"/>. </summary>
        internal VMwareToAzStackHciProtectedNicProperties()
        {
        }

        /// <summary> Initializes a new instance of <see cref="VMwareToAzStackHciProtectedNicProperties"/>. </summary>
        /// <param name="nicId"> Gets or sets the NIC Id. </param>
        /// <param name="macAddress"> Gets or sets the NIC mac address. </param>
        /// <param name="label"> Gets or sets the NIC label. </param>
        /// <param name="isPrimaryNic"> Gets or sets a value indicating whether this is the primary NIC. </param>
        /// <param name="networkName"> Gets or sets the network name. </param>
        /// <param name="targetNetworkId"> Gets or sets the target network Id within AzStackHCI Cluster. </param>
        /// <param name="testNetworkId"> Gets or sets the target test network Id within AzStackHCI Cluster. </param>
        /// <param name="selectionTypeForFailover"> Gets or sets the selection type of the NIC. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal VMwareToAzStackHciProtectedNicProperties(string nicId, string macAddress, string label, bool? isPrimaryNic, string networkName, string targetNetworkId, string testNetworkId, VmNicSelection? selectionTypeForFailover, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            NicId = nicId;
            MacAddress = macAddress;
            Label = label;
            IsPrimaryNic = isPrimaryNic;
            NetworkName = networkName;
            TargetNetworkId = targetNetworkId;
            TestNetworkId = testNetworkId;
            SelectionTypeForFailover = selectionTypeForFailover;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Gets or sets the NIC Id. </summary>
        public string NicId { get; }
        /// <summary> Gets or sets the NIC mac address. </summary>
        public string MacAddress { get; }
        /// <summary> Gets or sets the NIC label. </summary>
        public string Label { get; }
        /// <summary> Gets or sets a value indicating whether this is the primary NIC. </summary>
        public bool? IsPrimaryNic { get; }
        /// <summary> Gets or sets the network name. </summary>
        public string NetworkName { get; }
        /// <summary> Gets or sets the target network Id within AzStackHCI Cluster. </summary>
        public string TargetNetworkId { get; }
        /// <summary> Gets or sets the target test network Id within AzStackHCI Cluster. </summary>
        public string TestNetworkId { get; }
        /// <summary> Gets or sets the selection type of the NIC. </summary>
        public VmNicSelection? SelectionTypeForFailover { get; }
    }
}
