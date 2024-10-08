// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Batch.Protocol.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ManagedDisk
    {
        /// <summary>
        /// Initializes a new instance of the ManagedDisk class.
        /// </summary>
        public ManagedDisk()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ManagedDisk class.
        /// </summary>
        /// <param name="storageAccountType">The storage account type for
        /// managed disk.</param>
        /// <param name="securityProfile">Specifies the security profile
        /// settings for the managed disk.</param>
        public ManagedDisk(StorageAccountType? storageAccountType = default(StorageAccountType?), VMDiskSecurityProfile securityProfile = default(VMDiskSecurityProfile))
        {
            StorageAccountType = storageAccountType;
            SecurityProfile = securityProfile;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the storage account type for managed disk.
        /// </summary>
        /// <remarks>
        /// Possible values include: 'StandardLRS', 'PremiumLRS',
        /// 'StandardSSDLRS'
        /// </remarks>
        [JsonProperty(PropertyName = "storageAccountType")]
        public StorageAccountType? StorageAccountType { get; set; }

        /// <summary>
        /// Gets or sets specifies the security profile settings for the
        /// managed disk.
        /// </summary>
        [JsonProperty(PropertyName = "securityProfile")]
        public VMDiskSecurityProfile SecurityProfile { get; set; }

    }
}
