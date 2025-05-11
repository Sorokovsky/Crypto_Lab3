using System.Text.Json;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.Contracts;

public record RegisterRequest(string AlgorithmName, string HashOfData)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static RegisterRequest FromJson(string json)
    {
        return JsonSerializer.Deserialize<RegisterRequest>(json, BigIntegerConvertor.Options)!;
    }
}