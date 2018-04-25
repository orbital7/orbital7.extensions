using System;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Transforms;

namespace SixLabors.ImageSharp
{
    public static class ImageSharpExtensions
    {
        public static IImageProcessingContext<TPixel> EnsureMaximumSize<TPixel>(this IImageProcessingContext<TPixel> source, int maxWidth, int maxHeight,
            bool maintainAspectRatio = true) where TPixel : struct, IPixel<TPixel>
        {
            return source.Apply(image =>
            {
                if ((image.Width > maxWidth) || (image.Height > maxHeight))
                {
                    if (maintainAspectRatio)
                    {
                        // Handle if width is larger.
                        if (image.Width > image.Height)
                            image.Mutate(x => x.Resize(maxWidth, Convert.ToInt32(image.Height * maxWidth / image.Width)));
                        // Else height is larger.
                        else
                            image.Mutate(x => x.Resize(Convert.ToInt32(image.Width * maxHeight / image.Height), maxHeight));
                    }
                    else
                    {
                        image.Mutate(x => x.Resize(maxWidth, maxHeight));
                    }
                }
            });
        }
    }
}
