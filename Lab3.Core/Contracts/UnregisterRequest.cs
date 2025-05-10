using System.Text.Json;

namespace Lab3.Core.Contracts;

public record UnregisterRequest(string Encrypted)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static UnregisterRequest FromJson(string json) => JsonSerializer.Deserialize<UnregisterRequest>(json)!;
}