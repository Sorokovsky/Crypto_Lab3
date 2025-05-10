namespace Lab3.Core.Encryptions.Symmetric;

public partial class SymmetricEncryption
{
    private byte[] KeyToNextRound(byte[] key)
    {
        var bits = ByteAllToBit(key);
        for (var i = 0; i < _shiftKey; i++)
        {
            var first = bits[^1];
            for (var j = bits.Length - 1; j > 0; j--) bits[j] = bits[j - 1];
            bits[0] = first;
        }

        key = BitAllToByte(bits);
        return key;
    }

    private byte[] KeyToPreviousRound(byte[] key)
    {
        var bits = ByteAllToBit(key);
        for (var i = 0; i < _shiftKey; i++)
        {
            var first = bits[0];
            for (var j = 0; j < bits.Length - 1; j++) bits[j] = bits[j + 1];
            bits[^1] = first;
        }

        key = BitAllToByte(bits);
        return key;
    }
}