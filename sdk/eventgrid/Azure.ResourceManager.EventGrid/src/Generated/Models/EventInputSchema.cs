// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.EventGrid.Models
{
    /// <summary> This determines the format that is expected for incoming events published to the topic. </summary>
    public readonly partial struct EventInputSchema : IEquatable<EventInputSchema>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="EventInputSchema"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public EventInputSchema(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string CloudEventSchemaV10Value = "CloudEventSchemaV1_0";

        /// <summary> CloudEventSchemaV1_0. </summary>
        public static EventInputSchema CloudEventSchemaV10 { get; } = new EventInputSchema(CloudEventSchemaV10Value);
        /// <summary> Determines if two <see cref="EventInputSchema"/> values are the same. </summary>
        public static bool operator ==(EventInputSchema left, EventInputSchema right) => left.Equals(right);
        /// <summary> Determines if two <see cref="EventInputSchema"/> values are not the same. </summary>
        public static bool operator !=(EventInputSchema left, EventInputSchema right) => !left.Equals(right);
        /// <summary> Converts a <see cref="string"/> to a <see cref="EventInputSchema"/>. </summary>
        public static implicit operator EventInputSchema(string value) => new EventInputSchema(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is EventInputSchema other && Equals(other);
        /// <inheritdoc />
        public bool Equals(EventInputSchema other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
