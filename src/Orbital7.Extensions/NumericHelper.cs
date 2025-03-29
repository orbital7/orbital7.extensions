namespace Orbital7.Extensions;

// TODO: These methods duplicate several StringExtensions parse methods.

public static class NumericHelper
{
    public static short? ParseNullableShort(
        string input)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return null;
    }

    public static ushort? ParseNullableUShort(
        string input)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return null;
    }

    public static uint? ParseNullableUInt(
        string input)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return null;
    }

    public static long? ParseNullableLong(
        string input)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return null;
    }

    public static ulong? ParseNullableUlong(
        string input)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return null;
    }

    public static decimal? ParseNullableDecimal(
        string input)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return null;
    }

    public static float? ParseNullableFloat(
        string input)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return null;
    }

    public static double? ParseNullableDouble(
        string input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return null;
    }

    public static short ParseShort(
        string input)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return default;
    }

    public static ushort ParseUShort(
        string input)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return default;
    }

    public static uint ParseUInt(
        string input)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return default;
    }

    public static long ParseLong(
        string input)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return default;
    }

    public static ulong ParseUlong(
        string input)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return default;
    }

    public static decimal ParseDecimal(
        string input)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return default;
    }

    public static float ParseFloat(
        string input)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return default;
    }

    public static double ParseDouble(
        string input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return default;
    }
}
