using System.Numerics;
using System.Security.Cryptography;

namespace Lab3.Core.Utils;

public static class BigIntMath
{
    public static BigInteger GenerateRandomNumber(BigInteger max)
    {
        var bytes = new byte[max.ToByteArray().Length];
        using var rng = RandomNumberGenerator.Create();
        BigInteger result;
        do
        {
            rng.GetBytes(bytes);
            result = new BigInteger(bytes) % max;
        } while (result <= 0);

        return result;
    }

    public static BigInteger ModInverse(BigInteger a, BigInteger p)
    {
        var g = ExtendedGcd(a, p, out var x, out _);
        if (g != 1) throw new ArithmeticException("Inverse doesn't exist");
        return (x % p + p) % p;
    }

    public static BigInteger ExtendedGcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }

        var gcd = ExtendedGcd(b % a, a, out var x1, out var y1);
        x = y1 - b / a * x1;
        y = x1;
        return gcd;
    }
}