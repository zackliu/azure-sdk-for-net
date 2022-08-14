// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.Messaging.WebPubSub.Client
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

        /// <summary>
        /// protobuf type.
        /// </summary>
        public static readonly DataType Protobuf = new DataType("protobuf");

        public string Name { get; }

        private DataType(string name)
        {
            Name = name;
        }
    }
}
