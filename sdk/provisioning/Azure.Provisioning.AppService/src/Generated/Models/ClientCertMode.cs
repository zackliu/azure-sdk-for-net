// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

namespace Azure.Provisioning.AppService;

/// <summary>
/// This composes with ClientCertEnabled setting.             -
/// ClientCertEnabled: false means ClientCert is ignored.             -
/// ClientCertEnabled: true and ClientCertMode: Required means ClientCert is
/// required.             - ClientCertEnabled: true and ClientCertMode:
/// Optional means ClientCert is optional or accepted.
/// </summary>
public enum ClientCertMode
{
    /// <summary>
    /// Required.
    /// </summary>
    Required,

    /// <summary>
    /// Optional.
    /// </summary>
    Optional,

    /// <summary>
    /// OptionalInteractiveUser.
    /// </summary>
    OptionalInteractiveUser,
}
