using System;
using System.Security.Cryptography;

namespace Cyclops.Core.Resource;

/// <summary>
/// Set of extension methods for System.Drawing.Image
/// </summary>
internal static class ImageUtils
{
    public static string CalculateSha1HashOfAnImage(byte[] image)
    {
        var cryptoTransformSha1 = new SHA1CryptoServiceProvider();
        string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(image)).Replace("-", "").ToLower();
        return hash;
    }
}
