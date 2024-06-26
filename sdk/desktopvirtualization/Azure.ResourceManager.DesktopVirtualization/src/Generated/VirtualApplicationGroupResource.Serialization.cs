// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Text.Json;

namespace Azure.ResourceManager.DesktopVirtualization
{
    public partial class VirtualApplicationGroupResource : IJsonModel<VirtualApplicationGroupData>
    {
        void IJsonModel<VirtualApplicationGroupData>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options) => ((IJsonModel<VirtualApplicationGroupData>)Data).Write(writer, options);

        VirtualApplicationGroupData IJsonModel<VirtualApplicationGroupData>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options) => ((IJsonModel<VirtualApplicationGroupData>)Data).Create(ref reader, options);

        BinaryData IPersistableModel<VirtualApplicationGroupData>.Write(ModelReaderWriterOptions options) => ModelReaderWriter.Write(Data, options);

        VirtualApplicationGroupData IPersistableModel<VirtualApplicationGroupData>.Create(BinaryData data, ModelReaderWriterOptions options) => ModelReaderWriter.Read<VirtualApplicationGroupData>(data, options);

        string IPersistableModel<VirtualApplicationGroupData>.GetFormatFromOptions(ModelReaderWriterOptions options) => ((IPersistableModel<VirtualApplicationGroupData>)Data).GetFormatFromOptions(options);
    }
}
