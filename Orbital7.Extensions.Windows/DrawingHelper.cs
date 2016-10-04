using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Windows
{
    public class DrawingHelper
    {
        public static Bitmap LoadBitmap(byte[] contents)
        {
            Bitmap bitmap = null;
            MemoryStream ms = new MemoryStream(contents);
            bitmap = new Bitmap(ms);

            return bitmap;
        }

        public static Bitmap LoadBitmap(string filePath)
        {
            Image image = null;

            if (File.Exists(filePath))
            {
                try
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    image = Image.FromStream(fileStream);
                    fileStream.Close();
                }
                catch { }
            }

            return image as Bitmap;
        }

        public static Bitmap LoadImage(string filePath)
        {
            Bitmap bitmap = null;

            // Determine the extension.
            string ext = Path.GetExtension(filePath).ToLower();

            // Handle on the basis of actual bitmap file extensions.
            if (System.IO.IOExtensions.BitmapExtensions.Contains(ext))
                bitmap = new Bitmap(filePath);
            // Else if a metafile, load it.
            else if (System.IO.IOExtensions.MetaFileExtensions.Contains(ext))
                bitmap = LoadMetaFile(filePath);

            return bitmap;
        }

        public static Bitmap LoadMetaFile(string filePath)
        {
            // Open metafile.
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Bitmap bitmap = LoadMetaFile(fileStream);

            // Clean up.
            fileStream.Close();

            return bitmap;
        }

        public static Bitmap LoadMetaFile(Stream stream)
        {
            // Open metafile.
            Metafile metaFile = new Metafile(stream);

            // Convert to bitmap with white background.
            Bitmap bitmap = new Bitmap(metaFile);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            g.DrawImage(metaFile, 0, 0, metaFile.Width, metaFile.Height);

            return bitmap;
        }

        public static Bitmap DownloadImage(string url)
        {
            // Create the HTTP web request.
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 20000;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return Image.FromStream(response.GetResponseStream()) as Bitmap;
        }

        public static ImageFormat GetImageFormat(string fileExtension)
        {
            ImageFormat imageFormat = ImageFormat.Bmp;

            switch (fileExtension.ToLower())
            {
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;

                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;

                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;

                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;

                case ".tif":
                case ".tiff":
                    imageFormat = ImageFormat.Tiff;
                    break;

                default:
                    throw new Exception("The specified file extension is not supported");
            }

            return imageFormat;
        }
    }
}
