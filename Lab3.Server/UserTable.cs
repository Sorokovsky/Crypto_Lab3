using System.Collections.Concurrent;
using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;
using SymmetricEncryption = Lab3.Core.Encryptions.Symmetric.SymmetricEncryption;

namespace Lab3.Server;

public class UserTable
{
    private readonly IDictionary<string, long> _users = new ConcurrentDictionary<string, long>();

    public bool Contains(string value)
    {
        var found = false;
        foreach (var user in _users)
        {
            var encryption = new SymmetricEncryption();
            var (result, _) = encryption.EncryptFile(Encoding.UTF8.GetBytes(value),
                Convertor.UlongTyByte(user.Value));
            var encrypted = Encoding.UTF8.GetString(result);
            
            var hashed = Hashing.GetHash(encrypted);
            if (hashed.StartsWith(user.Key)) found = true;
        }

        return found;
    }

    public void Register(string value, long keys)
    {
        _users.TryAdd(value, keys);
    }
}