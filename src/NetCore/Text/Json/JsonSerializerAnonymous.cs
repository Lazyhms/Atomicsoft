namespace System.Text.Json;

public static partial class JsonSerializerAnonymous
{
    public static T Deserialize<T>(string json, T _, JsonSerializerOptions? options = null) where T : notnull
        => JsonSerializer.Deserialize<T>(json, options)!;
}
