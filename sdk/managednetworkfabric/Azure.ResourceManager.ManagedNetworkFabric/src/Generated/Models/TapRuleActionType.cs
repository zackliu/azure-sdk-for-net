// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.ManagedNetworkFabric.Models
{
    /// <summary> Type of actions that can be performed. </summary>
    public readonly partial struct TapRuleActionType : IEquatable<TapRuleActionType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="TapRuleActionType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public TapRuleActionType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DropValue = "Drop";
        private const string CountValue = "Count";
        private const string LogValue = "Log";
        private const string ReplicateValue = "Replicate";
        private const string GotoValue = "Goto";
        private const string RedirectValue = "Redirect";
        private const string MirrorValue = "Mirror";

        /// <summary> Drop. </summary>
        public static TapRuleActionType Drop { get; } = new TapRuleActionType(DropValue);
        /// <summary> Count. </summary>
        public static TapRuleActionType Count { get; } = new TapRuleActionType(CountValue);
        /// <summary> Log. </summary>
        public static TapRuleActionType Log { get; } = new TapRuleActionType(LogValue);
        /// <summary> Replicate. </summary>
        public static TapRuleActionType Replicate { get; } = new TapRuleActionType(ReplicateValue);
        /// <summary> Goto. </summary>
        public static TapRuleActionType Goto { get; } = new TapRuleActionType(GotoValue);
        /// <summary> Redirect. </summary>
        public static TapRuleActionType Redirect { get; } = new TapRuleActionType(RedirectValue);
        /// <summary> Mirror. </summary>
        public static TapRuleActionType Mirror { get; } = new TapRuleActionType(MirrorValue);
        /// <summary> Determines if two <see cref="TapRuleActionType"/> values are the same. </summary>
        public static bool operator ==(TapRuleActionType left, TapRuleActionType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="TapRuleActionType"/> values are not the same. </summary>
        public static bool operator !=(TapRuleActionType left, TapRuleActionType right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="TapRuleActionType"/>. </summary>
        public static implicit operator TapRuleActionType(string value) => new TapRuleActionType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is TapRuleActionType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(TapRuleActionType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
