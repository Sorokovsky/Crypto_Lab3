using System.Text.Json;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.Contracts;

public record SetEncryptorBody(string Hash, string AlgorithmName)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static SetEncryptorBody FromJson(string json)
    {
        return JsonSerializer.Deserialize<SetEncryptorBody>(json, BigIntegerConvertor.Options)!;
    }
}