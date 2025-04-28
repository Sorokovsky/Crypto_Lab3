namespace Lab3.Core.Encryptions.Symmetric;

public partial class SymmetricEncryption
{
    public SymmetricEncryption(int sizeOfBlock = 16, int shiftKey = 2, int quantityOfRounds = 16)
    {
        _sizeOfBlock = sizeOfBlock;
        _shiftKey = shiftKey;
        _quantityOfRounds = quantityOfRounds;
    }

    public (byte[], byte[]) EncryptFile(byte[] input, byte[] key)
    {
        if (key.Length <= 0) return ([], []);
        ReadAndProcessBytes(input);
        key = SymmetricEncryption.CorrectKeyWord(key, _sizeOfBlock / 2);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++)
                _blocks[i] = SymmetricEncryption.EncodeDesOneRound(_blocks[i], key);
            key = KeyToNextRound(key);
        }

        var decodedKey = KeyToPreviousRound(key);
        var result = GenerateOutputBytes();
        return (result, decodedKey);
    }

    public byte[] Decrypt(byte[] input, byte[] decodeKey)
    {
        if (decodeKey.Length <= 0) return [];
        ReadAndProcessBytes(input);
        decodeKey = SymmetricEncryption.CorrectKeyWord(decodeKey, _sizeOfBlock / 2);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++) _blocks[i] = SymmetricEncryption.DecodeDesOneRound(_blocks[i], decodeKey);
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