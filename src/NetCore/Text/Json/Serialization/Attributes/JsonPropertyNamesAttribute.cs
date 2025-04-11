namespace System.Text.Json.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class JsonPropertyNamesAttribute(params string[] names) : JsonAttribute
{
    public string[] Names { get; } = names;
}
