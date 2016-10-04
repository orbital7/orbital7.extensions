using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class NumberExtensions
    {
        public static string ToCurrency(this double number, bool addSymbol = false, bool addCommas = true)
        {
            string format = "#,##0.00";
            if (!addCommas) format = "0.00";

            double roundedNumber = Math.Round(number, 2);
            bool isNegative = roundedNumber < 0;
            string value = String.Empty;
            if (isNegative) value += "-";
            if (addSymbol) value += "$";
            value += Math.Abs(roundedNumber).ToString(format);

            return value;
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
    }
}
