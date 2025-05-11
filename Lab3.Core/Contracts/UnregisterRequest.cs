using System.Text.Json;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.Contracts;

public record UnregisterRequest(string Encrypted)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static UnregisterRequest FromJson(string json)
    {
        return JsonSerializer.Deserialize<UnregisterRequest>(json, BigIntegerConvertor.Options)!;
    }
}