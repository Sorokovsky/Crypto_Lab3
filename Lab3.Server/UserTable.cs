using System.Collections.Concurrent;
using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;

namespace Lab3.Server;

public class UserTable
{
    private readonly IDictionary<string, Keys> _users = new ConcurrentDictionary<string, Keys>();

    public bool Contains(string value)
    {
        var found = false;
        foreach (var user in _users)
        {
            var encryption = new SymmetricEncryption();
            var result = encryption.Decrypt(Encoding.UTF8.GetBytes(user.Key),
                BitConverter.GetBytes(user.Value.Decrypt));
            var decryptedText = Encoding.UTF8.GetString(result);
            if (decryptedText.Equals(value)) found = true;
        }

        return found;
    }

    public Keys GetByValue(string value)
    {
        return _users[value];
    }

    public void Register(string value, Keys keys)
    {
        _users.TryAdd(value, keys);
    }
}