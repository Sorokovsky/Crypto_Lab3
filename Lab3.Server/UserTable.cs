using System.Collections.Concurrent;

namespace Lab3.Server;

public class UserTable
{
    private readonly IDictionary<string, long> _users = new ConcurrentDictionary<string, long>();

    public bool Contains(string value)
    {
        return _users.ContainsKey(value);
    }

    public long GetByValue(string value)
    {
        return _users[value];
    }

    public void Register(string value, long decodeKey)
    {
        _users.TryAdd(value, decodeKey);
    }
}