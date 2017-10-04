using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Orbital7.Extensions.WPF
{
    public static class MediaHelper
    {
        public static BitmapEncoder GetBitmapEncoder(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".gif":
                    return new GifBitmapEncoder();

                case ".jpg":
                case ".jpeg":
                    return new JpegBitmapEncoder();

                case ".png":
                    return new PngBitmapEncoder();

                case ".bmp":
                    return new BmpBitmapEncoder();

                case ".tif":
                case ".tiff":
                    return new TiffBitmapEncoder();
            }

            throw new Exception("The specified file extension is not supported");
        }

        public static BitmapImage LoadBitmapImage(string filePath)
        {
            var imgTemp = new BitmapImage();
            imgTemp.BeginInit();
            imgTemp.CacheOption = BitmapCacheOption.OnLoad;
            imgTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            imgTemp.UriSource = new Uri(filePath, UriKind.Absolute);
            imgTemp.EndInit();

            return imgTemp;
        }

        public static BitmapImage LoadBitmapImage(byte[] bitmapContents)
        {
            using (var stream = new MemoryStream(bitmapContents))
            {
                var imgTemp = new BitmapImage();
                imgTemp.BeginInit();
                imgTemp.CacheOption = BitmapCacheOption.OnLoad;
                imgTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                imgTemp.StreamSource = stream;
                imgTemp.EndInit();

                return imgTemp;
            }
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
