using System.Numerics;
using System.Security.Cryptography;
using Lab3.Core.Encryptions;
using Lab3.Core.Utils;

namespace Lab3.Core.EllipticalCurves;

public class EllipticalEncryption : IEncryption
{
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
            throw new ArgumentException("Invalid key type", nameof(encryptionKey));
        var r = GenerateRandomScalar();
        var R = r * G;
        var sharedPoint = r * eccKey.PublicKey;
        var rBytes = Point.PointToBytes(R);
        var result = new List<byte>(rBytes);
        var chunkSize = 32;
        for (var i = 0; i < input.Length; i += chunkSize)
        {
            var currentChunkSize = Math.Min(chunkSize, input.Length - i);
            var chunk = new byte[currentChunkSize];
            Array.Copy(input, i, chunk, 0, currentChunkSize);
            var m = new BigInteger(chunk, true, true);
            if (m >= P)
                m = new BigInteger(chunk, true, true) % P;

            var c = m * sharedPoint.X % P;
            result.AddRange(c.ToByteArray(true, true));
        }

        return result.ToArray();
    }

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        if (decryptionKey is not EllipticalDecodeKey eccKey || eccKey.Key == null)
            throw new ArgumentException("Invalid key type", nameof(decryptionKey));
        var pointSize = 65;
        if (input.Length < pointSize)
            throw new ArgumentException("Invalid input", nameof(input));

        var rBytes = new byte[pointSize];
        Array.Copy(input, 0, rBytes, 0, pointSize);
        var R = Point.BytesToPoint(rBytes, A, B, P);
        var sharedPoint = eccKey.Key * R;
        var xInv = BigIntMath.ModInverse(sharedPoint.X, P);
        var result = new List<byte>();
        for (var i = pointSize; i < input.Length; i += 32)
        {
            var chunkSize = Math.Min(32, input.Length - i);
            var chunk = new byte[chunkSize];
            Array.Copy(input, i, chunk, 0, chunkSize);

            var c = new BigInteger(chunk, true, true);
            var m = c * xInv % P;
            result.AddRange(m.ToByteArray(true, true));
        }
        while (result.Count > 0 && result[result.Count - 1] == 0)
            result.RemoveAt(result.Count - 1);

        return result.ToArray();
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

    private static BigInteger GenerateRandomScalar()
    {
        // Розмір ключа в байтах (256 біт для secp256r1)
        const int keySizeBytes = 32;
        var randomBytes = new byte[keySizeBytes];

        using (var rng = RandomNumberGenerator.Create())
        {
            // Генеруємо випадкові байти
            rng.GetBytes(randomBytes);
        }
        var scalar = new BigInteger(randomBytes, true, true);
        scalar = scalar % N;

        // Переконуємося, що скаляр знаходиться в правильному діапазоні [1, N-1]
        if (scalar == 0)
            // Якщо випадково отримали 0, генеруємо знову
            return GenerateRandomScalar();

        return scalar;
    }
}