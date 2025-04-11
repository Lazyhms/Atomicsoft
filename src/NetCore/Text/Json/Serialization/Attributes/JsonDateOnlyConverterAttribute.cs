namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class JsonDateOnlyConverterAttribute(string dateFormatString) : JsonConverterAttribute
{
    public JsonDateOnlyConverterAttribute() : this("yyyy-MM-dd HH:mm:ss")
    {
    }

    public override JsonConverter? CreateConverter(Type typeToConvert)
        => new JsonDateOnlyConverter(dateFormatString);
}
