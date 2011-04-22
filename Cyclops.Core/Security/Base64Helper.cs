using System;
using System.Security.Cryptography;
using System.Text;

namespace Cyclops.Core.Security
{
    public static class CryptoHelper
    {
        /// <summary>
        /// Encode string into Base64
        /// </summary>
        public static string Base64Encode(string source)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(source));
        }

        /// <summary>
        /// Decode string from base64
        /// </summary>
        public static string Base64Decode(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.Default.GetString(data);
        }

        /// <summary>
        /// Calculate SHA1 hash from given string
        /// </summary>
        public static byte[] CalculateSha1Hash(string val)
        {
            byte[] data = Encoding.UTF8.GetBytes(val);
            SHA1 sha = new SHA1Managed();
            return sha.ComputeHash(data);
        }

        public static string CalculateMd5Hash(string val)
        {
            return BitConverter.ToString((new MD5CryptoServiceProvider()).ComputeHash(Encoding.Default.GetBytes(val)));
        }
    }
}