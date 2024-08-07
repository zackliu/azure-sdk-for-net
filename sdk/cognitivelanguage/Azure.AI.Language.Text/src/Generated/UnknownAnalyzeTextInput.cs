// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace Azure.AI.Language.Text
{
    /// <summary> Unknown version of AnalyzeTextInput. </summary>
    internal partial class UnknownAnalyzeTextInput : AnalyzeTextInput
    {
        /// <summary> Initializes a new instance of <see cref="UnknownAnalyzeTextInput"/>. </summary>
        /// <param name="kind"> The kind of task to perform. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        internal UnknownAnalyzeTextInput(AnalyzeTextInputKind kind, IDictionary<string, BinaryData> serializedAdditionalRawData) : base(kind, serializedAdditionalRawData)
        {
        }

        /// <summary> Initializes a new instance of <see cref="UnknownAnalyzeTextInput"/> for deserialization. </summary>
        internal UnknownAnalyzeTextInput()
        {
        }
    }
}
