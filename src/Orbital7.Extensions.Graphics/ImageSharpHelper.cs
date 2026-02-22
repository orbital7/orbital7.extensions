using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Orbital7.Extensions.Graphics;

public static class ImageSharpHelper
{
    public static IImageFormat? GetImageFormat(
        string fileExtension)
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

        return null;
    }

    public static async Task<List<Image>> LoadImageFilesAsync(
        IEnumerable<string> filePaths)
    {
        var images = new List<Image>();

        foreach (var filePath in filePaths)
        {
            if (GetImageFormat(Path.GetExtension(filePath)) != null)
            {
                // Load the image.
                var image = await Image.LoadAsync(filePath);

                // TODO: Is there a way to generally record the 
                // name of the file as metadata?

                // Add the image to the list.
                images.Add(image);
            }
        }

        return images;
    }

    public static async Task<List<Image>> LoadImageFilesAsync(
        string folderPath)
    {
        return await LoadImageFilesAsync(Directory.GetFiles(folderPath));
    }
}
