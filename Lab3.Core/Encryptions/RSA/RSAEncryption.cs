using System.Numerics;

namespace Lab3.Core.Encryptions.RSA;

public class RsaEncryption
{

    public static BigInteger GenerateKey(ulong p, ulong q, out BigInteger d, out BigInteger e)
    {
        e = 0;
        BigInteger n = p * q;
        BigInteger fuel = (p - 1) * (q - 1);
        d = fuel - 1;
        for (BigInteger i = 2; i <= fuel; i++)
        {
            if ((fuel % i == 0) && (d % i == 0))
            {
                d--;
                i = 1;
            }

            e = 10;
            while (true)
            {
                if ((e * d) % fuel == 1) break;
                e++;
            }
        }
        return n;
    }

    private static ulong PowRsa(ulong byt, BigInteger de, BigInteger n)
    {
        BigInteger st = 1;
        for (BigInteger i = 0; i < de; i++)
        {
            st *= byt;
        }

        st %= n;
        return (ulong)st;
    }

    public static byte[] Encrypt(byte[] input, BigInteger d, BigInteger n)
    {
        RSApkde pkde;
        pkde.RSAByte1 = 0;
        pkde.RSAByte2 = 0;
        pkde.RSAByte3 = 0;
        pkde.RSAByte4 = 0;
        var result = new byte[input.Length * 4];
        for (var i = 0; i < input.Length; i++)
        {
            pkde.RSAInt = PowRsa(input[i], d, n);
            result[i * 4] = pkde.RSAByte1;
            result[i * 4 + 1] = pkde.RSAByte2;
            result[i * 4 + 2] = pkde.RSAByte3;
            result[i * 4 + 3] = pkde.RSAByte4;
        }
        return result;
    }

    public static byte[] Decrypt(byte[] input, BigInteger e, BigInteger n)
    {
        RSApkde pkde;
        pkde.RSAInt = 0;
        pkde.RSAByte1 = 0;
        pkde.RSAByte2 = 0;
        pkde.RSAByte3 = 0;
        pkde.RSAByte4 = 0;
        var result = new byte[input.Length / 4];
        for (var i = 0; i < result.Length; i++)
        {
            pkde.RSAByte1 = input[i * 4 + 0];
            pkde.RSAByte2 = input[i * 4 + 1];
            pkde.RSAByte3 = input[i * 4 + 2];
            pkde.RSAByte4 = input[i * 4 + 3];
            pkde.RSAInt = PowRsa(pkde.RSAInt, e, n);
            result[i] = pkde.RSAByte1;
        }
        return result;
    }
}