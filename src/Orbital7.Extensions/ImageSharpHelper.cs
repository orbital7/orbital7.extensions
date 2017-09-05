using ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions
{
    public static class ImageSharpHelper
    {
        public static IImageFormat GetImageFormat(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".gif":
                    return new GifFormat();

                case ".jpg":
                case ".jpeg":
                    return new JpegFormat();

                case ".png":
                    return new PngFormat();

                case ".bmp":
                    return new BmpFormat();

                    //case ".tif":
                    //case ".tiff":
            }

            throw new Exception("The specified file extension is not supported");
        }
    }
}
