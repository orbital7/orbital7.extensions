using Orbital7.Extensions.Formatters;

namespace Orbital7.Extensions;

public static class NumericExtensions
{
    public static bool IsOdd(
        this int value)
    {
        return (value % 2) != 0;
    }

    public static bool IsEven(
        this int value)
    {
        return (value % 2) == 0;
    }

    public static string ToRoundedString(
        this double value,
        int decimalPlaces = 2,
        MidpointRounding roundingMode = MidpointRounding.ToEven,
        bool addCommas = true,
        bool addPlusIfPositive = false,
        string? baseNumberPrefix = null)
    {
        var output = baseNumberPrefix + Math
            .Round(Math.Abs(value), decimalPlaces, roundingMode)
            .ToString(GetDecimalStringFormat(decimalPlaces, addCommas));

        if (value < 0)
        {
            return "-" + output;
        }
        else if (addPlusIfPositive && value > 0)
        {
            return "+" + output;
        }
        else
        {
            return output;
        }
    }

    public static string ToRoundedString(
        this decimal value,
        int decimalPlaces = 2,
        MidpointRounding roundingMode = MidpointRounding.ToEven,
        bool addCommas = true,
        bool addPlusIfPositive = false,
        string? baseNumberPrefix = null)
    {
        return Convert.ToDouble(value).ToRoundedString(
            decimalPlaces: decimalPlaces,
            roundingMode: roundingMode,
            addCommas: addCommas,
            addPlusIfPositive: addPlusIfPositive);
    }

    public static string ToCurrencyString(
        this double number, 
        DisplayValueOptions? options = null)
    {
        var displayOptions = options ?? new DisplayValueOptions();

        return number.ToRoundedString(
            decimalPlaces: displayOptions.CurrencyDecimalPlaces,
            roundingMode: displayOptions.CurrencyRoundingMode,
            addCommas: displayOptions.CurrencyAddCommas,
            addPlusIfPositive: displayOptions.CurrencyAddPlusIfPositive,
            baseNumberPrefix: displayOptions.CurrencyAddSymbol ? "$" : null);
    }

    public static string ToCurrencyString(
        this decimal number,
        DisplayValueOptions? options = null)
    {
        return Convert.ToDouble(number).ToCurrencyString(
            options: options);
    }

    public static string ToPercentString(
        this double percent,
        int decimalPlaces,
        MidpointRounding roundingMode = MidpointRounding.ToEven,
        bool addCommas = true,
        bool addPlusIfPositive = false)
    {
        return (percent * 100).ToRoundedString(
            decimalPlaces: decimalPlaces,
            roundingMode: roundingMode,
            addCommas: addCommas,
            addPlusIfPositive: addPlusIfPositive) + "%";
    }

    public static string ToPercentString(
        this double percent,
        DisplayValueOptions? options = null)
    {
        var displayOptions = options ?? new DisplayValueOptions();

        return percent.ToPercentString(
            displayOptions.PercentageDecimalPlaces,
            roundingMode: displayOptions.PercentageRoundingMode,
            addCommas: displayOptions.PercentageAddCommas,
            addPlusIfPositive: displayOptions.PercentageAddPlusIfPositive);
    }

    public static string ToFileSizeString(
        this long fileSizeInBytes)
    {
        return string.Format(new FileSizeFormatProvider(), "{0:fs}", fileSizeInBytes);
    }

    public static string ToHourString(
        this int value,
        bool includeMinutes = false)
    {
        if (includeMinutes)
            return new DateTime(2000, 1, 1, value, 0, 0).ToString("hh:mm tt");
        else
            return new DateTime(2000, 1, 1, value, 0, 0).ToString("htt");
    }

    public static string ToTimeString(
        this double value,
        bool includeLeadingZero = false)
    {
        var format = includeLeadingZero ? "hh:mm tt" : "h:mm tt";

        var iPart = (int)value;
        var dPart = value - iPart;
        return new DateTime(2000, 1, 1, iPart, (int)Math.Round(60 * dPart, 2), 0).ToString(format);
    }

    public static decimal SafeDivide(
        this decimal numerator, 
        decimal denominator, 
        decimal errorOutput = 0)
    {
        return (denominator == 0) ? errorOutput : numerator / denominator;
    }

