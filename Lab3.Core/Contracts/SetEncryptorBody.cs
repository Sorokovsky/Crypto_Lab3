using System.Text.Json;

namespace Lab3.Core.Contracts;

public record SetEncryptorBody(string Hash, string AlgorithmName)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static SetEncryptorBody FromJson(string json) => JsonSerializer.Deserialize<SetEncryptorBody>(json)!;
}