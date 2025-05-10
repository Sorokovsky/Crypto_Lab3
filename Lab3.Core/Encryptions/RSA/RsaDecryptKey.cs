using System.Numerics;
using System.Text.Json;

namespace Lab3.Core.Encryptions.RSA;

public class RsaDecryptKey : IKey
{
    public RsaDecryptKey(BigInteger n, BigInteger d)
    {
        N = n;
        D = d;
    }

    public BigInteger N { get; }

    public BigInteger D { get; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<RsaDecryptKey>(json, BigIntegerConvertor.Options)!;
    }
}