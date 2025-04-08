﻿using System.Security.Cryptography;

namespace Orbital7.Extensions.Encryption;

public enum EncryptionMethod 
{ 
    TripleDES,
}

public static class EncryptionHelper
{
    public static string CreatePassphrase(
        int length)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public static string ComputeHmacSha256Hash(
        string input,
        string salt)
    {
        using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
        {
            byte[] hashBytes = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static byte[] Encrypt(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod)
            .Encrypt(
                data, 
                passphrase, 
                encoding);
    }

    public static byte[] Encrypt(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return Encrypt(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    public static byte[] Encrypt(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod)
            .Encrypt(
                encoding.GetBytes(data), 
                passphrase, 
                encoding);
    }

    public static byte[] Encrypt(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return Encrypt(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    public static string EncryptAsString(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return Convert.ToBase64String(
            Encrypt(
                data, 
                passphrase, 
                encryptionMethod, 
                encoding));
    }

    public static string EncryptAsString(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return EncryptAsString(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    public static byte[] Decrypt(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod)
            .Decrypt(
                data, 
                passphrase, 
                encoding);
    }

    public static byte[] Decrypt(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return Decrypt(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    public static string DecryptAsString(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return encoding.GetString(
            Decrypt(
                data, 
                passphrase, 
                encryptionMethod, 
                encoding));
    }

    public static string DecryptAsString(
        byte[] data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return DecryptAsString(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    public static string DecryptAsString(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod, 
        Encoding encoding)
    {
        return DecryptAsString(
            Convert.FromBase64String(data), 
            passphrase, 
            encryptionMethod, 
            encoding);
    }

    public static string DecryptAsString(
        string data, 
        string passphrase, 
        EncryptionMethod encryptionMethod)
    {
        return DecryptAsString(
            data, 
            passphrase, 
            encryptionMethod, 
            GetDefaultEncoding());
    }

    private static IEncryptionEngine GetEncryptionEngine(
        EncryptionMethod encryptionMethod)
    {
        switch (encryptionMethod)
        {
            case EncryptionMethod.TripleDES:
                return new TripleDESEncryptionEngine();

            default:
                throw new Exception($"Encryption method '{encryptionMethod}' is not supported.");
        }
    }

    private static Encoding GetDefaultEncoding()
    {
        return new UTF8Encoding();
    }
}
