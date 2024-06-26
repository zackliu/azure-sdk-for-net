// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using Azure.Core;

namespace Azure.Data.Tables
{
    internal partial class TableCreateHeaders
    {
        private readonly Response _response;
        public TableCreateHeaders(Response response)
        {
            _response = response;
        }
        /// <summary> Indicates the version of the Table service used to execute the request. This header is returned for requests made against version 2009-09-19 and above. </summary>
        public string Version => _response.Headers.TryGetValue("x-ms-version", out string value) ? value : null;
        /// <summary> Indicates whether the Prefer request header was honored. If the response does not include this header, then the Prefer header was not honored. If this header is returned, its value will either be return-content or return-no-content. </summary>
        public string PreferenceApplied => _response.Headers.TryGetValue("Preference-Applied", out string value) ? value : null;
    }
}
