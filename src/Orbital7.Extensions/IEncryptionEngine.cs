using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions
{
    public interface IEncryptionEngine
    {
        byte[] Encrypt(byte[] data, string passphrase);
        byte[] Encrypt(byte[] data, string passphrase, Encoding encoding);

        byte[] Decrypt(byte[] data, string passphrase);
        byte[] Decrypt(byte[] data, string passphrase, Encoding encoding);
    }
}
