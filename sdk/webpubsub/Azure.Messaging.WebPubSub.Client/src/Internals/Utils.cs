// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Azure.Core;

namespace Azure.Messaging.WebPubSub.Clients
{
    internal static class Utils
    {
        internal static RetryOptions GetRetryOptions()
        {
            return (RetryOptions)typeof(RetryOptions).GetConstructor(
                  BindingFlags.NonPublic | BindingFlags.Instance,
                  null, Type.EmptyTypes, null).Invoke(null);
        }
    }
}
