using System.Text.RegularExpressions;

namespace System;

public static partial class StringExtensions
{
    [GeneratedRegex("[A-Z][a-z]*")]
    private static partial Regex RegexCapitalLetters();

    public static bool IsNullOrEmpty(this string? source)
        => string.IsNullOrEmpty(source);

    public static string? IsNullOrEmpty(this string? source, string defaultValue)
        => source.IsNullOrEmpty() ? defaultValue : source;

    public static bool IsNotNullOrEmpty(this string? source)
        => !source.IsNullOrEmpty();

    public static bool IsNullOrWhiteSpace(this string? source)
        => string.IsNullOrWhiteSpace(source);

    public static string? IsNullOrWhiteSpace(this string? source, string defaultValue)
        => source.IsNullOrWhiteSpace() ? defaultValue : source;

    public static bool IsNotNullOrWhiteSpace(this string? source)
        => !source.IsNullOrWhiteSpace();

    public static string[] SplitCapitalLetters(this string? source)
        => source.IsNullOrWhiteSpace() ? [] : [.. RegexCapitalLetters().Matches(source!).Select(s => s.Value)];
}
