namespace Lab3.Server;

public class SecurityContext
{
    private static SecurityContext? _instance;
    private readonly UserTable _userTable = new();

    public static SecurityContext Instance => GetInstance();

    public bool Contains(string value)
    {
        return _userTable.Contains(value);
    }

    public void Register(string value, string decodeKey)
    {
        _userTable.Register(value, decodeKey);
    }

    private static SecurityContext GetInstance()
    {
        if (_instance is null) _instance = new SecurityContext();
        return _instance;
    }
}