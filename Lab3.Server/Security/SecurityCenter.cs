using System.Text;
using Lab3.Core.Encryptions;

namespace Lab3.Server.Security;

public static class SecurityCenter
{
    public static IKey? Register(string hash, string algorithm)
    {
        var found = EncryptionsDictionary.Instance.TryGet(algorithm, out var encryption);
        if (found is false) return null;
        var (encryptKey, decryptKey) = encryption.GenerateKeys();
        var user = new User(hash, encryption, decryptKey);
        UserTable.Instance.Add(user);
        return encryptKey;
    }

    public static bool Unregister(string encrypted)
    {
        foreach (var user in UserTable.Instance.Users)
        {
            var encryptedBytes = Encoding.UTF8.GetBytes(encrypted);
            var decryptedBytes = user.Algorithm.Decrypt(encryptedBytes, user.Key);
            var decrypted = Encoding.UTF8.GetString(decryptedBytes);
            if (decrypted.Equals(user.Hash)) return true;
        }

        return false;
    }

    public static bool IsCorrectSign(string encrypted)
    {
        foreach (var user in UserTable.Instance.Users)
        {
            var encryptedBytes = Encoding.UTF8.GetBytes(encrypted);
            var decryptedBytes = user.Algorithm.Decrypt(encryptedBytes, user.Key);
            var decrypted = Encoding.UTF8.GetString(decryptedBytes);
            if (decrypted.Equals(user.Hash)) return true;
        }

        return false;
    }
}