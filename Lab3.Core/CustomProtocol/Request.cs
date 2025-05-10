using System.Text.Json;
using Lab3.Core.Contracts;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.CustomProtocol;

public record Request(MessageType Type, string Value)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static Request FromJson(string json)
    {
        return JsonSerializer.Deserialize<Request>(json, BigIntegerConvertor.Options)!;
    }
}