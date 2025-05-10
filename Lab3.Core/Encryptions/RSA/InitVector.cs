using System.Text.Json;

namespace Lab3.Core.Encryptions.RSA;

public record InitVector(ulong P, ulong Q)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static InitVector FromJson(string json) => JsonSerializer.Deserialize<InitVector>(json)!;
}