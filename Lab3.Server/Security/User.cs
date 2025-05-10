using Lab3.Core.Encryptions;

namespace Lab3.Server.Security;

public class User
{
    public User(string hash, IEncryption algorithm, IKey key)
    {
        Hash = hash;
        Algorithm = algorithm;
        Key = key;
    }

    public string Hash { get; private set; }

    public IEncryption Algorithm { get; set; }

    public IKey Key { get; set; }
}