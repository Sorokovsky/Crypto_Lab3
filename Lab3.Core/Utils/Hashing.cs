using System.Security.Cryptography;
using System.Text;

namespace Lab3.Core.Utils;

public static class Hashing
{
    public static string GetHash(string input)
    {
        input = input.Replace("\uFEFF", "");
        var bytes = Encoding.UTF8.GetBytes(input);
        var byteHash = SHA256.HashData(bytes);
        var builder = new StringBuilder();
        foreach (var b in byteHash)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}