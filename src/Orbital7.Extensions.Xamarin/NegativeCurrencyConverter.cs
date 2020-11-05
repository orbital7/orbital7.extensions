using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Xamarin.Forms
{
    public class NegativeCurrencyConverter : IValueConverter
    {
        public object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (value != null)
            {
                var d = decimal.Parse(value.ToString());
                if (d != 0)
                    return $"-{d:C}";
                else
                    return $"{d:C}";
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            string valueFromString = Regex.Replace(value.ToString(), @"\D", "");

            if (valueFromString.Length <= 0)
                return null;

            if (!long.TryParse(valueFromString, out long valueLong))
                return null;

            if (valueLong < 0)
                return null;

            return valueLong / 100m;
        }
    }
}
