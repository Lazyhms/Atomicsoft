namespace System.Text.Json.Serialization;

internal class NullableConverter<T>(JsonConverter<T> elementConverter) : JsonConverter<T?> where T : struct
{
    public override bool HandleNull => true;

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonTokenType.String == reader.TokenType && reader.GetString() is { Length: 0 }
            ? default : (T?)elementConverter.Read(ref reader, typeToConvert, options);

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            elementConverter.Write(writer, value.Value, options);
        }
    }
}
