using System.Text;
using System.Text.RegularExpressions;

namespace AiConsole.Utils.ResponseVoice;

public static class SpeechTextSanitizer
{
    public static string RemoveEmojis(string text)
    {
        var result = new StringBuilder();

        foreach (var rune in text.EnumerateRunes())
        {
            var value = rune.Value;

            if (IsEmoji(value) ||
                value is 0x200D or 0xFE0F or 0xFE0E or 0x20E3)
            {
                result.Append(' ');
                continue;
            }

            result.Append(rune.ToString());
        }

        return Regex.Replace(result.ToString(), @"\s+", " ").Trim();
    }

    private static bool IsEmoji(int value)
    {
        return value is 0x00A9 or 0x00AE or 0x203C or
            0x2049 or 0x2122 or 0x2139
            || value is >= 0x1F000 and <= 0x1FAFF
            || value is >= 0x2600 and <= 0x27BF
            || value is >= 0x2300 and <= 0x23FF
            || value is >= 0x2B00 and <= 0x2BFF
            || value is >= 0x2194 and <= 0x21AA;
    }
}
