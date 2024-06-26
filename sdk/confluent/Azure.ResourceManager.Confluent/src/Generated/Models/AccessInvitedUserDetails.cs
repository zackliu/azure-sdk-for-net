// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.Confluent.Models
{
    /// <summary> Details of the user being invited. </summary>
    public partial class AccessInvitedUserDetails
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

        /// <summary> Initializes a new instance of <see cref="AccessInvitedUserDetails"/>. </summary>
        public AccessInvitedUserDetails()
        {
        }

        /// <summary> Initializes a new instance of <see cref="AccessInvitedUserDetails"/>. </summary>
        /// <param name="invitedEmail"> UPN/Email of the user who is being invited. </param>
        /// <param name="authType"> Auth type of the user. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal AccessInvitedUserDetails(string invitedEmail, string authType, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            InvitedEmail = invitedEmail;
            AuthType = authType;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> UPN/Email of the user who is being invited. </summary>
        public string InvitedEmail { get; set; }
        /// <summary> Auth type of the user. </summary>
        public string AuthType { get; set; }
    }
}
