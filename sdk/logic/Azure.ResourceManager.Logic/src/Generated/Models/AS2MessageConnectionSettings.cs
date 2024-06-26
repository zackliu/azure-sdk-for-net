// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.ResourceManager.Logic.Models
{
    /// <summary> The AS2 agreement message connection settings. </summary>
    public partial class AS2MessageConnectionSettings
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

        /// <summary> Initializes a new instance of <see cref="AS2MessageConnectionSettings"/>. </summary>
        /// <param name="ignoreCertificateNameMismatch"> The value indicating whether to ignore mismatch in certificate name. </param>
        /// <param name="supportHttpStatusCodeContinue"> The value indicating whether to support HTTP status code 'CONTINUE'. </param>
        /// <param name="keepHttpConnectionAlive"> The value indicating whether to keep the connection alive. </param>
        /// <param name="unfoldHttpHeaders"> The value indicating whether to unfold the HTTP headers. </param>
        public AS2MessageConnectionSettings(bool ignoreCertificateNameMismatch, bool supportHttpStatusCodeContinue, bool keepHttpConnectionAlive, bool unfoldHttpHeaders)
        {
            IgnoreCertificateNameMismatch = ignoreCertificateNameMismatch;
            SupportHttpStatusCodeContinue = supportHttpStatusCodeContinue;
            KeepHttpConnectionAlive = keepHttpConnectionAlive;
            UnfoldHttpHeaders = unfoldHttpHeaders;
        }

        /// <summary> Initializes a new instance of <see cref="AS2MessageConnectionSettings"/>. </summary>
        /// <param name="ignoreCertificateNameMismatch"> The value indicating whether to ignore mismatch in certificate name. </param>
        /// <param name="supportHttpStatusCodeContinue"> The value indicating whether to support HTTP status code 'CONTINUE'. </param>
        /// <param name="keepHttpConnectionAlive"> The value indicating whether to keep the connection alive. </param>
        /// <param name="unfoldHttpHeaders"> The value indicating whether to unfold the HTTP headers. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal AS2MessageConnectionSettings(bool ignoreCertificateNameMismatch, bool supportHttpStatusCodeContinue, bool keepHttpConnectionAlive, bool unfoldHttpHeaders, IDictionary<string, BinaryData> serializedAdditionalRawData)
        {
            IgnoreCertificateNameMismatch = ignoreCertificateNameMismatch;
            SupportHttpStatusCodeContinue = supportHttpStatusCodeContinue;
            KeepHttpConnectionAlive = keepHttpConnectionAlive;
            UnfoldHttpHeaders = unfoldHttpHeaders;
            _serializedAdditionalRawData = serializedAdditionalRawData;
        }

        /// <summary> Initializes a new instance of <see cref="AS2MessageConnectionSettings"/> for deserialization. </summary>
        internal AS2MessageConnectionSettings()
        {
        }

        /// <summary> The value indicating whether to ignore mismatch in certificate name. </summary>
        public bool IgnoreCertificateNameMismatch { get; set; }
        /// <summary> The value indicating whether to support HTTP status code 'CONTINUE'. </summary>
        public bool SupportHttpStatusCodeContinue { get; set; }
        /// <summary> The value indicating whether to keep the connection alive. </summary>
        public bool KeepHttpConnectionAlive { get; set; }
        /// <summary> The value indicating whether to unfold the HTTP headers. </summary>
        public bool UnfoldHttpHeaders { get; set; }
    }
}
