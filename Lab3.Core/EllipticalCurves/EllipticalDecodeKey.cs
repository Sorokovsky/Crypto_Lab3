using System.Numerics;
using System.Text.Json;
using Lab3.Core.Encryptions;

namespace Lab3.Core.EllipticalCurves;

public class EllipticalDecodeKey : IKey
{
    public EllipticalDecodeKey(BigInteger key)
    {
        Key = key;
    }

    public EllipticalDecodeKey()
    {
    }

    public BigInteger Key { get; set; }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<EllipticalEncryptionKey>(json)!;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}