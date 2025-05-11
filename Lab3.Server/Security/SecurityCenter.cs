using System.Text;
using Lab3.Core.Encryptions;

namespace Lab3.Server.Security;

public static class SecurityCenter
{
    public static IKey? Register(string hash, string algorithm)
    {
        var found = EncryptionsDictionary.Instance.TryGet(algorithm, out var encryption);
        if (found is false) return null;
        var (encryptKey, decryptKey) = GenerateKeys(hash, encryption);
        var user = new User(hash, encryption, decryptKey);
        UserTable.Instance.Add(user);
        return encryptKey;
    }

    public static bool Unregister(string encrypted)
    {
        foreach (var user in UserTable.Instance.Users)
        {
            if (!IsCorrectUserAndSign(user, encrypted)) continue;
            UserTable.Instance.Remove(user.Hash);
            return true;
        }

        return false;
    }

    public static bool IsCorrectSign(string encrypted)
    {
        return UserTable.Instance.Users.Any(user => IsCorrectUserAndSign(user, encrypted));
    }

    private static bool IsCorrectUserAndSign(User user, string encrypted)
    {
        return Decrypt(user, encrypted).Equals(user.Hash);
    }

    private static string Decrypt(User user, string encrypted)
    {
        var encryptedBytes = Convert.FromBase64String(encrypted);
        var decryptedBytes = user.Algorithm.Decrypt(encryptedBytes, user.Key);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private static (IKey encryptKey, IKey decryptKey) GenerateKeys(string hash, IEncryption encryption)
    {
        try
        {
            var (encryptKey, decryptKey) = encryption.GenerateKeys();
            var encryptedBytes = encryption.Encrypt(Convert.FromBase64String(hash), encryptKey);
            encryption.Decrypt(encryptedBytes, decryptKey);
            return (encryptKey, decryptKey);
        }
        catch (Exception)
        {
            return GenerateKeys(hash, encryption);
        }
    }
}