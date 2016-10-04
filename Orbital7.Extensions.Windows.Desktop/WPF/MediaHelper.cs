using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Orbital8.Utility.WPF
{
    public static class MediaHelper
    {
        public static ImageSource GetBitmapImageSource(string projectPath)
        {
            return new BitmapImage(new Uri(projectPath, UriKind.Relative));
        }

        public static LinearGradientBrush GetVerticalGradientBrush(string colorTop, string colorBottom)
        {
            return GetVerticalGradientBrush(
                (Color)ColorConverter.ConvertFromString(colorTop),
                (Color)ColorConverter.ConvertFromString(colorBottom));
        }

        public static LinearGradientBrush GetVerticalGradientBrush(Color colorTop, Color colorBottom)
        {
            GradientStopCollection gradients = new GradientStopCollection();
            gradients.Add(new GradientStop(colorTop, 0));
            gradients.Add(new GradientStop(colorBottom, 1));
            LinearGradientBrush brush = new LinearGradientBrush(gradients);
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 1);

            return brush;
        }

        public static LinearGradientBrush GetDiagonalGradientBrush(string colorTop, string colorBottom)
        {
            return GetDiagonalGradientBrush(
                (Color)ColorConverter.ConvertFromString(colorTop),
                (Color)ColorConverter.ConvertFromString(colorBottom));
        }

        public static LinearGradientBrush GetDiagonalGradientBrush(Color colorTop, Color colorBottom)
        {
            GradientStopCollection gradients = new GradientStopCollection();
            gradients.Add(new GradientStop(colorTop, 0));
            gradients.Add(new GradientStop(colorBottom, 1));
            LinearGradientBrush brush = new LinearGradientBrush(gradients);
            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(1, 1);

            return brush;
        }

        public static Color ColorFromPredefinedName(string colorName)
        {
            Type colorType = (typeof(System.Windows.Media.Colors));
            if (colorType.GetProperty(colorName) != null)
            {
                object o = colorType.InvokeMember(colorName, BindingFlags.GetProperty, null, null, null);
                if (o != null)
                    return (Color)o;
            }

            return Colors.Black;
        }

        public static Color ColorFromHTML(string htmlValue)
        {
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(htmlValue);
            return Color.FromRgb(color.R, color.G, color.B);
        }
    }
}
