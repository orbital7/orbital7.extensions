using Orbital7.Extensions.NETFramework.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Media.Imaging
{
    public static class MediaExtensions
    {
        public static System.Drawing.Bitmap ToBitmap(this BitmapSource source)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
              new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
              System.Drawing.Imaging.ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);

            bmp.UnlockBits(data);

            return bmp;
        }
        
        public static byte[] ToByteArray(this BitmapSource source, BitmapEncoder encoder)
        {
            encoder.Frames.Add(BitmapFrame.Create(source));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        public static byte[] ToByteArray(this BitmapSource source, string fileExtension)
        {
            return source.ToByteArray(MediaHelper.GetBitmapEncoder(fileExtension));
        }
    }
}
