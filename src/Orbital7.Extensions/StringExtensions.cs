using System.Text.RegularExpressions;

namespace Orbital7.Extensions;

public static class StringExtensions
{
    public const string PunctuationDash = "-";
    public const string PunctuationUnderscore = "_";
    public const string PunctuationCharsBase = ".,()[]{}|\\';:!@#$&%?/<>–^+=*\"";
    public const string PunctuationChars = PunctuationCharsBase + PunctuationDash + PunctuationUnderscore;
    public const string NumberChars = "0123456789";
    public const string LowercaseLetterChars = "abcdefghijklmnopqrstuvwxyz";
    public const string CapitalLetterChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LetterChars = CapitalLetterChars + LowercaseLetterChars;
    public const string AlphanumericChars = NumberChars + LetterChars;
    public const string WhitespaceChars = " \r\n\t\v\f";

    public static T? ToTypedValue<T>(
        this string? value)
    {
        if (!value.HasText())
            return default;
        
        var converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter == null || !converter.CanConvertFrom(typeof(string)))
            throw new InvalidOperationException($"Cannot convert from string to {typeof(T).Name}.");

        return (T?)converter.ConvertFromInvariantString(value);
    }

    public static string? ShuffleCharacters(
        this string? value)
    {
        if (value != null)
        {
            var chars = value.ToCharArray();

            var randomlyOrderedChars = chars.Randomize();

            var sb = new StringBuilder();
            sb.Append(randomlyOrderedChars);
            return sb.ToString();
        }

        return null;
    }

    public static string? First(
        this string? value, 
        int numCharacters)
    {
        if (value != null && value.Length >= numCharacters)
        {
            return value.Substring(0, numCharacters);
        }

        return null;
    }

    public static string? Last(
        this string? value, 
        int numCharacters)
    {
        if (value != null && value.Length >= numCharacters)
        {
            return value.Substring(value.Length - numCharacters, numCharacters);
        }

        return null;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? PascalCaseToPhrase(
         this string? value)
    {
        if (value != null)
        {
            return Regex.Replace(value, "([A-Z])", " $1").Trim();
        }

        return null;
    }

    public static string? Pluralize(
        this string? value)
    {
        if (value != null)
        {
            var plural = value;

            if (value.EndsWith("y"))
                plural = value.PruneEnd(1) + "ies";
            else if (value.EndsWith("is"))
                plural = value.PruneEnd(2) + "es";
            else if (value.EndsWith("s") || value.EndsWith("ch"))
                plural = value + "es";
            else
                plural = value + "s";

            return plural;
        }

        return null;
    }

    public static string? PluralizeIf(
        this string? value,
        bool condition)
    {
        if (condition)
        {
            return value.Pluralize();
        }

        return value;
    }

    public static string? PluralizeIf(
        this string? value,
        int count)
    {
        return value.PluralizeIf(count != 1);
    }

    public static string? Remove(
        this string? value, 
        string toRemove)
    {
        return value?.Replace(toRemove, "");
    }

    public static string? Replace(
        this string? text, 
        IDictionary<string, string>? textReplacementKeys)
    {
        if (text != null)
        {
            string value = text;

            if (textReplacementKeys != null)
                foreach (var textReplacementKey in textReplacementKeys)
                    value = value.Replace(textReplacementKey.Key, textReplacementKey.Value);

            return value;
        }

        return null;
    }

    public static string? Replace(
        this string? text, 
        List<(string, string)>? textReplacementKeys)
    {
        if (text != null)
        {
            string value = text;

            if (textReplacementKeys != null)
                foreach (var textReplacementKey in textReplacementKeys)
                    value = value.Replace(textReplacementKey.Item1, textReplacementKey.Item2);

            return value;
        }

        return null;
    }

    public static string? UrlEncode(
        this string? value)
    {
        return System.Web.HttpUtility.UrlEncode(value);
    }

    public static string? ToTitleCase(
        this string? value)
    {
        if (value != null)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(value.ToLower());
        }

        return null;
    }

    public static string GetHash(
        this string value)
    {
        var provider = System.Security.Cryptography.MD5.Create();
        byte[] data = Encoding.ASCII.GetBytes(value);
        data = provider.ComputeHash(data);

        string ret = "";
        for (int i = 0; i < data.Length; i++)
            ret += data[i].ToString("x2").ToLower();

        return ret;
    }

    public static bool IsWindowsFileSystemSafe(
        this string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();

        foreach (var c in invalidChars)
            if (value.Contains(c.ToString()))
                return false;

        return true;
    }

    public static string EnsureWindowsFileSystemSafe(
        this string value, 
        string replacementChar = "")
    {
        var windowsSafeValue = value;
        var invalidChars = Path.GetInvalidFileNameChars();

        foreach (var c in invalidChars)
            windowsSafeValue = windowsSafeValue.Replace(c.ToString(), replacementChar);

        return windowsSafeValue;
    }

    public static MemoryStream ToStream(
        this string value)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(value));
    }

    public static string? EnsureStartsWith(
        this string? value,
        string startsWith)
    {
        if (value != null)
        {
            if (!value.StartsWith(startsWith))
                return startsWith + value;
            else
                return value;
        }

        return null;
    }

    public static string? EnsureEndsWith(
        this string? value,
        string endsWith)
    {
        if (value != null)
        {
            if (!value.EndsWith(endsWith))
                return value + endsWith;
            else
                return value;
        }

        return null;
    }

    public static string EnsureHasText(
        this string? value, 
        string defaultText)
    {
        if (value.HasText())
            return value;
        else
            return defaultText;
    }

    public static string? EnsureNullIfEmpty(
        this string? value)
    {
        if (value.HasText())
            return value;
        else
            return null;
    }

    public static string? DecodeToString(
        this byte[]? bytes)
    {
        if (bytes != null && bytes.Length > 0)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        return null;
    }

    public static byte[]? EncodeToByteArray(
        this string? value)
    {
        if (value != null && value.Length > 0)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(value);
        }

        return null;
    }

    public static string? StripInvalidXMLCharacters(
        this string? value)
    {
        if (value != null)
        {
            StringBuilder textOut = new StringBuilder(); // Used to hold the output.   
            char current; // Used to reference the current character.   

            if (value == null || value == string.Empty) return string.Empty; // vacancy test.   
            for (int i = 0; i < value.Length; i++)
            {
                current = value[i];

                if ((current == 0x9 || current == 0xA || current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)))// ||   
                //((current >= 0x10000) && (current <= 0x10FFFF)))   
                {
                    textOut.Append(current);
                }
            }

            return textOut.ToString();
        }

        return null;
    }

    public static string EnsureCapitalized(
        this string value)
    {
        var firstLetter = value.Substring(0, 1);
        return firstLetter.ToUpper() + value.PruneStart(1);
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToSeparatedWords(
        this string value)
    {
        if (value != null)
        {
            return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
        }
        return null;
    }

    public static string EnsureNonEmptyHTMLContent(
        this string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            return "&nbsp;";
        else
            return value;
    }

    public static string Mask(
        this string value,
        string maskChar = "*")
    {
        if (!string.IsNullOrEmpty(value))
            return new string(maskChar.ToCharArray()[0], value.Length);
        else
            return value;
    }

    public static string MaskNumber(
        this string number, 
        int endingCount,
        string maskChar = "*")
    {
        string maskedNumber = string.Empty;

        string endingNumber = number.Substring(number.Length - endingCount, endingCount);
        string leftSide = number.Substring(0, number.Length - endingCount);
        foreach (char c in leftSide.ToCharArray())
        {
            if (c.Equals(' ') || c.Equals('-'))
                maskedNumber += c;
            else
                maskedNumber += maskChar;
        }

        return maskedNumber + endingNumber;
    }

    public static string EnsureMaxStringLength(
        this string input, 
        int maxLength, 
        string? truncationSuffix = null)
    {
        if (!string.IsNullOrEmpty(input) && input.Length > maxLength)
        {
            int actualMaxLength = maxLength;
            if (!string.IsNullOrEmpty(truncationSuffix))
                actualMaxLength -= truncationSuffix.Length;

            return input.Substring(0, actualMaxLength) + truncationSuffix;
        }

        return input;
    }

    public static bool ContainsChars(
        this string value, 
        string chars)
    {
        return ContainsChars(value, chars.ToCharArray());
    }

    public static bool ContainsChars(
        this string value, 
        char[] chars)
    {
        bool contains = false;

        foreach (char c in chars)
        {
            string s = c.ToString();
            if (value.Contains(s))
            {
                contains = true;
                break;
            }
        }

        return contains;
    }

    public static bool HasText(
        [NotNullWhen(true)] this string? value)
    {
        return (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value.Trim()));
    }

    public static string NumbersOnly(
        this string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in value.ToCharArray())
            {
                string s = c.ToString();
                if (NumberChars.Contains(s)) sb.Append(s);
            }

            return sb.ToString();
        }
        else
        {
            return value;
        }
    }

    public static bool IsNumbers(
        this string? value)
    {
        if (value.HasText())
        {
            foreach (char c in value.ToCharArray())
                if (!NumberChars.Contains(c.ToString()))
                    return false;

            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsLetters(
        this string? value)
    {
        if (value.HasText())
        {
            foreach (char c in value.ToCharArray())
                if (!LetterChars.Contains(c.ToString()))
                    return false;

            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsAlphanumeric(
        this string? value)
    {
        if (value.HasText())
        {
            foreach (char c in value.ToCharArray())
                if (!AlphanumericChars.Contains(c.ToString()))
                    return false;

            return true;
        }
        else
        {
            return false;
        }
    }

    public static string[] Parse(
        this string? input, 
        bool byWhitespace, 
        bool byPunctuation, 
        bool includeDash, 
        bool includeUnderscore, 
        bool includeDelimitersInResults, 
        bool removeEmptyEntries)
    {
        string[]? result = null;

        if (input.HasText())
        {
            string chars = string.Empty;
            if (byWhitespace) chars += WhitespaceChars;
            if (byPunctuation) chars += GetPuncuationChars(includeDash, includeUnderscore);

            if (includeDelimitersInResults)
            {
                var delimiters = chars.ToStringArray();
                if (delimiters.Length > 0)
                {
                    string pattern = "(" + string.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) + ")";
                    result = Regex.Split(input, pattern);
                }
            }
            else
            {
                result = input.Split(chars.ToCharArray(), GetStringSplitOptions(removeEmptyEntries));
            }
        }

        // Verify.
        if (result == null || result.Length == 0)
        {
            if (input != null)
            {
                result = [input];
            }
            else
            {
                result = [];
            }
        }

        return result;
    }

    public static string[] ParseLines(
        this string value, 
        bool removeEmptyEntries = true)
    {
        return value.NormalizeLineTerminators("\n").Split("\n".ToCharArray(), GetStringSplitOptions(removeEmptyEntries));
    }

    public static string[] Parse(
        this string? value, 
        string delimiter, 
        bool removeEmptyEntries = true)
    {
        if (value != null)
        {
            return value.Split(
                [delimiter],
                GetStringSplitOptions(removeEmptyEntries));
        }
        else
        {
            return [];
        }
    }

    public static List<Guid> ParseGuids(
        this string? value, 
        string delimiter, 
        bool allowDuplicates, 
        bool allowEmptyGuid)
    {
        var items = new List<Guid>();

        if (value != null)
        {
            items = (from x in value.Parse(delimiter)
                     let a = Guid.Parse(x)
                     where allowEmptyGuid || a != Guid.Empty
                     select a).ToList();
            if (!allowDuplicates)
                items = items.Distinct().ToList();
        }

        return items;
    }

    public static List<string> ParseBetween(
        this string value, 
        string start, 
        string end)
    {
        return ParseBetween(value, start, end, true);
    }

    public static List<string> ParseBetween(
        this string? value, 
        string start, 
        string end, 
        bool removeEmptyEntries)
    {
        List<string> output = [];

        if (value != null)
        {
            int startLength = start.Length;
            int endLength = end.Length;

            // Find the first index.
            int startIndex = value.IndexOf(start);
            while (startIndex >= 0)
            {
                int endIndex = value.IndexOf(end, startIndex + startLength + 1);
                if (endIndex >= 0)
                {
                    // Add the item.
                    int itemIndex = startIndex + startLength;
                    string item = value.Substring(itemIndex, endIndex - itemIndex);
                    if (!removeEmptyEntries || !string.IsNullOrEmpty(item))
                        output.Add(item);

                    // Find the start of the next item.
                    startIndex = value.IndexOf(start, endIndex + endLength);
                }
                else
                {
                    break;
                }
            }
        }

        return output;
    }

    public static string GetSingleLine(
        this string value)
    {
        string output = value;

        // Parse by return and newline characters.
        string separator = "\r\n";
        string[] lines = value.Split(separator.ToCharArray(), StringSplitOptions.None);

        // Return only the first line.
        if (lines.Length > 0) output = lines[0];

        return output;
    }

    public static string FindFirstBetween(
        this string? value, 
        string start, 
        string end)
    {
        return FindFirstBetween(value, start, end, true);
    }

    public static string FindFirstBetween(
        this string? value, 
        string start, 
        string end, 
        bool autoTrim)
    {
        string output = string.Empty;

        if (value != null)
        {
            // Determine lengths.
            int startLength = start.Length;
            int endLength = end.Length;

            // Find the first index.
            int startIndex = value.IndexOf(start);
            if (startIndex >= 0)
            {
                int endIndex = value.IndexOf(end, startIndex + startLength + 1);
                if (endIndex >= 0)
                {
                    // Add the item.
                    int itemIndex = startIndex + startLength;
                    output = value.Substring(itemIndex, endIndex - itemIndex);
                }
            }

            // Trim as necessary.
            if (autoTrim)
            {
                output = output.Trim();
            }
        }

        return output;
    }

    public static string[] ToStringArray(
        this string input)
    {
        string[] chars = new string[input.Length];

        for (int i = 0; i < input.Length; i++)
            chars[i] = input.Substring(i, 1);

        return chars;
    }

    public static string NormalizeLineTerminators(
        this string value, 
        string lineTerm = "\n")
    {
        return value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", lineTerm);
    }

    public static string FormatLineTerminatorsAsHtml(
        this string value)
    {
        return value.NormalizeLineTerminators().Replace("\n", "<br />");
    }

    public static string GetLineTerminator(
        this string value)
    {
        string term = string.Empty;

        if (value.Contains("\r\n"))
            term = "\r\n";
        else if (value.Contains("\r"))
            term = "\r";
        else
            term = "\n";

        return term;
    }

    public static string EncloseInQuotes(
        this string? value)
    {
        return "\"" + value + "\"";
    }

    public static string RemoveQuotes(
        this string value)
    {
        return PruneEnd(PruneStart(value, "\""), "\"");
    }

    public static List<string> ToNGrams(
        this string input, 
        int min, 
        int max)
    {
        List<string> nGrams = new List<string>();

        // Setup.
        string inStr = input.Trim();
        int inLen = inStr.Length;
        int gramSize = min;
        int pos = 0;

        // Loop.
        while (true)
        {
            if (pos + gramSize > inLen) // if we hit the end of the string
            {
                pos = 0;              // reset to beginning of string
                gramSize++;           // increase n-gram size
                if (gramSize > max)   // we are done
                    break;
                if (pos + gramSize > inLen)
                    break;
            }
            string gram = inStr.Substring(pos, gramSize);
            nGrams.Add(gram);
            pos++;
        }

        return nGrams;
    }

    public static string ToUrlSlug(
        this string value)
    {
        return value
            .Trim()
            .ToLower()
            .Replace(" ", "-")
            .Replace("@", "")
            .Replace("`", "")
            .Replace("~", "")
            .Replace("!", "")
            .Replace("#", "")
            .Replace("$", "")
            .Replace("%", "")
            .Replace("^", "")
            .Replace("*", "")
            .Replace("+", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("_", "")
            .Replace("&", "")
            .Replace("=", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("|", "")
            .Replace(":", "")
            .Replace(";", "")
            .Replace("'", "")
            .Replace(",", "")
            .Replace(".", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("/", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("\\", "");
    }

    public static string PruneEnd(
        this string value, 
        string end)
    {
        string output = value;

        if (!string.IsNullOrEmpty(value) && value.EndsWith(end))
            output = value.Substring(0, value.Length - end.Length);

        return output;
    }

    public static string PruneEnd(
        this string value, 
        int length)
    {
        string output = value;

        if (!string.IsNullOrEmpty(value))
            output = value.Substring(0, value.Length - length);

        return output;
    }

    public static string PruneStart(
        this string value, 
        string start)
    {
        string output = value;

        if (!string.IsNullOrEmpty(value) && value.StartsWith(start, StringComparison.OrdinalIgnoreCase))
            output = value.Substring(start.Length, value.Length - start.Length);

        return output;
    }

    public static string PruneStart(
        this string value, 
        int length)
    {
        string output = value;

        if (!string.IsNullOrEmpty(value))
            output = value.Substring(length, value.Length - length);

        return output;
    }

    public static string Left(
        this string value, 
        int length)
    {
        return value.Substring(0, length);
    }

    public static string Right(
        this string value, 
        int length)
    {
        return value.Substring(value.Length - length, length);
    }

    private static StringSplitOptions GetStringSplitOptions(
        bool removeEmptyEntries)
    {
        StringSplitOptions options = StringSplitOptions.None;
        if (removeEmptyEntries) options = StringSplitOptions.RemoveEmptyEntries;

        return options;
    }

    private static string GetPuncuationChars(
        bool includeDash, 
        bool includeUnderscore)
    {
        string chars = PunctuationCharsBase;
        if (includeDash) chars += PunctuationDash;
        if (includeUnderscore) chars += PunctuationUnderscore;

        return chars;
    }

    public static int? ParseInt(
        this string? value,
        int? defaultValue = null)
    {
        if (value != null && int.TryParse(value, out int parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static uint? ParseUInt(
        this string? value,
        uint? defaultValue = null)
    {
        if (value != null && uint.TryParse(value, out uint parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static long? ParseLong(
        this string? value,
        long? defaultValue = null)
    {
        if (value != null && long.TryParse(value, out long parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static ulong? ParseULong(
        this string? value,
        ulong? defaultValue = null)
    {
        if (value != null && ulong.TryParse(value, out ulong parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static decimal? ParseDecimal(
        this string? value,
        decimal? defaultValue = null)
    {
        if (value != null && decimal.TryParse(value, out decimal parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static double? ParseDouble(
        this string? value,
        double? defaultValue = null)
    {
        if (value != null && double.TryParse(value, out double parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static DateTime? ParseDateTime(
        this string? value,
        DateTime? defaultValue = null)
    {
        if (value != null && DateTime.TryParse(value, out DateTime parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static DateOnly? ParseDateOnly(
        this string? value,
        DateOnly? defaultValue = null)
    {
        if (value != null && DateOnly.TryParse(value, out DateOnly parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static bool? ParseBoolean(
        this string? value,
        bool? defaultValue = null)
    {
        if (value != null && bool.TryParse(value, out bool parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }

    public static Guid? ParseGuid(
        this string? value,
        Guid? defaultValue = null)
    {
        if (value != null && Guid.TryParse(value, out Guid parsedValue))
            return parsedValue;
        else
            return defaultValue;
    }
}
