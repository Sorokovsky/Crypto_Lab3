using System.Text.Json;

namespace Lab3.Core.Contracts;

public record CheckRequest(string Encrypted)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static CheckRequest FromJson(string json) => JsonSerializer.Deserialize<CheckRequest>(json)!;
}