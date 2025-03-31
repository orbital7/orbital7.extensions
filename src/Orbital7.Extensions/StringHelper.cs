using System.Security.Cryptography;

namespace Orbital7.Extensions;

public class StringHelper
{
    public static string CreateRandom(
        int length,
        string characters = StringExtensions.AlphanumericChars)
    {
        var chars = characters.ToCharArray();
        byte[] data = new byte[4 * length];
        RandomNumberGenerator.Fill(data);

        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }
}
