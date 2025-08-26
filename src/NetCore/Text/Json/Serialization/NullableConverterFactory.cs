namespace System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
public class NullableConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsNullableValueType();

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert.IsNullableValueType());

        var valueTypeToConvert = typeToConvert.GetGenericArguments()[0];
        var valueConverter = options.GetConverter(valueTypeToConvert);

        return !valueConverter.Type!.IsValueType && valueTypeToConvert.IsValueType
            ? valueConverter : CreateValueConverter(valueTypeToConvert, valueConverter);
    }

    public static JsonConverter CreateValueConverter(Type valueTypeToConvert, JsonConverter valueConverter)
    {
        Debug.Assert(valueTypeToConvert.IsNullableValueType());

        return (JsonConverter)Activator.CreateInstance(GetNullableConverterType(valueTypeToConvert),
            BindingFlags.Instance | BindingFlags.Public, binder: null, args: [valueConverter], culture: null)!;
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2071:UnrecognizedReflectionPattern",
            Justification = "'NullableConverter<T> where T : struct' implies 'T : new()', so the trimmer is warning calling MakeGenericType here because valueTypeToConvert's constructors are not annotated. " +
            "But NullableConverter doesn't call new T(), so this is safe.")]
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    private static Type GetNullableConverterType(Type valueTypeToConvert) => typeof(NullableConverter<>).MakeGenericType(valueTypeToConvert);
}
