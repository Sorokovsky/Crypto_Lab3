using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lab3.Core.Encryptions;

namespace Lab3.Core.EllipticalCurves;

public class EllipticalEncryptionKey : IKey
{
    [JsonConstructor]
    public EllipticalEncryptionKey(Point point)
    {
        Point = point;
    }

    [JsonConstructor]
    public EllipticalEncryptionKey(BigInteger privateKey, Point publicKey)
    {
        PrivateKey = privateKey;
        Point = publicKey;
    }

    public EllipticalEncryptionKey()
    {
    }

    public Point Point { get; set; }
    public BigInteger? PrivateKey { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<EllipticalEncryptionKey>(json)!;
    }
}