using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class NumericExtensions
    {
        public static string ToCurrency(
            this double number, 
            bool addSymbol = false, 
            bool addCommas = true,
            bool addPlusForPositiveNumber = false)
        {
            var format = addCommas ? "#,##0.00" : "0.00";

            var roundedNumber = Math.Round(number, 2);
            var isNegative = roundedNumber < 0;
            var value = String.Empty;

            if (isNegative)
                value += "-";
            else if (addPlusForPositiveNumber && roundedNumber > 0)
                value += "+";

            if (addSymbol)
                value += "$";

            value += Math.Abs(roundedNumber).ToString(format);

            return value;
        }

        public static string ToCurrency(
            this decimal number, 
            bool addSymbol = false, 
            bool addCommas = true,
            bool addPlusForPositiveNumber = false)
        {
            return ToCurrency(Convert.ToDouble(number), addSymbol, addCommas, addPlusForPositiveNumber);
        }

        public static string ToFileSize(this long fileSizeInBytes)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", fileSizeInBytes);
        }

        public static string ToPercent(this double percent)
        {
            return percent.ToString("0.00%");
        }

        public static double ToPercent(this double value, double total)
        {
            if (total != 0)
                return value / total;
            else
                return 0;
        }

        public static string ToHour(this int value)
        {
            return new DateTime(2000, 1, 1, value, 0, 0).ToString("hh:mm tt");
        }

        public static string ToString(this int? value, string nullValue)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return nullValue;
        }

        public static string ToString(this decimal? value, string nullValue)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return nullValue;
        }

        public static string ToString(this double? value, string nullValue)
        {
            if (value.HasValue)
                return value.ToString();
            else
                return nullValue;
        }

        public static decimal SafeDivide(this decimal numerator, decimal denominator, decimal errorOutput = 0)
        {
            return (denominator == 0) ? errorOutput : numerator / denominator;
        }

        public static double SafeDivide(this double numerator, double denominator, double errorOutput = 0)
        {
            return (denominator == 0) ? errorOutput : numerator / denominator;
        }

        public static double SafeDivide(this int numerator, double denominator, double errorOutput = 0)
        {
            return (denominator == 0) ? errorOutput : Convert.ToDouble(numerator) / denominator;
        }
    }
}
