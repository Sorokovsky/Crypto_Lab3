using System.Numerics;

namespace Lab3.Core.Utils;

public static class BigIntMath
{
    public static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        if (m == 1)
            return 0;

        var m0 = m;
        BigInteger y = 0, x = 1;

        while (a > 1)
        {
            var q = a / m;
            var t = m;
            m = a % m;
            a = t;
            t = y;
            y = x - q * y;
            x = t;
        }

        if (x < 0)
            x += m0;

        return x;
    }
}