namespace Lab3.Server.Security;

public class UserTable
{
    private static UserTable? _instance;

    private readonly List<User> _users = [];

    public IReadOnlyList<User> Users => _users;

    public static UserTable Instance => GetInstance();

    public bool TryGet(string hash, out User user)
    {
        var candidate = _users.FirstOrDefault(u => u.Hash == hash);
        user = candidate!;
        return candidate is not null;
    }

    public void Add(User user)
    {
        _users.Add(user);
    }

    private static UserTable GetInstance()
    {
        if (_instance is null) _instance = new UserTable();
        return _instance;
    }
}