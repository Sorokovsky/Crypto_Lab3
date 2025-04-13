using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;

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

    public long Register(string value)
    {
        var encryption = new SymmetricEncryption();
        var generator = new PrimeNumberGenerator();
        var key = generator.Generate();
        var keyBytes = Convertor.UlongTyByte(key);
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var (encrypted, decodeKey) = encryption.EncryptFile(valueBytes, keyBytes);
        _userTable.Register(Encoding.UTF8.GetString(encrypted), BitConverter.ToInt64(decodeKey, 0));
        return key;
    }

    private static SecurityContext GetInstance()
    {
        if (_instance is null) _instance = new SecurityContext();
        return _instance;
    }
}