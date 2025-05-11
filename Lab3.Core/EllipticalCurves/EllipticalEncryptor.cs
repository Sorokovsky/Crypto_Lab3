using System.Numerics;
using Lab3.Core.Encryptions;
using Lab3.Core.Utils;

namespace Lab3.Core.EllipticalCurves;

public class EllipticalEncryption : IEncryption
{
    // Curve parameters (secp256r1/NIST P-256)
    private static readonly BigInteger P =
        BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853951");

    private static readonly BigInteger A =
        BigInteger.Parse("115792089210356248762697446949407573530086143415290314195533631308867097853948");

    private static readonly BigInteger B =
        BigInteger.Parse("41058363725152142129326129780047268409114441015993725554835256314039467401291");

    private static readonly BigInteger N =
        BigInteger.Parse("115792089210356248762697446949407573529996955224135760342422259061068512044369");

    private static readonly Point G = new(
        BigInteger.Parse("48439561293906451759052585252797914202762949526041747995844080717082404635286"),
        BigInteger.Parse("36134250956749795798585127919587881956611106672985015071877198253568414405109"),
        A, B, P);

    public Type EncodeKeyType { get; set; } = typeof(EllipticalEncryptionKey);

    public byte[] Encrypt(byte[] input, IKey encryptionKey)
    {
        if (encryptionKey is not EllipticalEncryptionKey eccKey)
            return [];
        var r = BigIntMath.GenerateRandomNumber(N - 1);
        var rg = r * G;
        var rp = r * eccKey.PublicKey;
        var m = new BigInteger(input);
        if (m >= P)
            throw new ArgumentException("Message too large for curve parameters");

        var c = m * rp.X % P;
        var rBytes = Point.PointToBytes(rg);
        var cBytes = c.ToByteArray();
        var result = new byte[rBytes.Length + cBytes.Length];
        Array.Copy(rBytes, 0, result, 0, rBytes.Length);
        Array.Copy(cBytes, 0, result, rBytes.Length, cBytes.Length);
        return result;
    }

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        if (decryptionKey is not EllipticalEncryptionKey eccKey || eccKey.PrivateKey == null)
            return [];
        var pointSize = P.ToByteArray().Length * 2 + 1;
        if (input.Length < pointSize)
            return [];
        var rBytes = new byte[pointSize];
        var cBytes = new byte[input.Length - pointSize];
        Buffer.BlockCopy(input, 0, rBytes, 0, pointSize);
        Buffer.BlockCopy(input, pointSize, cBytes, 0, cBytes.Length);

        var r = Point.BytesToPoint(rBytes, A, B, P);
        var c = new BigInteger(cBytes);
        var q = eccKey.PrivateKey * r;
        var xInv = BigIntMath.ModInverse(q.X, P);
        var m = c * xInv % P;
        return m.ToByteArray();
    }

    public (IKey encryptKey, IKey decryptKey) GenerateKeys()
    {
        var (privateKey, publicKey) = GenerateKeyPair();
        var encryptKey = new EllipticalEncryptionKey { PrivateKey = privateKey, PublicKey = publicKey };
        var decryptKey = new EllipticalDecodeKey { Key = privateKey };
        return (encryptKey, decryptKey);
    }

    public override string ToString()
    {
        return "Elliptical curves";
    }

    private static (BigInteger privateKey, Point publicKey) GenerateKeyPair()
    {
        var privateKey = BigIntMath.GenerateRandomNumber(P - 1);
        var publicKey = privateKey * G;
        return (privateKey, publicKey);
    }
}