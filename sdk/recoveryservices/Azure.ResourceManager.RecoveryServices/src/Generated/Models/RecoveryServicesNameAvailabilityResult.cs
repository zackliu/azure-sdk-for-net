// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.RecoveryServices.Models
{
    /// <summary> Response for check name availability API. Resource provider will set availability as true | false. </summary>
    public partial class RecoveryServicesNameAvailabilityResult
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

        /// <summary> Initializes a new instance of <see cref="RecoveryServicesNameAvailabilityResult"/>. </summary>
        internal RecoveryServicesNameAvailabilityResult()
        {
        }

        /// <summary> Initializes a new instance of <see cref="RecoveryServicesNameAvailabilityResult"/>. </summary>
        /// <param name="isNameAvailable"></param>
        /// <param name="reason"></param>
        /// <param name="message"></param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal RecoveryServicesNameAvailabilityResult(bool? isNameAvailable, string reason, string message, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            IsNameAvailable = isNameAvailable;
            Reason = reason;
            Message = message;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Gets the is name available. </summary>
        public bool? IsNameAvailable { get; }
        /// <summary> Gets the reason. </summary>
        public string Reason { get; }
        /// <summary> Gets the message. </summary>
        public string Message { get; }
    }
}
