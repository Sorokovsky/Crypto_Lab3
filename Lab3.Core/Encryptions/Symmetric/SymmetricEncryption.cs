namespace Lab3.Core.Encryptions.Symmetric;

public partial class SymmetricEncryption : IEncryption
{
    public SymmetricEncryption(int sizeOfBlock = 16, int shiftKey = 2, int quantityOfRounds = 16)
    {
        _sizeOfBlock = sizeOfBlock;
        _shiftKey = shiftKey;
        _quantityOfRounds = quantityOfRounds;
    }

    public byte[] Encrypt(byte[] input, IKey encryptionKey)
    {
        if (encryptionKey is not SymmetricKey symmetricKey) return [];
        var key = symmetricKey.Bytes;
        if (key.Length <= 0) return [];
        ReadAndProcessBytes(input);
        key = CorrectKeyWord(key, _sizeOfBlock / 2);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++)
                _blocks[i] = EncodeDesOneRound(_blocks[i], key);
            key = KeyToNextRound(key);
        }

        var result = GenerateOutputBytes();
        return result;
    }

    public (IKey encryptKey, IKey decryptKey) GenerateKeys()
    {
        var generator = new PrimeNumberGenerator();
        var encodeKey = Convertor.UlongTyByte((long)generator.Generate());
        var keys = (new SymmetricKey([]), new SymmetricKey([]));
        if (encodeKey.Length == 0) return keys;
        var key = CorrectKeyWord(encodeKey, _sizeOfBlock / 2);

        for (var i = 0; i < _quantityOfRounds; i++)
        {
            key = KeyToNextRound(key);
        }

        return (new SymmetricKey(encodeKey), new SymmetricKey(KeyToPreviousRound(key)));
    }

    public Type EncodeKeyType { get; set; } = typeof(SymmetricKey);

    public byte[] Decrypt(byte[] input, IKey decryptionKey)
    {
        if (decryptionKey is not SymmetricKey symmetricKey) return [];
        var decodeKey = symmetricKey.Bytes;
        if (decodeKey.Length <= 0) return [];
        ReadAndProcessBytes(input);
        decodeKey = CorrectKeyWord(decodeKey, _sizeOfBlock / 2);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++) _blocks[i] = DecodeDesOneRound(_blocks[i], decodeKey);
            decodeKey = KeyToPreviousRound(decodeKey);
        }

        var result = GenerateOutputBytes();
        return result;
    }

    private static byte[] Xor(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
        for (var i = 0; i < first.Length; i++) result[i] = (byte)(first[i] ^ second[i]);

        return result;
    }

    private static byte[] F(byte[] first, byte[] second)
    {
        var sBox = new SBox();
        var xored = Xor(first, second);
        var length = Math.Min(first.Length, second.Length);
        var result = new byte[first.Length];
        for (var i = 0; i < length; i++)
        {
            var fromSBox = sBox.Substitute(xored[i]);

            result[i] = (byte)(((fromSBox << 1) | (fromSBox >> 3)) & ((fromSBox << 2) | (fromSBox >> 4)));
        }

        return result;
    }
}