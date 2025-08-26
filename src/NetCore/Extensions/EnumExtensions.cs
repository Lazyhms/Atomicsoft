using System.ComponentModel;

namespace System;

public static class EnumExtensions
{
    public static T? GetAttributeOfType<T>(this Enum enumValue) where T : Attribute
    {
        var type = enumValue.GetType();
        var name = Enum.GetName(type, enumValue) ?? string.Empty;
        return type.GetField(name, BindingFlags.Public | BindingFlags.Static)?.GetCustomAttribute<T>(false);
    }

    public static string? GetDescription(this Enum enumValue)
        => enumValue.GetAttributeOfType<DescriptionAttribute>()?.Description;
}
