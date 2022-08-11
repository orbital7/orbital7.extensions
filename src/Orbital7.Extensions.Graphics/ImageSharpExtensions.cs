using System;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using static System.Net.Mime.MediaTypeNames;

namespace SixLabors.ImageSharp
{
    public static class ImageSharpExtensions
    {
        public static IImageProcessingContext EnsureMaximumSize(this IImageProcessingContext source, int maxWidth, int maxHeight,
            bool maintainAspectRatio = true)
        {
            var size = source.GetCurrentSize();

            if ((size.Width > maxWidth) || (size.Height > maxHeight))
            {
                if (maintainAspectRatio)
                {
                    // Handle if width is larger.
                    if (size.Width > size.Height)
                        return source.Resize(maxWidth, Convert.ToInt32(size.Height * maxWidth / size.Width));
                    // Else height is larger.
                    else
                        return source.Resize(Convert.ToInt32(size.Width * maxHeight / size.Height), maxHeight);
                }
                else
                {
                    return source.Resize(maxWidth, maxHeight);
                }
            }
            else
            {
                return source;
            }
        }
    }
}
