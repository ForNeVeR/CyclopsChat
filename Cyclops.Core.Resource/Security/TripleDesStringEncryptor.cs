using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cyclops.Core.Security;

namespace Cyclops.Core.Resource.Security
{
    public class TripleDesStringEncryptor : IStringEncryptor
    {
        private readonly byte[] iv;
        private readonly byte[] key;
        private readonly TripleDESCryptoServiceProvider provider;

        public TripleDesStringEncryptor()
        {
            key = Encoding.ASCII.GetBytes("ASYAHAGCBDUUADIADKOPAAAW");
            iv = Encoding.ASCII.GetBytes("SSAZBGAW");
            provider = new TripleDESCryptoServiceProvider();
        }

        #region IStringEncryptor Members

        public string EncryptString(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Transform(plainBytes, provider.CreateEncryptor(key, iv)));
        }

        public string DecryptString(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Transform(encryptedBytes, provider.CreateDecryptor(key, iv)));
        }

        #endregion

        private static byte[] Transform(byte[] bytes, ICryptoTransform transform)
        {
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                    cryptoStream.FlushFinalBlock();

                    return stream.ToArray();
                }
            }
        }
    }
}
