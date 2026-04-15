namespace Orbital7.Extensions;

public static class IOExtensions
{
    public static async Task<string> ReadAllTextAsync(
        this Stream stream,
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(
            stream, 
            encoding ?? Encoding.UTF8);

        return await reader.ReadToEndAsync(
            cancellationToken);
    }

    public static async Task<byte[]> ReadAllBytesAsync(
        this Stream stream,
        CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        await using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer, cancellationToken);
        return buffer.ToArray();
    }
}
