using System.Collections.Concurrent;
using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;

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
            var result = encryption.Decrypt(Encoding.UTF8.GetBytes(user.Key),
                BitConverter.GetBytes(user.Value));
            var decryptedText = Encoding.UTF8.GetString(result);
            if (decryptedText.StartsWith(value)) found = true;
        }

        return found;
    }

    public long GetByValue(string value)
    {
        return _users[value];
    }

    public void Register(string value, long keys)
    {
        _users.TryAdd(value, keys);
    }
}