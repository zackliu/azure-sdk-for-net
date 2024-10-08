// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Sql.Models
{
    /// <summary> Specifies the state of ledger digest upload. </summary>
    public readonly partial struct ManagedLedgerDigestUploadsState : IEquatable<ManagedLedgerDigestUploadsState>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ManagedLedgerDigestUploadsState"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ManagedLedgerDigestUploadsState(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string EnabledValue = "Enabled";
        private const string DisabledValue = "Disabled";

        /// <summary> Enabled. </summary>
        public static ManagedLedgerDigestUploadsState Enabled { get; } = new ManagedLedgerDigestUploadsState(EnabledValue);
        /// <summary> Disabled. </summary>
        public static ManagedLedgerDigestUploadsState Disabled { get; } = new ManagedLedgerDigestUploadsState(DisabledValue);
        /// <summary> Determines if two <see cref="ManagedLedgerDigestUploadsState"/> values are the same. </summary>
        public static bool operator ==(ManagedLedgerDigestUploadsState left, ManagedLedgerDigestUploadsState right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ManagedLedgerDigestUploadsState"/> values are not the same. </summary>
        public static bool operator !=(ManagedLedgerDigestUploadsState left, ManagedLedgerDigestUploadsState right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="ManagedLedgerDigestUploadsState"/>. </summary>
        public static implicit operator ManagedLedgerDigestUploadsState(string value) => new ManagedLedgerDigestUploadsState(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ManagedLedgerDigestUploadsState other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ManagedLedgerDigestUploadsState other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
