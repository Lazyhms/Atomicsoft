using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json.Serialization;

internal static class JsonTypeInfoResolver
{
    public static void AddEnumModifier(JsonTypeInfo jsonTypeInfo)
    {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
        {
            return;
        }

        foreach (var jsonPropertyInfo in jsonTypeInfo.Properties.Where(w => w.PropertyType.IsEnum).ToList())
        {
            var newJsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(string), $"{jsonPropertyInfo.Name}Name");
            newJsonPropertyInfo.Get = (obj) =>
            {
                var result = jsonPropertyInfo.Get?.Invoke(obj);
                var fieldName = result == null ? string.Empty : Enum.GetName(jsonPropertyInfo.PropertyType, result);
                var field = jsonPropertyInfo.PropertyType.GetField(fieldName ?? string.Empty, BindingFlags.Public | BindingFlags.Static);
                return field?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
            };
            jsonTypeInfo.Properties.Add(newJsonPropertyInfo);
        }
    }

    public static void JsonPropertyModifier(JsonTypeInfo jsonTypeInfo)
    {
        if (JsonTypeInfoKind.Object != jsonTypeInfo.Kind)
        {
            return;
        }

        foreach (var jsonPropertyInfo in jsonTypeInfo.Properties.Where(w => w.AttributeProvider?.IsDefined(typeof(JsonPropertyNamesAttribute), false) != null).ToList())
        {
            var jsonPropertyNamesAttribute = jsonPropertyInfo.AttributeProvider?.GetCustomAttributes(typeof(JsonPropertyNamesAttribute), false).OfType<JsonPropertyNamesAttribute>().FirstOrDefault();
            if (jsonPropertyNamesAttribute?.Names is { Length: > 0 })
            {
                foreach (var name in jsonPropertyNamesAttribute.Names!)
                {
                    var newPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(jsonPropertyInfo.PropertyType, name);

                    newPropertyInfo.Set = jsonPropertyInfo.Set;
                    //newPropertyInfo.Get = jsonPropertyInfo.Get;

                    jsonTypeInfo.Properties.Add(newPropertyInfo);
                }
            }
        }
    }
}
