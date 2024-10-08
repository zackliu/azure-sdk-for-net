// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.Media.VideoAnalyzer.Edge.Models
{
    /// <summary> The MPEG4 Profile. </summary>
    public readonly partial struct Mpeg4Profile : IEquatable<Mpeg4Profile>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="Mpeg4Profile"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public Mpeg4Profile(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string SPValue = "SP";
        private const string ASPValue = "ASP";

        /// <summary> Simple Profile. </summary>
        public static Mpeg4Profile SP { get; } = new Mpeg4Profile(SPValue);
        /// <summary> Advanced Simple Profile. </summary>
        public static Mpeg4Profile ASP { get; } = new Mpeg4Profile(ASPValue);
        /// <summary> Determines if two <see cref="Mpeg4Profile"/> values are the same. </summary>
        public static bool operator ==(Mpeg4Profile left, Mpeg4Profile right) => left.Equals(right);
        /// <summary> Determines if two <see cref="Mpeg4Profile"/> values are not the same. </summary>
        public static bool operator !=(Mpeg4Profile left, Mpeg4Profile right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="Mpeg4Profile"/>. </summary>
        public static implicit operator Mpeg4Profile(string value) => new Mpeg4Profile(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is Mpeg4Profile other && Equals(other);
        /// <inheritdoc />
        public bool Equals(Mpeg4Profile other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
