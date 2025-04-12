using System.Collections.Concurrent;

namespace Lab3.Server;

public class UserTable
{
    private readonly IDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

    public bool Contains(string value)
    {
        return _users.ContainsKey(value);
    }

    public void Register(string value, string decodeKey)
    {
        _users.TryAdd(value, decodeKey);
    }
}