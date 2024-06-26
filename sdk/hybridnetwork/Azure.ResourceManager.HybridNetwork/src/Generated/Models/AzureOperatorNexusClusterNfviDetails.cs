// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.HybridNetwork.Models
{
    /// <summary> The AzureOperatorNexusCluster NFVI detail. </summary>
    public partial class AzureOperatorNexusClusterNfviDetails : NFVIs
    {
        /// <summary> Initializes a new instance of <see cref="AzureOperatorNexusClusterNfviDetails"/>. </summary>
        public AzureOperatorNexusClusterNfviDetails()
        {
            NfviType = NfviType.AzureOperatorNexus;
        }

        /// <summary> Initializes a new instance of <see cref="AzureOperatorNexusClusterNfviDetails"/>. </summary>
        /// <param name="name"> Name of the nfvi. </param>
        /// <param name="nfviType"> The NFVI type. </param>
        /// <param name="serializedAdditionalRawData"> Keeps track of any properties unknown to the library. </param>
        /// <param name="customLocationReference"> The reference to the custom location. </param>
        internal AzureOperatorNexusClusterNfviDetails(string name, NfviType nfviType, IDictionary<string, BinaryData> serializedAdditionalRawData, WritableSubResource customLocationReference) : base(name, nfviType, serializedAdditionalRawData)
        {
            CustomLocationReference = customLocationReference;
            NfviType = nfviType;
        }

        /// <summary> The reference to the custom location. </summary>
        internal WritableSubResource CustomLocationReference { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier CustomLocationReferenceId
        {
            get => CustomLocationReference is null ? default : CustomLocationReference.Id;
            set
            {
                if (CustomLocationReference is null)
                    CustomLocationReference = new WritableSubResource();
                CustomLocationReference.Id = value;
            }
        }
    }
}
