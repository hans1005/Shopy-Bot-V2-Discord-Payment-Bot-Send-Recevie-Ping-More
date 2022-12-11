using System.Text.RegularExpressions;

namespace CardsAgainstDiscord.Extensions;

public static class StringExtensions
{
    public static string FormatBlackCard(this string original)
    {
        return new Regex("_+")
            .Replace(original, "`________`")
            .Replace("\\n", "\n")
            .ReplaceLineEndings();
    }

    public static string SafeSubstring(this string original, int start, int length)
    {
        return start >= original.Length
            ? string.Empty
            : original.Substring(start, Math.Min(start + length, original.Length - start));
    }
}