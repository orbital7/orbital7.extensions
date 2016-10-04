using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace System.Drawing
{
    public static class DrawingExtensions
    {
        public static byte[] ToByteArray(this Bitmap bitmap)
        {
            return bitmap.ToByteArray(System.Drawing.Imaging.ImageFormat.Png);
        }

        public static byte[] ToByteArray(this Bitmap bitmap, ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, imageFormat);
            return ms.ToArray();
        }

        public static Bitmap ToBitmap(this byte[] imageContents)
        {
            MemoryStream ms = new MemoryStream(imageContents);
            return System.Drawing.Image.FromStream(ms) as System.Drawing.Bitmap;
        }
        
        public static Bitmap Resize(this Bitmap bitmap, int width, int height) 
        { 
            Bitmap result = new Bitmap(width, height); 
            using (Graphics g = Graphics.FromImage((Image)result))
                g.DrawImage(bitmap, 0, 0, width, height);
            return result; 
        }

        public static Bitmap EnsureMaximumSize(this Bitmap bitmap, int maxWidth, int maxHeight, bool maintainAspectRatio)
        {
            if ((bitmap.Width > maxWidth) || (bitmap.Height > maxHeight))
            {
                if (maintainAspectRatio)
                {
                    Bitmap sizedBitmap = bitmap;

                    // Handle if width is larger.
                    if (bitmap.Width > bitmap.Height)
                        sizedBitmap = Resize(bitmap, maxWidth, bitmap.Height * maxWidth / bitmap.Width);
                    // Else height is larger.
                    else
                        sizedBitmap = Resize(bitmap, bitmap.Width * maxHeight / bitmap.Height, maxHeight);

                    return sizedBitmap;
                }
                else
                {
                    return Resize(bitmap, maxWidth, maxHeight);
                }
            }
            else
            {
                return bitmap;
            }
        }
        
        public static Bitmap RemoveTransparency(this Bitmap bitmap)
        {
            return RemoveTransparency(bitmap, Color.White);
        }

        public static Bitmap RemoveTransparency(this Bitmap bitmap, Color background)
        {
            Bitmap target = CreateBitmap(bitmap);
            Graphics g = Graphics.FromImage(target);
            g.Clear(background);
            g.DrawImage(bitmap, 0, 0);

            return target;
        }

        public static Bitmap PadSize(this Bitmap bitmap, double multiplier)
        {
            return PadSize(bitmap, multiplier, Color.White);
        }

        public static Bitmap PadSize(this Bitmap bitmap, double multiplier, Color background)
        {
            int width = (int)(bitmap.Size.Width * multiplier);
            int height = (int)(bitmap.Size.Height * multiplier);
            int left = (int)((width - bitmap.Size.Width) / 2);
            int top = (int)((height - bitmap.Size.Height) / 2);

            // Create the new bitmap.
            Bitmap target = new Bitmap(width, height);
            target.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            // Draw the image.
            Graphics g = Graphics.FromImage(target);
            g.Clear(background);
            g.DrawImage(bitmap, left, top);

            return target;
        }

        public static Bitmap AddGlyph(this Bitmap baseBitmap, Bitmap glyphBitmap, int left, int top)
        {
            Graphics g = Graphics.FromImage(baseBitmap);
            g.DrawImage(glyphBitmap, left, top, glyphBitmap.Width, glyphBitmap.Height);
            return baseBitmap;
        }

        public static Bitmap DrawBorder(this Bitmap bitmap, Color borderColor, int borderWidth)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawRectangle(new Pen(borderColor, borderWidth), 0, 0, bitmap.Width - (2 * borderWidth), bitmap.Height - (2 * borderWidth));
            return bitmap;
        }

        public static Bitmap InvertBlackAndWhite(this Bitmap bitmap)
        {
            Bitmap img = CreateBitmap(bitmap);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cmPicture = new ColorMatrix(new float[][] { 
                                new float[] { -1, 0, 0, 0, 0 }, 
                                new float[] { 0, -1, 0, 0, 0 }, 
                                new float[] { 0, 0, -1, 0, 0 }, 
                                new float[] { 0, 0, 0, 1, 0 }, 
                                new float[] { 1, 1, 1, 0, 1 } });
            ia.SetColorMatrix(cmPicture);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0,
                img.Width, img.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            img.Dispose();

            return bitmap;
        }
        
        public static string GetImageExtension(this ImageFormat imageFormat)
        {
            string fileExtension = String.Empty;

            if (imageFormat.Guid.Equals(ImageFormat.Jpeg.Guid))
                fileExtension = ".jpg";
            else if (imageFormat.Guid.Equals(ImageFormat.Gif.Guid))
                fileExtension = ".gif";
            else if (imageFormat.Guid.Equals(ImageFormat.Png.Guid))
                fileExtension = ".png";
            else if (imageFormat.Guid.Equals(ImageFormat.Bmp))
                fileExtension = ".bmp";
            else if (imageFormat.Guid.Equals(ImageFormat.Tiff))
                fileExtension = ".tif";
            else
                throw new Exception("The specified image format is not supported");

            return fileExtension;
        }

        private static Bitmap CreateBitmap(Image image)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            return bitmap;
        }
    }
}
