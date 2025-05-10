using System.Numerics;
using System.Security.Cryptography;

namespace Lab3.Core.EllipticalCurves;

public static class DiffiHelmanTools
{
    public static (BigInteger privateKey, Point publicKey) GenerateKeys(BigInteger a, BigInteger b, BigInteger p,
        Point basePoint, BigInteger n)
    {
        var privateKey = GenerateRandomPrivateKey(n);

        // Обчислюємо публічний ключ Q = d*G
        var publicKey = privateKey * basePoint;

        return (privateKey, publicKey);
    }

    private static BigInteger GenerateRandomPrivateKey(BigInteger n)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[n.ToByteArray().LongLength];
        BigInteger privateKey;

        do
        {
            rng.GetBytes(bytes);
            privateKey = new BigInteger(bytes) % (n - 1) + 1;
        } while (privateKey <= 0 || privateKey >= n);

        return privateKey;
    }
}