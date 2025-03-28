namespace Lab3.Core.Encryptions;

public partial class SymmetricEncryption
{
    public SymmetricEncryption(int sizeOfBlock = 16, int shiftKey = 2, int quantityOfRounds = 16)
    {
        _sizeOfBlock = sizeOfBlock;
        _shiftKey = shiftKey;
        _quantityOfRounds = quantityOfRounds;
    }

    public byte[] EncryptFile(string inputFile, string outputFile, byte[] key)
    {
        if (key.Length <= 0) return [];
        ReadAndProcessBytes(inputFile);
        key = CorrectKeyWord(key, _sizeOfBlock / 2);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++)
                _blocks[i] = EncodeDesOneRound(_blocks[i], key);
            key = KeyToNextRound(key);
        }

        var decodedKey = KeyToPreviousRound(key);
        var result = GenerateOutputBytes();
        ByteFilesService.WriteBytes(outputFile, result, result.Length);
        return decodedKey;
    }

    public void DecryptFile(string inputFile, string outputFile, byte[] decodeKey)
    {
        if (decodeKey.Length <= 0) return;
        ReadAndProcessBytes(inputFile);
        for (var j = 0; j < _quantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++) _blocks[i] = DecodeDesOneRound(_blocks[i], decodeKey);
            decodeKey = KeyToPreviousRound(decodeKey);
        }

        var result = GenerateOutputBytes();
        ByteFilesService.WriteBytes(outputFile, result, result.Length - _addOfByte);
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