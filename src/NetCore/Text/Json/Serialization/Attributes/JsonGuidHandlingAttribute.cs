﻿using System.Text.Json.Serialization.Converters.Value;

namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class JsonGuidHandlingAttribute(JsonGuidHandling jsonGuidHandling) : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
        => new Converters.Value.GuidConverter(jsonGuidHandling);
}
