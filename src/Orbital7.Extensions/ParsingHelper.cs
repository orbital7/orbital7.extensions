using CsvHelper;
using CsvHelper.Configuration;

namespace Orbital7.Extensions;

// TODO: Some methods duplicate several StringExtensions parse methods.
public static class ParsingHelper
{
    public static short ParseShort(
        string? input,
        short defaultValue = default)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return defaultValue;
    }

    public static short? ParseNullableShort(
        string? input,
        short? defaultValue = null)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return defaultValue;
    }

    public static ushort ParseUShort(
        string? input,
        ushort defaultValue = default)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return defaultValue;
    }

    public static ushort? ParseNullableUShort(
        string? input,
        ushort? defaultValue = null)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return defaultValue;
    }

    public static int ParseInt(
        string? input,
        int defaultValue = default)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }

        return defaultValue;
    }

    public static int? ParseNullableInt(
        string? input,
        int? defaultValue = null)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }

        return defaultValue;
    }

    public static uint ParseUInt(
        string? input,
        uint defaultValue = default)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return defaultValue;
    }

    public static uint? ParseNullableUInt(
        string? input,
        uint? defaultValue = null)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return defaultValue;
    }

    public static long ParseLong(
        string? input,
        long defaultValue = default)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return defaultValue;
    }

    public static long? ParseNullableLong(
        string? input,
        long? defaultValue = null)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return defaultValue;
    }

    public static ulong ParseUlong(
        string? input,
        ulong defaultValue = default)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return defaultValue;
    }

    public static ulong? ParseNullableUlong(
        string? input,
        ulong? defaultValue = null)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return defaultValue;
    }

    public static decimal ParseDecimal(
        string? input,
        decimal defaultValue = default)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return defaultValue;
    }

    public static decimal? ParseNullableDecimal(
        string? input,
        decimal? defaultValue = null)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return defaultValue;
    }

    public static float ParseFloat(
        string? input,
        float defaultValue = default)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return defaultValue;
    }

    public static float? ParseNullableFloat(
        string? input,
        float? defaultValue = null)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return defaultValue;
    }

    public static double ParseDouble(
        string? input,
        double defaultValue = default)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return defaultValue;
    }

    public static double? ParseNullableDouble(
        string? input,
        double? defaultValue = null)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return defaultValue;
    }

    public static DateTime ParseDateTime(
        string? input,
        DateTime defaultValue = default)
    {
        if (DateTime.TryParse(input, out DateTime result))
        {
            return result;
        }

        return defaultValue;
    }

    public static DateTime? ParseNullableDateTime(
        string? input,
        DateTime? defaultValue = null)
    {
        if (DateTime.TryParse(input, out DateTime result))
        {
            return result;
        }

        return defaultValue;
    }

    public static DateOnly ParseDateOnly(
        string? input,
        DateOnly defaultValue = default)
    {
        if (DateOnly.TryParse(input, out DateOnly result))
        {
            return result;
        }

        return defaultValue;
    }

    public static DateOnly? ParseNullableDateOnly(
        string? input,
        DateOnly? defaultValue = null)
    {
        if (DateOnly.TryParse(input, out DateOnly result))
        {
            return result;
        }

        return defaultValue;
    }

    public static bool ParseBool(
        string? input,
        bool defaultValue = default)
    {
        if (bool.TryParse(input, out bool result))
        {
            return result;
        }

        return defaultValue;
    }

    public static bool? ParseNullableBool(
        string? input,
        bool? defaultValue = null)
    {
        if (bool.TryParse(input, out bool result))
        {
            return result;
        }

        return defaultValue;
    }

    public static List<TType> ParseTextFileToTypedList<TType>(
        string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileContents = File.ReadAllText(filePath);
            return ParseTextLinesToTypedList<TType>(fileContents);
        }
        else
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
    }

    public static List<TType> ParseTextLinesToTypedList<TType>(
        string text)
    {
        var lines = text.ParseLines();
        return lines.ToTypedList<TType>();
    }

    public static List<TModel> ParseCsvFileToModels<TModel>(
        string filePath,
        bool hasColumnHeadersRow)
    {
        if (File.Exists(filePath))
        {
            using (var reader = new StreamReader(filePath))
            {
                return ParseCsvStreamReaderToModels<TModel>(
                    reader,
                    hasColumnHeadersRow);
            }
        }
        else
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
    }

    public static List<TModel> ParseCsvContentsToModels<TModel>(
        string csvContents,
        bool hasColumnHeadersRow)
    {
        using (var ms = csvContents.ToStream())
        {
            using (var reader = new StreamReader(ms))
            {
                return ParseCsvStreamReaderToModels<TModel>(
                    reader,
                    hasColumnHeadersRow);
            }
        }
    }

    private static List<TModel> ParseCsvStreamReaderToModels<TModel>(
        StreamReader reader,
        bool hasColumnHeadersRow)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasColumnHeadersRow,
        };

        using (var csv = new CsvReader(reader, config))
        {
            return csv.GetRecords<TModel>().ToList();
        }
    }
}
