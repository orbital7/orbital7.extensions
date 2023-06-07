namespace System;

public class StringHelper
{
    public static string CreateRandomPassword(
        int passwordLength)
    {
        string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
        char[] chars = new char[passwordLength];
        Random rd = new Random();

        for (int i = 0; i < passwordLength; i++)
            chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

        return new string(chars);
    }
}
