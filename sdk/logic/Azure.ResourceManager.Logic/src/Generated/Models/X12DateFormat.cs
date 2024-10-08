// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.Logic.Models
{
    /// <summary> The x12 date format. </summary>
    public readonly partial struct X12DateFormat : IEquatable<X12DateFormat>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="X12DateFormat"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public X12DateFormat(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string NotSpecifiedValue = "NotSpecified";
        private const string CcyymmddValue = "CCYYMMDD";
        private const string YymmddValue = "YYMMDD";

        /// <summary> NotSpecified. </summary>
        public static X12DateFormat NotSpecified { get; } = new X12DateFormat(NotSpecifiedValue);
        /// <summary> CCYYMMDD. </summary>
        public static X12DateFormat Ccyymmdd { get; } = new X12DateFormat(CcyymmddValue);
        /// <summary> YYMMDD. </summary>
        public static X12DateFormat Yymmdd { get; } = new X12DateFormat(YymmddValue);
        /// <summary> Determines if two <see cref="X12DateFormat"/> values are the same. </summary>
        public static bool operator ==(X12DateFormat left, X12DateFormat right) => left.Equals(right);
        /// <summary> Determines if two <see cref="X12DateFormat"/> values are not the same. </summary>
        public static bool operator !=(X12DateFormat left, X12DateFormat right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="X12DateFormat"/>. </summary>
        public static implicit operator X12DateFormat(string value) => new X12DateFormat(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is X12DateFormat other && Equals(other);
        /// <inheritdoc />
        public bool Equals(X12DateFormat other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
