// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client.Models
{
    /// <summary>
    /// Represent the type of the data.
    /// </summary>
    public record DataType
    {
        /// <summary>
        /// json type.
        /// </summary>
        public static readonly DataType Json = new DataType("json");

        /// <summary>
        /// test type.
        /// </summary>
        public static readonly DataType Text = new DataType("text");

        /// <summary>
        /// binary type.
        /// </summary>
        public static readonly DataType Binary = new DataType("binary");

        private readonly string _name;

        private DataType(string name)
        {
            _name = name;
        }
    }
}
