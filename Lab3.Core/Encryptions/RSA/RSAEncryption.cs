using System.Numerics;
using Lab3.Core.Utils;

namespace Lab3.Core.Encryptions.RSA;

public class RsaEncryption : IEncryption
{
    public Type EncodeKeyType { get; set; } = typeof(RsaEncryptionKey);

    public byte[] Encrypt(byte[] input, IKey encryptionKey)
    {
        if (encryptionKey is not RsaEncryptionKey rsaEncryptionKey)
            return [];

        var e = rsaEncryptionKey.E;
        var n = rsaEncryptionKey.N;
        var result = new List<byte>();

        foreach (var plainByte in input)
        {
            var message = new BigInteger(new[] { plainByte });
            var encrypted = BigInteger.ModPow(message, e, n);
            var cBytes = encrypted.ToByteArray();

            result.Add((byte)cBytes.Length);
            result.AddRange(cBytes);
        }

        return result.ToArray();
    }

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        var result = new List<byte>();
        if (decryptionKey is not RsaDecryptKey rsaDecryptKey) return result.ToArray();

        var d = rsaDecryptKey.D;
        var n = rsaDecryptKey.N;

        var i = 0;
        while (i < input.Length)
        {
            int length = input[i];
            i++;
            var encryptedBytes = input.Skip(i).Take(length).ToArray();
            i += length;

            var encrypted = new BigInteger(encryptedBytes);
            var message = BigInteger.ModPow(encrypted, d, n);

            var messagesBytes = message.ToByteArray();
            var originalByte = messagesBytes.Length > 0 ? messagesBytes[0] : (byte)0;
            result.Add(originalByte);
        }

        return result.ToArray();
    }


    public (IKey encryptKey, IKey decryptKey) GenerateKeys()
    {
        var generator = new PrimeNumberGenerator();
        BigInteger p = generator.Generate();
        while (p < 256) p = generator.Generate();

        BigInteger q = generator.Generate();
        while (q < 256 || q == p)
        {
            q = generator.Generate();
        }

        var n = p * q;
        var euler = (p - 1) * (q - 1);

        BigInteger e = generator.Generate();
        while (BigInteger.GreatestCommonDivisor(e, euler) != 1)
        {
            e = generator.Generate();
            if (e >= euler)
                e = generator.Generate();
        }

        var d = BigIntMath.ModInverse(e, euler);

        return (new RsaEncryptionKey(n, e), new RsaDecryptKey(n, d));
    }

    public override string ToString()
    {
        return "RSA";
    }
}