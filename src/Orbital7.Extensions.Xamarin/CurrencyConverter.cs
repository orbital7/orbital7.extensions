using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Xamarin.Forms
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            if (value != null)
                return decimal.Parse(value.ToString()).ToString("C");
            else
                return null;
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
