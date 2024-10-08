// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.SecurityInsights.Models
{
    /// <summary> The ConditionType. </summary>
    internal readonly partial struct ConditionType : IEquatable<ConditionType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ConditionType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ConditionType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string PropertyValue = "Property";
        private const string PropertyArrayValue = "PropertyArray";
        private const string PropertyChangedValue = "PropertyChanged";
        private const string PropertyArrayChangedValue = "PropertyArrayChanged";
        private const string BooleanValue = "Boolean";

        /// <summary> Evaluate an object property value. </summary>
        public static ConditionType Property { get; } = new ConditionType(PropertyValue);
        /// <summary> Evaluate an object array property value. </summary>
        public static ConditionType PropertyArray { get; } = new ConditionType(PropertyArrayValue);
        /// <summary> Evaluate an object property changed value. </summary>
        public static ConditionType PropertyChanged { get; } = new ConditionType(PropertyChangedValue);
        /// <summary> Evaluate an object array property changed value. </summary>
        public static ConditionType PropertyArrayChanged { get; } = new ConditionType(PropertyArrayChangedValue);
        /// <summary> Apply a boolean operator (e.g AND, OR) to conditions. </summary>
        public static ConditionType Boolean { get; } = new ConditionType(BooleanValue);
        /// <summary> Determines if two <see cref="ConditionType"/> values are the same. </summary>
        public static bool operator ==(ConditionType left, ConditionType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ConditionType"/> values are not the same. </summary>
        public static bool operator !=(ConditionType left, ConditionType right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="ConditionType"/>. </summary>
        public static implicit operator ConditionType(string value) => new ConditionType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ConditionType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ConditionType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
