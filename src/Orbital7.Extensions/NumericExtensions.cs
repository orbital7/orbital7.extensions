﻿namespace System;

public static class NumericExtensions
{
    public static string ToCurrencyString(
        this double number, 
        bool addSymbol = false, 
        bool addCommas = true,
        bool addPlusForPositiveNumber = false)
    {
        var format = addCommas ? "#,##0.00" : "0.00";

        var roundedNumber = Math.Round(number, 2);
        var isNegative = roundedNumber < 0;
        var value = string.Empty;

        if (isNegative)
            value += "-";
        else if (addPlusForPositiveNumber && roundedNumber > 0)
            value += "+";

        if (addSymbol)
            value += "$";

        value += Math.Abs(roundedNumber).ToString(format);

        return value;
    }

    public static string ToCurrencyString(
        this decimal number, 
        bool addSymbol = false, 
        bool addCommas = true,
        bool addPlusForPositiveNumber = false)
    {
        return ToCurrencyString(Convert.ToDouble(number), addSymbol, addCommas, addPlusForPositiveNumber);
    }

    public static string ToFileSizeString(
        this long fileSizeInBytes)
    {
        return string.Format(new FileSizeFormatProvider(), "{0:fs}", fileSizeInBytes);
    }

    public static string ToPercentString(
        this double percent)
    {
        return percent.ToString("0.00%");
    }

    public static double ToPercentString(
        this double value, 
        double total)
    {
        if (total != 0)
            return value / total;
        else
            return 0;
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

    public static string ToString(
        this int? value, 
        string nullValue)
    {
        if (value.HasValue)
            return value.ToString();
        else
            return nullValue;
    }

    public static string ToString(
        this decimal? value, 
        string nullValue)
    {
        if (value.HasValue)
            return value.ToString();
        else
            return nullValue;
    }

    public static string ToString(
        this double? value, 
        string nullValue)
    {
        if (value.HasValue)
            return value.ToString();
        else
            return nullValue;
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

    public static decimal RoundToTwoDigits(
        this decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public static decimal RoundToNDigits(
        this decimal value,
        int digits)
    {
        return Math.Round(value, digits, MidpointRounding.ToEven);
    }

    public static decimal RoundUpToLargestQuarter(
        this decimal value)
    {
        var nearestOf = 0.25m;
        return Math.Ceiling(value / nearestOf) * nearestOf;
    }

    public static decimal RoundDownToSmallestQuarter(
        this decimal value)
    {
        var nearestOf = 0.25m;
        return Math.Floor(value / nearestOf) * nearestOf;
    }
}
