using System.Numerics;
using System.Text.Json;
using Lab3.Core.EllipticalCurves;
using Lab3.Core.Encryptions;
using Lab3.Core.Encryptions.RSA;

public class EllipticalEncryptionKey : IKey
{
    public Point PublicKey { get; set; }
    public BigInteger PrivateKey { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<EllipticalEncryptionKey>(json, BigIntegerConvertor.Options)!;
    }
}