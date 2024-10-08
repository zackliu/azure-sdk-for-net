// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.NetworkCloud.Models
{
    /// <summary> The more detailed status of the trunked network. </summary>
    public readonly partial struct TrunkedNetworkDetailedStatus : IEquatable<TrunkedNetworkDetailedStatus>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="TrunkedNetworkDetailedStatus"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public TrunkedNetworkDetailedStatus(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ErrorValue = "Error";
        private const string AvailableValue = "Available";
        private const string ProvisioningValue = "Provisioning";

        /// <summary> Error. </summary>
        public static TrunkedNetworkDetailedStatus Error { get; } = new TrunkedNetworkDetailedStatus(ErrorValue);
        /// <summary> Available. </summary>
        public static TrunkedNetworkDetailedStatus Available { get; } = new TrunkedNetworkDetailedStatus(AvailableValue);
        /// <summary> Provisioning. </summary>
        public static TrunkedNetworkDetailedStatus Provisioning { get; } = new TrunkedNetworkDetailedStatus(ProvisioningValue);
        /// <summary> Determines if two <see cref="TrunkedNetworkDetailedStatus"/> values are the same. </summary>
        public static bool operator ==(TrunkedNetworkDetailedStatus left, TrunkedNetworkDetailedStatus right) => left.Equals(right);
        /// <summary> Determines if two <see cref="TrunkedNetworkDetailedStatus"/> values are not the same. </summary>
        public static bool operator !=(TrunkedNetworkDetailedStatus left, TrunkedNetworkDetailedStatus right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="TrunkedNetworkDetailedStatus"/>. </summary>
        public static implicit operator TrunkedNetworkDetailedStatus(string value) => new TrunkedNetworkDetailedStatus(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is TrunkedNetworkDetailedStatus other && Equals(other);
        /// <inheritdoc />
        public bool Equals(TrunkedNetworkDetailedStatus other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
