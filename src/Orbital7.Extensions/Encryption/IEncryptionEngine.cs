namespace Orbital7.Extensions.Encryption;

public interface IEncryptionEngine
{
    byte[] Encrypt(byte[] data, string passphrase);
    byte[] Encrypt(byte[] data, string passphrase, Encoding encoding);

    byte[] Decrypt(byte[] data, string passphrase);
    byte[] Decrypt(byte[] data, string passphrase, Encoding encoding);
}
