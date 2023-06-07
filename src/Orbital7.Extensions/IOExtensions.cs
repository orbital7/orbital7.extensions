namespace System.IO;

public static class IOExtensions
{
    public static readonly string[] BitmapExtensions = { ".gif", ".jpg", ".jpeg", ".png", ".bmp", ".tif", ".tiff" };
    public static readonly string[] MetaFileExtensions = { ".wmf", ".emf" };

    public static byte[] ReadAll(this Stream stream, int initialLength)
    {
        // If we've been passed an unhelpful initial length, just
        // use 32K.
        if (initialLength < 1)
        {
            initialLength = 32768;
        }

        byte[] buffer = new byte[initialLength];
        int read = 0;

        int chunk;
        while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
        {
            read += chunk;

            // If we've reached the end of our buffer, check to see if there's
            // any more information
            if (read == buffer.Length)
            {
                int nextByte = stream.ReadByte();

                // End of stream? If so, we're done
                if (nextByte == -1)
                {
                    return buffer;
                }

                // Nope. Resize the buffer, put in the byte we've just
                // read, and continue
                byte[] newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }
        }

        // Buffer is now too big. Shrink it.
        byte[] ret = new byte[read];
        Array.Copy(buffer, ret, read);
        return ret;
    }

    public static byte[] ReadAll(this Stream stream)
    {
        return stream.ReadAll(-1);
    }

    public static string ReadText(this Stream stream)
    {
        return stream.ReadAll().ToTextString();
    }

    public static string ReadXML(this Stream stream)
    {
        string xml = stream.ReadAll().ToTextString();
        int index = xml.IndexOf("<");
        xml = xml.Substring(index, xml.Length - index);

        return xml;
    }
}
