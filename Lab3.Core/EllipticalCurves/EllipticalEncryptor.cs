using System.Numerics;
using System.Security.Cryptography;
using Lab3.Core.Encryptions;

namespace Lab3.Core.EllipticalCurves;

public class EllipticalEncryption : IEncryption
{
    // Curve parameters (secp256r1/NIST P-256)
    private static readonly BigInteger p =
        BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853951");

    private static readonly BigInteger a =
        BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853948");

    private static readonly BigInteger b =
        BigInteger.Parse("41058363725152142129326129780047268409114441015993725554835256314039467401291");

    private static readonly Point G = new(
        BigInteger.Parse("48439561293906451759052585252797914202762949526041747995844080717082404635286"),
        BigInteger.Parse("36134250956749795798585127919587881956611106672985015071877198253568414405109"),
        a, b, p);

    public Type EncodeKeyType { get; set; } = typeof(EllipticalEncryptionKey);

    public byte[] Encrypt(byte[] input, IKey encryptionKey)
    {
        if (!(encryptionKey is EllipticalEncryptionKey eccKey))
            throw new ArgumentException("Invalid key type", nameof(encryptionKey));

        // 1. Choose random r
        var r = GenerateRandomNumber(p - 1);

        // 2. Compute R = rG, P = rYb = (x, y)
        var R = r * G;
        var P = r * eccKey.Point;

        // 3. Encrypt C ≡ (M * x) mod p
        var M = new BigInteger(input);
        if (M >= p)
            throw new ArgumentException("Message too large for curve parameters");

        var C = M * P.X % p;

        // 4. Combine R and C
        var rBytes = PointToBytes(R);
        var cBytes = C.ToByteArray();

        var result = new byte[rBytes.Length + cBytes.Length];
        Buffer.BlockCopy(rBytes, 0, result, 0, rBytes.Length);
        Buffer.BlockCopy(cBytes, 0, result, rBytes.Length, cBytes.Length);

        return result;
    }

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        if (!(decryptionKey is EllipticalEncryptionKey eccKey) || eccKey.PrivateKey == null)
            throw new ArgumentException("Invalid key type or missing private key", nameof(decryptionKey));

        // Split input into R and C
        var pointSize = p.ToByteArray().Length * 2 + 1;
        if (input.Length < pointSize)
            throw new ArgumentException("Invalid input length", nameof(input));

        var rBytes = new byte[pointSize];
        var cBytes = new byte[input.Length - pointSize];
        Buffer.BlockCopy(input, 0, rBytes, 0, pointSize);
        Buffer.BlockCopy(input, pointSize, cBytes, 0, cBytes.Length);

        var R = BytesToPoint(rBytes);
        var C = new BigInteger(cBytes);

        // 1. Compute Q = kb*R = (x, y)
        var Q = eccKey.PrivateKey.Value * R;

        // 2. Compute x^(-1) mod p
        var xInv = ModInverse(Q.X, p);

        // 3. Decrypt M ≡ (C * x^(-1)) mod p
        var M = C * xInv % p;

        return M.ToByteArray();
    }

    public (IKey encryptKey, IKey decryptKey) GenerateKeys()
    {
        var (privateKey, publicKey) = GenerateKeyPair();
        return (new EllipticalEncryptionKey(publicKey),
            new EllipticalEncryptionKey(privateKey, publicKey));
    }

    public override string ToString()
    {
        return "ECC-DH-DES Encryption (secp256r1)";
    }

    private (BigInteger privateKey, Point publicKey) GenerateKeyPair()
    {
        var privateKey = GenerateRandomNumber(p - 1);
        var publicKey = privateKey * G;
        return (privateKey, publicKey);
    }

    private static byte[] PointToBytes(Point point)
    {
        var xBytes = point.X.ToByteArray();
        var yBytes = point.Y.ToByteArray();
        var result = new byte[1 + xBytes.Length + yBytes.Length];

        result[0] = 0x04; // Uncompressed format
        Buffer.BlockCopy(xBytes, 0, result, 1, xBytes.Length);
        Buffer.BlockCopy(yBytes, 0, result, 1 + xBytes.Length, yBytes.Length);

        return result;
    }

    private static Point BytesToPoint(byte[] bytes)
    {
        if (bytes[0] != 0x04)
            throw new ArgumentException("Only uncompressed points are supported");

        var componentSize = (bytes.Length - 1) / 2;
        var xBytes = new byte[componentSize];
        var yBytes = new byte[componentSize];

        Buffer.BlockCopy(bytes, 1, xBytes, 0, componentSize);
        Buffer.BlockCopy(bytes, 1 + componentSize, yBytes, 0, componentSize);

        return new Point(
            new BigInteger(xBytes),
            new BigInteger(yBytes),
            a, b, p);
    }

    private static BigInteger GenerateRandomNumber(BigInteger max)
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

    private static BigInteger ModInverse(BigInteger a, BigInteger p)
    {
        BigInteger x, y;
        var g = ExtendedGcd(a, p, out x, out y);
        if (g != 1) throw new ArithmeticException("Inverse doesn't exist");
        return (x % p + p) % p;
    }

    private static BigInteger ExtendedGcd(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }

        BigInteger x1, y1;
        var gcd = ExtendedGcd(b % a, a, out x1, out y1);
        x = y1 - b / a * x1;
        y = x1;
        return gcd;
    }
}