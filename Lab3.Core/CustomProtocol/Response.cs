using System.Text.Json;
using Lab3.Core.Contracts;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.CustomProtocol;

public record Response(ResponseStatus Status, object Value)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static Response FromJson(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<Response>(json, BigIntegerConvertor.Options)!;
        }
        catch (Exception)
        {
            return JsonSerializer.Deserialize<Response>(json.Trim('\0'), BigIntegerConvertor.Options)!;
        }
    }
}