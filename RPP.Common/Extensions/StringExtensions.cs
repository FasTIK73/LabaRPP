using System.Text.RegularExpressions;

namespace RPP.Common.Extensions;

public static class StringExtensions
{
    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static bool IsGuid(this string str)
    {
        return Guid.TryParse(str, out _);
    }

    public static bool IsPhoneNumber(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return false;
        return Regex.IsMatch(str, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");
    }

    public static bool IsEmail(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return false;
        return Regex.IsMatch(str, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}