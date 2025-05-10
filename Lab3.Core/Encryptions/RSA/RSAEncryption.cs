using System.Numerics;

namespace Lab3.Core.Encryptions.RSA;

public class RsaEncryption : IEncryption
{
    public (IKey encryptKey, IKey decryptKey) GenerateKeys()
    {
        var generator = new PrimeNumberGenerator();
        var p = generator.Generate();
        var q = generator.Generate();
        while (p == q)
        {
            q = generator.Generate();
        }

        BigInteger n = p * q;
        BigInteger eulerOfN = (p - 1) * (q - 1);
        BigInteger e = generator.Generate();
        while (BigInteger.GreatestCommonDivisor(e, eulerOfN) != 1)
        {
            e = generator.Generate();
        }

        BigInteger d = ModInverse(e, eulerOfN);
        return (new RsaEncryptionKey(n, e), new RsaDecryptKey(n, d));
    }

    public Type EncodeKeyType { get; set; } = typeof(RsaEncryptionKey);

    public byte[] Encrypt(byte[] input, IKey encryptionKey)
    {
        List<byte> result = new List<byte>();
        if (encryptionKey is not RsaEncryptionKey rsaEncryptionKey) return result.ToArray();

        BigInteger e = rsaEncryptionKey.E;
        BigInteger n = rsaEncryptionKey.N;
        int blockSize = n.ToByteArray().Length; // розмір блоку на основі n

        // Перевірка розміру повідомлення
        if (input.Length > blockSize - 11) // Для великих повідомлень потрібно розбивати на блоки
        {
            Console.WriteLine("Повідомлення занадто велике для цього ключа. Розбиваємо на блоки.");
            int blockCount = (int)Math.Ceiling((double)input.Length / (blockSize - 11));
            for (int i = 0; i < blockCount; i++)
            {
                var block = new byte[blockSize - 11];
                Array.Copy(input, i * (blockSize - 11), block, 0,
                    Math.Min(block.Length, input.Length - i * (blockSize - 11)));
                var encryptedBlock = EncryptBlock(block, rsaEncryptionKey);
                result.AddRange(encryptedBlock); // Додаємо зашифрований блок
            }
        }
        else
        {
            var paddedInput = AddPKCS1Padding(input, blockSize); // Використовуємо паддинг для маленьких повідомлень
            var encryptedBlock = EncryptBlock(paddedInput, rsaEncryptionKey);
            result.AddRange(encryptedBlock); // Додаємо зашифроване повідомлення
        }

        return result.ToArray();
    }

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        List<byte> result = new List<byte>();
        if (decryptionKey is not RsaDecryptKey rsaDecryptKey) return result.ToArray();

        BigInteger d = rsaDecryptKey.D;
        BigInteger n = rsaDecryptKey.N;
        int blockSize = n.ToByteArray().Length;

        for (int i = 0; i < input.Length; i += blockSize)
        {
            var block = new byte[Math.Min(blockSize, input.Length - i)];
            Array.Copy(input, i, block, 0, block.Length);

            var decryptedBigInt = PowRsa(new BigInteger(block), d, n);
            byte[] decryptedBlock = decryptedBigInt.ToByteArray();
            result.AddRange(decryptedBlock);
        }

        return result.ToArray();
    }

    public override string ToString()
    {
        return "RSA";
    }

    private static BigInteger PowRsa(BigInteger byt, BigInteger de, BigInteger n)
    {
        return BigInteger.ModPow(byt, de, n);
    }

    private byte[] EncryptBlock(byte[] block, RsaEncryptionKey rsaKey)
    {
        BigInteger e = rsaKey.E;
        BigInteger n = rsaKey.N;
        var encrypted = PowRsa(new BigInteger(block), e, n).ToByteArray();
        return encrypted;
    }

    private static BigInteger ModInverse(BigInteger e, BigInteger phi)
    {
        BigInteger t = 0, newT = 1;
        BigInteger r = phi, newR = e;

        while (newR != 0)
        {
            BigInteger quotient = r / newR;
            (t, newT) = (newT, t - quotient * newT);
            (r, newR) = (newR, r - quotient * newR);
        }

        if (r > 1) throw new Exception("e не має оберненого по модулю φ(n)");
        if (t < 0) t += phi;

        return t;
    }

    private static byte[] AddPKCS1Padding(byte[] input, int blockSize)
    {
        int paddingLength = blockSize - input.Length - 3; // 3 байти для 0x00, 0x02 та випадкові байти
        if (paddingLength < 8)
        {
            // Якщо повідомлення надто велике для цього ключа, розбиваємо на блоки
            throw new Exception("Повідомлення занадто велике для цього ключа.");
        }

        var paddedInput = new List<byte>();
        paddedInput.Add(0x00); // Стартовий байт для PKCS#1
        paddedInput.Add(0x02); // Тип паддингу (0x02 для випадкових байтів)

        // Додаємо випадкові байти
        var random = new Random();
        for (int i = 0; i < paddingLength; i++)
        {
            paddedInput.Add((byte)random.Next(1, 256));
        }

        paddedInput.Add(0x00); // Роздільник

        // Додаємо основне повідомлення
        paddedInput.AddRange(input);

        return paddedInput.ToArray();
    }
}