using System.Text.Json;

namespace Lab3.Core.Contracts;

public record RegisterRequest(string AlgorithmName, string HashOfData)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static RegisterRequest FromJson(string json) => JsonSerializer.Deserialize<RegisterRequest>(json)!;
}