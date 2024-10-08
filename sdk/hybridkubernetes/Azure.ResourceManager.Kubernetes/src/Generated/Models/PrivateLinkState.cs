// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Kubernetes.Models
{
    /// <summary> Property which describes the state of private link on a connected cluster resource. </summary>
    public readonly partial struct PrivateLinkState : IEquatable<PrivateLinkState>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="PrivateLinkState"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public PrivateLinkState(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string EnabledValue = "Enabled";
        private const string DisabledValue = "Disabled";

        /// <summary> Enabled. </summary>
        public static PrivateLinkState Enabled { get; } = new PrivateLinkState(EnabledValue);
        /// <summary> Disabled. </summary>
        public static PrivateLinkState Disabled { get; } = new PrivateLinkState(DisabledValue);
        /// <summary> Determines if two <see cref="PrivateLinkState"/> values are the same. </summary>
        public static bool operator ==(PrivateLinkState left, PrivateLinkState right) => left.Equals(right);
        /// <summary> Determines if two <see cref="PrivateLinkState"/> values are not the same. </summary>
        public static bool operator !=(PrivateLinkState left, PrivateLinkState right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="PrivateLinkState"/>. </summary>
        public static implicit operator PrivateLinkState(string value) => new PrivateLinkState(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is PrivateLinkState other && Equals(other);
        /// <inheritdoc />
        public bool Equals(PrivateLinkState other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
