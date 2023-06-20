using SixLabors.ImageSharp.Formats;
using System;

namespace SixLabors.ImageSharp
{
    public static class ImageSharpHelper
    {
        public static IImageFormat GetImageFormat(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".gif":
                    return SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance;

                case ".jpg":
                case ".jpeg":
                    return SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance;

                case ".png":
                    return SixLabors.ImageSharp.Formats.Png.PngFormat.Instance;

                case ".bmp":
                    return SixLabors.ImageSharp.Formats.Bmp.BmpFormat.Instance;

                    //case ".tif":
                    //case ".tiff":
            }

            throw new Exception("The specified file extension is not supported");
        }
    }
}
