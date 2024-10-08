// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Automation.Models
{
    /// <summary> The provisioning state of the job. </summary>
    public readonly partial struct SourceControlProvisioningState : IEquatable<SourceControlProvisioningState>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="SourceControlProvisioningState"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public SourceControlProvisioningState(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string CompletedValue = "Completed";
        private const string FailedValue = "Failed";
        private const string RunningValue = "Running";

        /// <summary> Completed. </summary>
        public static SourceControlProvisioningState Completed { get; } = new SourceControlProvisioningState(CompletedValue);
        /// <summary> Failed. </summary>
        public static SourceControlProvisioningState Failed { get; } = new SourceControlProvisioningState(FailedValue);
        /// <summary> Running. </summary>
        public static SourceControlProvisioningState Running { get; } = new SourceControlProvisioningState(RunningValue);
        /// <summary> Determines if two <see cref="SourceControlProvisioningState"/> values are the same. </summary>
        public static bool operator ==(SourceControlProvisioningState left, SourceControlProvisioningState right) => left.Equals(right);
        /// <summary> Determines if two <see cref="SourceControlProvisioningState"/> values are not the same. </summary>
        public static bool operator !=(SourceControlProvisioningState left, SourceControlProvisioningState right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="SourceControlProvisioningState"/>. </summary>
        public static implicit operator SourceControlProvisioningState(string value) => new SourceControlProvisioningState(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is SourceControlProvisioningState other && Equals(other);
        /// <inheritdoc />
        public bool Equals(SourceControlProvisioningState other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
