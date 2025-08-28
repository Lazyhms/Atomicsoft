
namespace System.Text.RegularExpressions;

public partial class GeneratedRegex
{
    public static readonly Regex Url = UrlRegex();

    public static readonly Regex Email = EmailRegex();

    public static readonly Regex Phone = PhoneRegex();

    public static readonly Regex IdCard = IdCardRegex();

    public static readonly Regex Numeric = NumericRegex();

    public static readonly Regex DecimalNumber = DecimalNumberRegex();

    public static readonly Regex Alphabetic = AlphabeticRegex();

    public static readonly Regex Alphanumeric = AlphanumericRegex();

    public static readonly Regex Hexadecimal = HexadecimalRegex();

    public static readonly Regex Guid = GuidRegex();

    public static readonly Regex IPv4 = IPv4Regex();

    public static readonly Regex IPv6 = IPv6Regex();

    public static readonly Regex Base64 = Base64Regex();

    public static readonly Regex StrongPassword = StrongPasswordRegex();

    public static readonly Regex ChineseCharacters = ChineseCharactersRegex();

    [GeneratedRegex(@"^(https?|ftp|file)://[^\s/$.?#].[^\s]*$", RegexOptions.Compiled)]
    private static partial Regex UrlRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^(?:(?:\+|00)86)?1[3-9]\d{9}$", RegexOptions.Compiled)]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}(\d|X|x)$")]
    private static partial Regex IdCardRegex();

    [GeneratedRegex(@"^\d+$", RegexOptions.Compiled)]
    private static partial Regex NumericRegex();

    [GeneratedRegex(@"^[+-]?(\d+(\.\d+)?|\.\d+)$", RegexOptions.Compiled)]
    private static partial Regex DecimalNumberRegex();

    [GeneratedRegex(@"^[a-zA-Z]+$", RegexOptions.Compiled)]
    private static partial Regex AlphabeticRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled)]
    private static partial Regex AlphanumericRegex();

    [GeneratedRegex(@"^[0-9a-fA-F]+$", RegexOptions.Compiled)]
    private static partial Regex HexadecimalRegex();

    [GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled)]
    private static partial Regex GuidRegex();

    [GeneratedRegex(@"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$", RegexOptions.Compiled)]
    private static partial Regex IPv4Regex();

    [GeneratedRegex(@"^(([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]|)[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]|)[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]|)[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]|)[0-9]))$", RegexOptions.Compiled)]
    private static partial Regex IPv6Regex();

    [GeneratedRegex(@"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$", RegexOptions.Compiled)]
    private static partial Regex Base64Regex();

    [GeneratedRegex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$")]
    private static partial Regex StrongPasswordRegex();

    [GeneratedRegex(@"[\u4e00-\u9fa5]", RegexOptions.Compiled)]
    private static partial Regex ChineseCharactersRegex();

}
