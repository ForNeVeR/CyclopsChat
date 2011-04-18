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
            return Base64Helper.Encode(Transform(plainText, provider.CreateEncryptor(key, iv)));
        }

        public string DecryptString(string encryptedText)
        {
            return Transform(Base64Helper.Decode(encryptedText), provider.CreateDecryptor(key, iv));
        }

        #endregion

        private static string Transform(string text, ICryptoTransform transform)
        {
            if (text == null)
                return null;
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    byte[] input = Encoding.Default.GetBytes(text);
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();

                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
        }
    }
}