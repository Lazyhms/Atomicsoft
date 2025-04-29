namespace System.Text.Json.Nodes;

public static class JsonNodeExtensions
{
    public static T? GetPropertyValue<T>(this JsonObject jsonObject, string propertyName)
    {
        var jsonNode = jsonObject.GetPropertyValue(propertyName);
        return jsonNode is null ? default : jsonNode!.GetValue<T>();
    }

    public static JsonNode? GetPropertyValue(this JsonObject jsonObject, string propertyName)
        => jsonObject.FirstOrDefault(fod => fod.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value;

    public static JsonObject TryUpdate(this JsonObject jsonObject, string propertyName, JsonNode? value)
    {
        var jsonNode = jsonObject.GetPropertyValue(propertyName);
        jsonNode?.ReplaceWith(value);
        return jsonObject;
    }

    public static JsonObject AddOrUpdate(this JsonObject jsonObject, string propertyName, JsonNode? value)
    {
        var jsonNode = jsonObject.GetPropertyValue(propertyName);
        switch (jsonNode)
        {
            case not null:
                jsonNode.ReplaceWith(value);
                break;
            default:
                jsonObject.Add(propertyName, value);
                break;
        }
        return jsonObject;
    }
}
