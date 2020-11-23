using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Xamarin.Forms
{
    public class VectorCurrencyConverter : IValueConverter
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
                var conversion = d.ToString("C");
                if (d > 0)
                    conversion = "+" + conversion;
                return conversion;
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