using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab3.Core.Encryptions.RSA;

public class RsaEncryptionKey : IKey
{
    [JsonConstructor]
    public RsaEncryptionKey(BigInteger n, BigInteger e)
    {
        N = n;
        E = e;
    }

    public RsaEncryptionKey()
    {
    }

    public BigInteger N { get; private set; }

    public BigInteger E { get; private set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<RsaEncryptionKey>(json, BigIntegerConvertor.Options)!;
    }
}