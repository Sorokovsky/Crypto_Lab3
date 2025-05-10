using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab3.Core.Encryptions.Symmetric;

public class SymmetricKey : IKey
{
    [JsonConstructor]
    public SymmetricKey(byte[] bytes)
    {
        Bytes = bytes;
    }

    public SymmetricKey()
    {
    }

    public byte[] Bytes { get; private set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public IKey FromJson(string json)
    {
        return JsonSerializer.Deserialize<SymmetricKey>(json)!;
    }
}