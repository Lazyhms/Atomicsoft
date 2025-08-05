using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

using JsonConvertValue = System.Text.Json.Serialization.Converters.Value;
using JsonConvertObject = System.Text.Json.Serialization.Converters.Object;

namespace System.Text.Json;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions ApplyWebDefault(this JsonSerializerOptions serializerOptions)
    {
        if (serializerOptions.IsReadOnly)
        {
            serializerOptions = new JsonSerializerOptions(serializerOptions);
        }

        serializerOptions.WriteIndented = true;
        serializerOptions.AllowTrailingCommas = true;
        serializerOptions.PropertyNameCaseInsensitive = true;
        serializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        serializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        serializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        serializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        serializerOptions.Converters.Add(new NullableConverterFactory());

        serializerOptions.Converters.Add(new JsonConvertObject.DataSetConverter());
        serializerOptions.Converters.Add(new JsonConvertObject.DataTableConverter());

        serializerOptions.Converters.Add(new JsonConvertValue.GuidConverter());
        serializerOptions.Converters.Add(new JsonConvertValue.StringConverter());
        serializerOptions.Converters.Add(new JsonConvertValue.DateOnlyConverter());
        serializerOptions.Converters.Add(new JsonConvertValue.DateTimeConverter());
        serializerOptions.Converters.Add(new JsonConvertValue.DateTimeOffsetConverter());

        serializerOptions.TypeInfoResolver = serializerOptions.TypeInfoResolver?
            .WithAddedModifier(Serialization.JsonTypeInfoResolver.AddEnumModifier)
            .WithAddedModifier(Serialization.JsonTypeInfoResolver.JsonPropertyModifier);

        return serializerOptions;
    }
}
