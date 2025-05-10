using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab3.Core.Encryptions.RSA;

public class BigIntegerConvertor : JsonConverter<BigInteger>
{
    public static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new BigIntegerConvertor() },
        WriteIndented = false
    };

    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return BigInteger.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}