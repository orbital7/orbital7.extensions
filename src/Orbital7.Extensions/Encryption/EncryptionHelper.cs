namespace Orbital7.Extensions.Encryption;

public enum EncryptionMethod 
{ 
    TripleDES 
}

public static class EncryptionHelper
{
    public static byte[] Encrypt(byte[] data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod).Encrypt(data, passphrase, encoding);
    }

    public static byte[] Encrypt(byte[] data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return Encrypt(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    public static byte[] Encrypt(string data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod).Encrypt(new System.Text.UTF8Encoding().GetBytes(data), passphrase, encoding);
    }

    public static byte[] Encrypt(string data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return Encrypt(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    public static string EncryptAsString(string data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return Convert.ToBase64String(Encrypt(data, passphrase, encryptionMethod, encoding));
    }

    public static string EncryptAsString(string data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return EncryptAsString(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    public static byte[] Decrypt(byte[] data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return GetEncryptionEngine(encryptionMethod).Decrypt(data, passphrase, encoding);
    }

    public static byte[] Decrypt(byte[] data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return Decrypt(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    public static string DecryptAsString(byte[] data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return encoding.GetString(Decrypt(data, passphrase, encryptionMethod, encoding));
    }

    public static string DecryptAsString(byte[] data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return DecryptAsString(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    public static string DecryptAsString(string data, string passphrase, EncryptionMethod encryptionMethod, Encoding encoding)
    {
        return DecryptAsString(Convert.FromBase64String(data), passphrase, encryptionMethod, encoding);
    }

    public static string DecryptAsString(string data, string passphrase, EncryptionMethod encryptionMethod)
    {
        return DecryptAsString(data, passphrase, encryptionMethod, GetDefaultEncoding());
    }

    private static IEncryptionEngine GetEncryptionEngine(EncryptionMethod encryptionMethod)
    {
        IEncryptionEngine engine = null;

        switch (encryptionMethod)
        {
            case EncryptionMethod.TripleDES:
                engine = new TripleDESEncryptionEngine();
                break;
        }

        return engine;
    }

    private static Encoding GetDefaultEncoding()
    {
        return new System.Text.UTF8Encoding();
    }
}
