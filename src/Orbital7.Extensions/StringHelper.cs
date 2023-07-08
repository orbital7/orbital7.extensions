using System.Security.Cryptography;

namespace System;

public class StringHelper
{
    public static string CreateRandomString(
        int length,
        string characters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-")
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
