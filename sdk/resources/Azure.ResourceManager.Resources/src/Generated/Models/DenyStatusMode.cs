// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Resources.Models
{
    /// <summary> denyAssignment settings applied to the resource. </summary>
    public readonly partial struct DenyStatusMode : IEquatable<DenyStatusMode>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="DenyStatusMode"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DenyStatusMode(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DenyDeleteValue = "denyDelete";
        private const string NotSupportedValue = "notSupported";
        private const string InapplicableValue = "inapplicable";
        private const string DenyWriteAndDeleteValue = "denyWriteAndDelete";
        private const string RemovedBySystemValue = "removedBySystem";
        private const string NoneValue = "none";

        /// <summary> Authorized users are able to read and modify the resources, but cannot delete. </summary>
        public static DenyStatusMode DenyDelete { get; } = new DenyStatusMode(DenyDeleteValue);
        /// <summary> Resource type does not support denyAssignments. </summary>
        public static DenyStatusMode NotSupported { get; } = new DenyStatusMode(NotSupportedValue);
        /// <summary> denyAssignments are not supported on resources outside the scope of the deployment stack. </summary>
        public static DenyStatusMode Inapplicable { get; } = new DenyStatusMode(InapplicableValue);
        /// <summary> Authorized users can only read from a resource, but cannot modify or delete it. </summary>
        public static DenyStatusMode DenyWriteAndDelete { get; } = new DenyStatusMode(DenyWriteAndDeleteValue);
        /// <summary> Deny assignment has been removed by Azure due to a resource management change (management group move, etc.). </summary>
        public static DenyStatusMode RemovedBySystem { get; } = new DenyStatusMode(RemovedBySystemValue);
        /// <summary> No denyAssignments have been applied. </summary>
        public static DenyStatusMode None { get; } = new DenyStatusMode(NoneValue);
        /// <summary> Determines if two <see cref="DenyStatusMode"/> values are the same. </summary>
        public static bool operator ==(DenyStatusMode left, DenyStatusMode right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DenyStatusMode"/> values are not the same. </summary>
        public static bool operator !=(DenyStatusMode left, DenyStatusMode right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="DenyStatusMode"/>. </summary>
        public static implicit operator DenyStatusMode(string value) => new DenyStatusMode(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DenyStatusMode other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DenyStatusMode other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
