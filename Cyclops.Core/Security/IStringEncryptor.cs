namespace Cyclops.Core.Security
{
    public interface IStringEncryptor
    {
        string EncryptString(string plainText);
        string DecryptString(string encryptedText);
    }
}