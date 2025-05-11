using System.Text.Json;
using Lab3.Core.Contracts;
using Lab3.Core.Encryptions.RSA;

namespace Lab3.Core.CustomProtocol;

public record Response(ResponseStatus Status, string Value)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, BigIntegerConvertor.Options);
    }

    public static Response FromJson(string json)
    {
        json = FixCorruptedJson(json);
        try
        {
            return JsonSerializer.Deserialize<Response>(json, BigIntegerConvertor.Options)!;
        }
        catch (Exception)
        {
            return JsonSerializer.Deserialize<Response>(json.Trim('\0'), BigIntegerConvertor.Options)!;
        }
    }

    private static string FixCorruptedJson(string input)
    {
        var openBraces = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '{') openBraces++;
            else if (input[i] == '}') openBraces--;

            if (openBraces == 0)
                return input.Substring(0, i + 1); // до кінця валідного JSON
        }

        return input; // або кинути виняток
    }
}