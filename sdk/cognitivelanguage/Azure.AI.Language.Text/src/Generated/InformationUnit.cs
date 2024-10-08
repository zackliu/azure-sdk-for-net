// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.AI.Language.Text
{
    /// <summary> The information (data) Unit of measurement. </summary>
    public readonly partial struct InformationUnit : IEquatable<InformationUnit>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="InformationUnit"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public InformationUnit(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string UnspecifiedValue = "Unspecified";
        private const string BitValue = "Bit";
        private const string KilobitValue = "Kilobit";
        private const string MegabitValue = "Megabit";
        private const string GigabitValue = "Gigabit";
        private const string TerabitValue = "Terabit";
        private const string PetabitValue = "Petabit";
        private const string ByteValue = "Byte";
        private const string KilobyteValue = "Kilobyte";
        private const string MegabyteValue = "Megabyte";
        private const string GigabyteValue = "Gigabyte";
        private const string TerabyteValue = "Terabyte";
        private const string PetabyteValue = "Petabyte";

        /// <summary> Unspecified data size unit. </summary>
        public static InformationUnit Unspecified { get; } = new InformationUnit(UnspecifiedValue);
        /// <summary> Data size unit in bits. </summary>
        public static InformationUnit Bit { get; } = new InformationUnit(BitValue);
        /// <summary> Data size unit in kilobits. </summary>
        public static InformationUnit Kilobit { get; } = new InformationUnit(KilobitValue);
        /// <summary> Data size unit in megabits. </summary>
        public static InformationUnit Megabit { get; } = new InformationUnit(MegabitValue);
        /// <summary> Data size unit in gigabits. </summary>
        public static InformationUnit Gigabit { get; } = new InformationUnit(GigabitValue);
        /// <summary> Data size unit in terabits. </summary>
        public static InformationUnit Terabit { get; } = new InformationUnit(TerabitValue);
        /// <summary> Data size unit in petabits. </summary>
        public static InformationUnit Petabit { get; } = new InformationUnit(PetabitValue);
        /// <summary> Data size unit in bytes. </summary>
        public static InformationUnit Byte { get; } = new InformationUnit(ByteValue);
        /// <summary> Data size unit in kilobytes. </summary>
        public static InformationUnit Kilobyte { get; } = new InformationUnit(KilobyteValue);
        /// <summary> Data size unit in megabytes. </summary>
        public static InformationUnit Megabyte { get; } = new InformationUnit(MegabyteValue);
        /// <summary> Data size unit in gigabytes. </summary>
        public static InformationUnit Gigabyte { get; } = new InformationUnit(GigabyteValue);
        /// <summary> Data size unit in terabytes. </summary>
        public static InformationUnit Terabyte { get; } = new InformationUnit(TerabyteValue);
        /// <summary> Data size unit in petabytes. </summary>
        public static InformationUnit Petabyte { get; } = new InformationUnit(PetabyteValue);
        /// <summary> Determines if two <see cref="InformationUnit"/> values are the same. </summary>
        public static bool operator ==(InformationUnit left, InformationUnit right) => left.Equals(right);
        /// <summary> Determines if two <see cref="InformationUnit"/> values are not the same. </summary>
        public static bool operator !=(InformationUnit left, InformationUnit right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="InformationUnit"/>. </summary>
        public static implicit operator InformationUnit(string value) => new InformationUnit(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is InformationUnit other && Equals(other);
        /// <inheritdoc />
        public bool Equals(InformationUnit other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
