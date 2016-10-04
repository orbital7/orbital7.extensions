using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        public const string IllegalFileSystemChars = "|/\\\"*?:<>";
        public const string PunctuationDash = "-";
        public const string PunctuationUnderscore = "_";
        public const string PunctuationCharsBase = ".,()[]{}|\\';:!@#$&%?/<>–^+=*\"";
        public const string PunctuationChars = PunctuationCharsBase + PunctuationDash + PunctuationUnderscore;
        public const string NumberChars = "0123456789";
        public const string LetterChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public const string AlphanumericChars = NumberChars + LetterChars;
        public const string WhitespaceChars = " \r\n\t\v\f";

        public static MemoryStream ToStream(this string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public static string EnsureContent(this string value, string emptyText)
        {
            if (!String.IsNullOrEmpty(value))
                return value;
            else
                return emptyText;
        }

        public static string ToTextString(this byte[] bytes)
        {
            //ASCIIEncoding encoding = new ASCIIEncoding();
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public static byte[] ToByteArray(this string value)
        {
            //ASCIIEncoding encoding = new ASCIIEncoding();
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(value);
        }

        public static string StripInvalidXMLCharacters(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                StringBuilder textOut = new StringBuilder(); // Used to hold the output.   
                char current; // Used to reference the current character.   

                if (text == null || text == string.Empty) return string.Empty; // vacancy test.   
                for (int i = 0; i < text.Length; i++)
                {
                    current = text[i];

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
            else
            {
                return text;
            }
        }

        public static string ToSeparatedWords(this string value)
        {
            if (value != null)
            {
                return Regex.Replace(value, "([A-Z][a-z]?)", " $1").Trim();
            }
            return null;
        }

        public static string EnsureNonEmptyHTMLContent(this string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty(value.Trim()))
                return "&nbsp;";
            else
                return value;
        }

        public static string Mask(this string value)
        {
            string maskChar = "*";

            if (!String.IsNullOrEmpty(value))
                return new string(maskChar.ToCharArray()[0], value.Length);
            else
                return value;
        }

        public static string MaskNumber(this string number, int endingCount)
        {
            string maskedNumber = String.Empty;

            string endingNumber = number.Substring(number.Length - endingCount, endingCount);
            string leftSide = number.Substring(0, number.Length - endingCount);
            foreach (char c in leftSide.ToCharArray())
            {
                if (c.Equals(' ') || c.Equals('-'))
                    maskedNumber += c;
                else
                    maskedNumber += '*';
            }

            return maskedNumber + endingNumber;
        }

        public static string EnsureMaxStringLength(this string input, int maxLength)
        {
            if (!String.IsNullOrEmpty(input) && (input.Length > maxLength))
                return input.Substring(0, maxLength);
            else
                return input;
        }

        public static bool ContainsChars(this string value, string chars)
        {
            return ContainsChars(value, chars.ToCharArray());
        }

        public static bool ContainsChars(this string value, char[] chars)
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

        public static bool HasText(this string value)
        {
            return (!String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(value.Trim()));
        }

        public static string NumbersOnly(this string value)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in value.ToCharArray())
            {
                string s = c.ToString();
                if (NumberChars.Contains(s)) sb.Append(s);
            }

            return sb.ToString();
        }

        public static bool IsNumbers(this string value)
        {
            foreach (char c in value.ToCharArray())
                if (!NumberChars.Contains(c.ToString()))
                    return false;

            return true;
        }

        public static bool IsLetters(this string value)
        {
            foreach (char c in value.ToCharArray())
                if (!LetterChars.Contains(c.ToString()))
                    return false;

            return true;
        }

        public static bool IsAlphanumeric(this string value)
        {
            foreach (char c in value.ToCharArray())
                if (!AlphanumericChars.Contains(c.ToString()))
                    return false;

            return true;
        }

        public static string[] Parse(this string input, bool byWhitespace, bool byPunctuation, bool includeDash, bool includeUnderscore, bool includeDelimitersInResults, bool removeEmptyEntries)
        {
            string[] result = null;

            string chars = String.Empty;
            if (byWhitespace) chars += WhitespaceChars;
            if (byPunctuation) chars += GetPuncuationChars(includeDash, includeUnderscore);

            if (includeDelimitersInResults)
            {
                var delimiters = chars.StringToStringArray();
                if (delimiters.Length > 0)
                {
                    string pattern = "(" + String.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) + ")";
                    result = Regex.Split(input, pattern);
                }
            }
            else
            {
                result = input.Split(chars.ToCharArray(), GetStringSplitOptions(removeEmptyEntries));
            }

            // Verify.
            if (result.Length == 0)
                result = new string[1] { input };

            return result;
        }

        public static string[] ParseLines(this string value, bool removeEmptyEntries = true)
        {
            return value.NormalizeLineTerminators("\n").Split("\n".ToCharArray(), GetStringSplitOptions(removeEmptyEntries));
        }

        public static string[] Parse(this string value, string delimiter, bool removeEmptyEntries = true)
        {
            if (value != null)
                return value.Split(new string[] { delimiter }, GetStringSplitOptions(removeEmptyEntries));
            else
                return new string[] { };
        }

        public static List<Guid> ParseGuids(this string value, string delimiter)
        {
            var items = new List<Guid>();

            if (value != null)
            {
                var list = value.Split(new string[] { delimiter }, GetStringSplitOptions(true));
                foreach (string item in list)
                    items.Add(Guid.Parse(item));
            }

            return items;
        }

        public static List<string> ParseBetween(this string value, string start, string end)
        {
            return ParseBetween(value, start, end, true);
        }

        public static List<string> ParseBetween(this string value, string start, string end, bool removeEmptyEntries)
        {
            List<string> output = new List<string>();
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
                    if (!removeEmptyEntries || !String.IsNullOrEmpty(item))
                        output.Add(item);

                    // Find the start of the next item.
                    startIndex = value.IndexOf(start, endIndex + endLength);
                }
                else
                {
                    break;
                }
            }

            return output;
        }

        public static string GetSingleLine(this string value)
        {
            string output = value;

            // Parse by return and newline characters.
            string separator = "\r\n";
            string[] lines = value.Split(separator.ToCharArray(), StringSplitOptions.None);

            // Return only the first line.
            if (lines.Length > 0) output = lines[0];

            return output;
        }

        public static string FindFirstBetween(this string value, string start, string end)
        {
            return FindFirstBetween(value, start, end, true);
        }

        public static string FindFirstBetween(this string value, string start, string end, bool autoTrim)
        {
            string output = String.Empty;

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
            if (autoTrim) output = output.Trim();

            return output;
        }

        public static string[] StringToStringArray(this string input)
        {
            string[] chars = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
                chars[i] = input.Substring(i, 1);

            return chars;
        }

        public static string NormalizeLineTerminators(this string value, string lineTerm = "\n")
        {
            return value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", lineTerm);
        }

        public static string GetLineTerminator(this string value)
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

        public static string EncloseInQuotes(this string value)
        {
            return "\"" + value + "\"";
        }

        public static string RemoveQuotes(this string value)
        {
            return PruneEnd(PruneStart(value, "\""), "\"");
        }

        public static List<string> ToNGrams(this string input, int min, int max)
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
                int oldPos = pos;
                pos++;
            }

            return nGrams;
        }

        public static string PruneEnd(this string value, string end)
        {
            string output = value;

            if (value.EndsWith(end))
                output = value.Substring(0, value.Length - end.Length);

            return output;
        }

        public static string PruneEnd(this string value, int length)
        {
            string output = value;

            output = value.Substring(0, value.Length - length);

            return output;
        }

        public static string PruneStart(this string value, string start)
        {
            string output = value;

            if (value.StartsWith(start, StringComparison.CurrentCultureIgnoreCase))
                output = value.Substring(start.Length, value.Length - start.Length);

            return output;
        }

        public static string PruneStart(this string value, int length)
        {
            string output = value;

            output = value.Substring(length, value.Length - length);

            return output;
        }

        public static string Left(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        private static StringSplitOptions GetStringSplitOptions(bool removeEmptyEntries)
        {
            StringSplitOptions options = StringSplitOptions.None;
            if (removeEmptyEntries) options = StringSplitOptions.RemoveEmptyEntries;

            return options;
        }

        private static string GetPuncuationChars(bool includeDash, bool includeUnderscore)
        {
            string chars = PunctuationCharsBase;
            if (includeDash) chars += PunctuationDash;
            if (includeUnderscore) chars += PunctuationUnderscore;

            return chars;
        }
    }
}