    public static double SafeDivide(
        this double numerator, 
        double denominator, 
        double errorOutput = 0)
    {
        return (denominator == 0) ? errorOutput : numerator / denominator;
    }

    public static double SafeDivide(
        this int numerator, 
        double denominator, 
        double errorOutput = 0)
    {
        return (denominator == 0) ? errorOutput : Convert.ToDouble(numerator) / denominator;
    }

    public static string ToVectorString(
            this int value)
    {
        if (value >= 0)
            return "+" + value.ToString();
        else
            return value.ToString();
    }

    public static string ToVectorString(
        this long value)
    {
        if (value >= 0)
            return "+" + value.ToString();
        else
            return value.ToString();
    }

    public static string ToVectorString(
        this double value)
    {
        if (value >= 0)
            return "+" + value.ToString();
        else
            return value.ToString();
    }

    public static string ToNthString(
        this int value)
    {
        if (value == 1)
            return "1st";
        else if (value == 2)
            return "2nd";
        else if (value == 3)
            return "3rd";
        else
            return value + "th";
    }

    public static double RoundToPointFive(
        this double value)
    {
        return Math.Ceiling(value * 2) / 2;
    }

    public static decimal RoundToNDigits(
        this decimal value,
        int digits,
        MidpointRounding roundingMode = MidpointRounding.ToEven)
    {
        return Math.Round(value, digits, roundingMode);
    }

    public static decimal RoundToTwoDigits(
        this decimal value,
        MidpointRounding roundingMode = MidpointRounding.ToEven)
    {
        return value.RoundToNDigits(2, roundingMode);
    }

    public static double RoundToNDigits(
        this double value,
        int digits,
        MidpointRounding roundingMode = MidpointRounding.ToEven)
    {
        return Math.Round(value, digits, roundingMode);
    }

    public static double RoundToTwoDigits(
        this double value,
        MidpointRounding roundingMode = MidpointRounding.ToEven)
    {
        return value.RoundToNDigits(2, roundingMode);
    }

    // NOTE: This should be the same as RoundUpToNearestStep() using step = 0.25m.
    public static decimal RoundUpToLargestQuarter(
        this decimal value)
    {
        var nearestOf = 0.25m;
        return Math.Ceiling(value / nearestOf) * nearestOf;
    }

    // NOTE: This should be the same as RoundDownToNearestStep() using step = 0.25m.
    public static decimal RoundDownToSmallestQuarter(
        this decimal value)
    {
        var nearestOf = 0.25m;
        return Math.Floor(value / nearestOf) * nearestOf;
    }

    public static decimal RoundUpToNearestStep(
        this decimal value,
        decimal step)
    {
        // If the value or step 0, return the value.
        if (step == 0)
        {
            return value;
        }
        else
        {
            var remainder = value % step;

            // If both the value and step have the same sign, 
            // then we're rounding "up".
            if ((value >= 0 && step >= 0) ||
                (value < 0 && step < 0))
            {
                if (remainder == 0)
                    return value;
                else
                    return value + (step - remainder);
            }
            // Otherwise, we're actually rounding "down" here.
            else
            {
                return value - remainder;
            }
        }
    }

    public static decimal RoundDownToNearestStep(
        this decimal value,
        decimal step)
    {
        // If the value or step 0, return the value.
        if (step == 0)
        {
            return value;
        }
        else
        {
            var remainder = value % step;

            // If both the value and step have the same sign, 
            // then we're rounding "down".
            if ((value >= 0 && step >= 0) ||
                (value < 0 && step < 0))
            {
                return value - remainder;
            }
            // Otherwise, we're actually rounding "up" here.
            else
            {
                if (remainder == 0)
                    return value;
                else
                    return value - (step + remainder);
            }
        }
    }

    public static DateTime FromUnixEpochSecondsToDateTimeUtc(
        this long seconds)
    {
        // Returns a DateTime in UTC.
        return DateTime.UnixEpoch.AddSeconds(seconds);
    }

    private static string GetDecimalStringFormat(
        int decimalPlaces,
        bool addCommas)
    {
        var decimalPlacesFormat =  $"0{((decimalPlaces > 0) ? "." : "")}{new string('0', decimalPlaces)}";

        return addCommas ? 
            $"#,##{decimalPlacesFormat}" : 
            decimalPlacesFormat;
    }
}
