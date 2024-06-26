// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.BillingBenefits
{
    public partial class BillingBenefitsSavingsPlanOrderResource : IJsonModel<BillingBenefitsSavingsPlanOrderData>
    {
        void IJsonModel<BillingBenefitsSavingsPlanOrderData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<BillingBenefitsSavingsPlanOrderData>)Data).Write(writer, options);

        BillingBenefitsSavingsPlanOrderData IJsonModel<BillingBenefitsSavingsPlanOrderData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<BillingBenefitsSavingsPlanOrderData>)Data).Create(ref reader, options);

        BinaryData IPersistableModel<BillingBenefitsSavingsPlanOrderData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write(Data, options);

        BillingBenefitsSavingsPlanOrderData IPersistableModel<BillingBenefitsSavingsPlanOrderData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<BillingBenefitsSavingsPlanOrderData>(data, options);

        string IPersistableModel<BillingBenefitsSavingsPlanOrderData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<BillingBenefitsSavingsPlanOrderData>)Data).GetFormatFromOptions(options);
    }
}
