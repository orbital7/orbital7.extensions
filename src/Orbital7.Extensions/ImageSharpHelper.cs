using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
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
                    return ImageFormats.Gif;

                case ".jpg":
                case ".jpeg":
                    return ImageFormats.Jpeg;

                case ".png":
                    return ImageFormats.Png;

                case ".bmp":
                    return ImageFormats.Bmp;

                    //case ".tif":
                    //case ".tiff":
            }

            throw new Exception("The specified file extension is not supported");
        }
    }
}
