using System.Security.Cryptography;

namespace Orbital7.Extensions;

public class StringHelper
{
    public static string GetNumericStringFormat(
        int decimalPlaces,
        bool addCommas)
    {
        var decimalPlacesFormat = $"0{((decimalPlaces > 0) ? "." : "")}{new string('0', decimalPlaces)}";

        return addCommas ?
            $"#,##{decimalPlacesFormat}" :
            decimalPlacesFormat;
    }

    public static string CreateRandom(
        int length,
        string characters = StringExtensions.AlphanumericChars)
    {
        var chars = characters.ToCharArray();
        byte[] data = new byte[4 * length];
        RandomNumberGenerator.Fill(data);

        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }

    public static TEnum ParseEnum<TEnum>(
        string value)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), value);
    }

    public static TEnum? ParseNullableEnum<TEnum>(
        string? value)
    {
        if (value.HasText())
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        return default;
    }

    public static short ParseShort(
        string? input)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return default;
    }

    public static short? ParseNullableShort(
        string? input)
    {
        if (short.TryParse(input, out short result))
        {
            return result;
        }

        return default;
    }

    public static ushort ParseUShort(
        string? input)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return default;
    }

    public static ushort? ParseNullableUShort(
        string? input)
    {
        if (ushort.TryParse(input, out ushort result))
        {
            return result;
        }

        return default;
    }

    public static int ParseInt(
        string? input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }

        return default;
    }

    public static int? ParseNullableInt(
        string? input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }

        return default;
    }

    public static uint ParseUInt(
        string? input)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return default;
    }

    public static uint? ParseNullableUInt(
        string? input)
    {
        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return default;
    }

    public static long ParseLong(
        string? input)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return default;
    }

    public static long? ParseNullableLong(
        string? input)
    {
        if (long.TryParse(input, out long result))
        {
            return result;
        }

        return default;
    }

    public static ulong ParseULong(
        string? input)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return default;
    }

    public static ulong? ParseNullableULong(
        string? input)
    {
        if (ulong.TryParse(input, out ulong result))
        {
            return result;
        }

        return default;
    }

    public static decimal ParseDecimal(
        string? input)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return default;
    }

    public static decimal? ParseNullableDecimal(
        string? input)
    {
        if (decimal.TryParse(input, out decimal result))
        {
            return result;
        }

        return default;
    }

    public static float ParseFloat(
        string? input)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return default;
    }

    public static float? ParseNullableFloat(
        string? input)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }

        return default;
    }

    public static double ParseDouble(
        string? input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return default;
    }

    public static double? ParseNullableDouble(
        string? input)
    {
        if (double.TryParse(input, out double result))
        {
            return result;
        }

        return default;
    }

    public static DateTime ParseDateTime(
        string? input)
    {
        if (DateTime.TryParse(input, out DateTime result))
        {
            return result;
        }

        return default;
    }

    public static DateTime? ParseNullableDateTime(
        string? input)
    {
        if (DateTime.TryParse(input, out DateTime result))
        {
            return result;
        }

        return default;
    }

    public static DateOnly ParseDateOnly(
        string? input)
    {
        if (DateOnly.TryParse(input, out DateOnly result))
        {
            return result;
        }

        return default;
    }

    public static DateOnly? ParseNullableDateOnly(
        string? input)
    {
        if (DateOnly.TryParse(input, out DateOnly result))
        {
            return result;
        }

        return default;
    }

    public static TimeOnly ParseTimeOnly(
        string? input)
    {
        if (TimeOnly.TryParse(input, out TimeOnly result))
        {
            return result;
        }

        return default;
    }

    public static TimeOnly? ParseNullableTimeOnly(
        string? input)
    {
        if (TimeOnly.TryParse(input, out TimeOnly result))
        {
            return result;
        }

        return default;
    }

    public static bool ParseBool(
        string? input)
    {
        if (bool.TryParse(input, out bool result))
        {
            return result;
        }

        return default;
    }

    public static bool? ParseNullableBool(
        string? input)
    {
        if (bool.TryParse(input, out bool result))
        {
            return result;
        }

        return default;
    }

    public static Guid ParseGuid(
        string? input)
    {
        if (Guid.TryParse(input, out Guid result))
        {
            return result;
        }

        return default;
    }

    public static Guid? ParseNullableGuid(
        string? input)
    {
        if (Guid.TryParse(input, out Guid result))
        {
            return result;
        }

        return default;
    }

    public static List<Guid> ParseGuids(
        string? value,
        string delimiter,
        bool allowDuplicates,
        bool allowEmptyGuids)
    {
        var items = new List<Guid>();

        if (value != null)
        {
            items = (from x in value.Parse(delimiter)
                     let a = Guid.Parse(x)
                     where allowEmptyGuids || a != Guid.Empty
                     select a).ToList();
            if (!allowDuplicates)
                items = items.Distinct().ToList();
        }

        return items;
    }

    public static TType? ParseToType<TType>(
        string? value)
    {
        var type = typeof(TType);

        if (type == typeof(string))
        {
            return (TType?)(object?)value;
        }
        else if (type == typeof(Guid))
        {
            return (TType)(object)ParseGuid(value);
        }
        else if (type == typeof(Guid?))
        {
            return (TType?)(object?)ParseNullableGuid(value);
        }
        else if (type == typeof(short))
        {
            return (TType)(object)ParseShort(value);
        }
        else if (type == typeof(short?))
        {
            return (TType?)(object?)ParseNullableShort(value);
        }
        else if (type == typeof(int))
        {
            return (TType)(object)ParseInt(value);
        }
        else if (type == typeof(int?))
        {
            return (TType?)(object?)ParseNullableInt(value);
        }
        else if (type == typeof(uint))
        {
            return (TType)(object)ParseUInt(value);
        }
        else if (type == typeof(uint?))
        {
            return (TType?)(object?)ParseNullableUInt(value);
        }
        else if (type == typeof(long))
        {
            return (TType)(object)ParseLong(value);
        }
        else if (type == typeof(long?))
        {
            return (TType?)(object?)ParseNullableLong(value);
        }
        else if (type == typeof(ulong))
        {
            return (TType)(object)ParseULong(value);
        }
        else if (type == typeof(ulong?))
        {
            return (TType?)(object?)ParseNullableULong(value);
        }
        else if (type == typeof(float))
        {
            return (TType)(object)ParseFloat(value);
        }
        else if (type == typeof(float?))
        {
            return (TType?)(object?)ParseNullableFloat(value);
        }
        else if (type == typeof(double))
        {
            return (TType)(object)ParseDouble(value);
        }
        else if (type == typeof(double?))
        {
            return (TType?)(object?)ParseNullableDouble(value);
        }
        else if (type == typeof(DateTime))
        {
            return (TType)(object)ParseDateTime(value);
        }
        else if (type == typeof(DateTime?))
        {
            return (TType?)(object?)ParseNullableDateTime(value);
        }
        else if (type == typeof(DateOnly))
        {
            return (TType)(object)ParseDateOnly(value);
        }
        else if (type == typeof(DateOnly?))
        {
            return (TType?)(object?)ParseNullableDateOnly(value);
        }
        else if (type == typeof(TimeOnly))
        {
            return (TType)(object)ParseTimeOnly(value);
        }
        else if (type == typeof(TimeOnly?))
        {
            return (TType?)(object?)ParseNullableTimeOnly(value);
        }
        else if (type == typeof(bool))
        {
            return (TType)(object)ParseBool(value);
        }
        else if (type == typeof(bool?))
        {
            return (TType?)(object?)ParseNullableBool(value);
        }

        throw new NotSupportedException($"Conversion from string to {typeof(TType).Name} is not supported.");
    }
}
