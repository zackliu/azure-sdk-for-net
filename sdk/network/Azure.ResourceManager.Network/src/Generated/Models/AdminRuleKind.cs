// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Network.Models
{
    /// <summary> Whether the rule is custom or default. </summary>
    internal readonly partial struct AdminRuleKind : IEquatable<AdminRuleKind>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="AdminRuleKind"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public AdminRuleKind(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string CustomValue = "Custom";
        private const string DefaultValue = "Default";

        /// <summary> Custom. </summary>
        public static AdminRuleKind Custom { get; } = new AdminRuleKind(CustomValue);
        /// <summary> Default. </summary>
        public static AdminRuleKind Default { get; } = new AdminRuleKind(DefaultValue);
        /// <summary> Determines if two <see cref="AdminRuleKind"/> values are the same. </summary>
        public static bool operator ==(AdminRuleKind left, AdminRuleKind right) => left.Equals(right);
        /// <summary> Determines if two <see cref="AdminRuleKind"/> values are not the same. </summary>
        public static bool operator !=(AdminRuleKind left, AdminRuleKind right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="AdminRuleKind"/>. </summary>
        public static implicit operator AdminRuleKind(string value) => new AdminRuleKind(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is AdminRuleKind other && Equals(other);
        /// <inheritdoc />
        public bool Equals(AdminRuleKind other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
