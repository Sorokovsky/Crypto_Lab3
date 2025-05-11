using System.Text.Json;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.Contracts;

public record CheckRequest(string Encrypted)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static CheckRequest FromJson(string json)
    {
        return JsonSerializer.Deserialize<CheckRequest>(json, BigIntegerConvertor.Options)!;
    }
}