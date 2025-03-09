namespace Lab3.Core;

public class SymmetricEncryptionByte
{
    private const int SizeOfBlock = 16;
    private static int _addOfByte;
    private const int ShiftKey = 2;
    private const int QuantityOfRounds = 16;
    private byte[][] _blocks = [];

    public byte[] EncryptFile(string inputFile, string outputFile, byte[] key, out int addByte)
    {
        if (key.Length > 0)
        {
            var array = ReadBytes(inputFile);
            array = ByteToRightLength(array);
            _blocks = CutByteIntoBlocks(array);
            key = CorrectKeyWord(key, SizeOfBlock / 2);
            for (var j = 0; j < QuantityOfRounds; j++)
            {
                for (var i = 0; i < _blocks.Length; i++)
                    _blocks[i] = EncodeDesOneRound(_blocks[i], key);
                key = KeyToNextRound(key);
            }

            var decodedKey = KeyToPreviousRound(key);
            var result = new byte[_blocks.Length * _blocks[0].Length];
            for (var i = 0; i < _blocks.Length; i++)
            {
                for (var j = 0; j < _blocks[i].Length; j++)
                {
                    result[i * _blocks[i].Length + j] = _blocks[i][j];
                }
            }
            WriteBytes(outputFile, result, result.Length);
            addByte = _addOfByte;
            return decodedKey;
        }

        addByte = _addOfByte;
        return [];
    }

    private static void WriteBytes(string outputFile, byte[] result, int length)
    {
        using var writer = new FileStream(outputFile, FileMode.OpenOrCreate);
        writer.Write(result, 0, length);
    }

    private static byte[] ReadBytes(string inputFile)
    {
        using var reader = File.OpenRead(inputFile);
        var array = new byte[reader.Length];
        reader.ReadExactly(array, 0, array.Length);
        return array;
    }

    public void DecryptFile(string inputFile, string outputFile, byte[] decodeKey, int addByte)
    {
        if (decodeKey.Length <= 0) return;
        var array = ReadBytes(inputFile);
        _blocks = CutByteIntoBlocks(array);
        for (var j = 0; j < QuantityOfRounds; j++)
        {
            for (var i = 0; i < _blocks.Length; i++)
            {
                _blocks[i] = DecodeDesOneRound(_blocks[i], decodeKey);
            }
            decodeKey = KeyToPreviousRound(decodeKey);
        }
        var result = new byte[_blocks.Length * _blocks[0].Length];
        for(var i = 0; i < _blocks.Length; i++) 
        for(var j = 0; j < _blocks[i].Length; j++)
            result[i * _blocks[i].Length + j] = _blocks[i][j];
        WriteBytes(outputFile, result, result.Length - addByte);
    }

    private static byte[] ByteToRightLength(byte[] input)
    {
        _addOfByte = 0;
        if ((input.Length % SizeOfBlock) == 0) return input;
        _addOfByte = SizeOfBlock - input.Length % SizeOfBlock;
        Array.Resize(ref input, input.Length + _addOfByte);

        return input;
    }

    private static byte[] ByteAllToBit(byte[] input)
    {
        var bit = new byte[input.Length * 8];

        for (var k = 0; k < input.Length; k++) ByteOneToBit(input[k], k);
        return bit;

        void ByteOneToBit(byte one, int k)
        {
            for (var i = 0; i < sizeof(byte) * 8; i++)
            {
                var index = k * 8 - i + 7;
                if ((one & (1 << i)) != 0)
                {
                    bit[index] = 1;
                }
                else
                {
                    bit[index] = 0;
                }
            }
        }
    }

    private static byte[] BitAllToByte(byte[] input)
    {
        var bytes = new byte[input.Length / (sizeof(byte) * 8)];
        var temp = new byte[sizeof(byte) * 8];
        for (var k = 0; k < input.Length / 8; k++)
        {
            Array.Copy(input, k * 8, temp, 0, 8);
            BitsToOneByte(temp, k);
        }

        return bytes;

        void BitsToOneByte(byte[] one, int k)
        {
            byte n = 1, b = 0;
            for (var i = sizeof(byte) * 8 - 1; i >= 0; i--, n *= 2) b += (byte)(one[i] * n);
            bytes[k] = b;
        }
    }

    private static byte[][] CutByteIntoBlocks(byte[] input)
    {
        var blocks = new byte[input.Length / SizeOfBlock][];
        var numberOfBlock = input.Length / SizeOfBlock;
        for (var i = 0; i < numberOfBlock; i++) blocks[i] = new byte[SizeOfBlock];
        for(var i = 0; i < numberOfBlock; i++)
        {
            Array.Copy(input, i * SizeOfBlock, blocks[i], 0, SizeOfBlock);
        }

        return blocks;
    }

    private static byte[] CorrectKeyWord(byte[] input, int keyLength)
    {
        Array.Resize(ref input, keyLength);
        return input;
    }

    private static byte[] Xor(byte[] first, byte[] second)
    {
        var result = new byte[first.Length];
        for (var i = 0; i < first.Length; i++)
        {
            result[i] = (byte)(first[i] ^ second[i]);
        }

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

    private static byte[] EncodeDesOneRound(byte[] input, byte[] key)
    {
        var k = input.Length / 2;
        var result = new byte[input.Length];
        var l = new byte[k];
        var r = new byte[k];
        Array.Copy(input, 0, l, 0, k);
        Array.Copy(input, k, r, 0, k);
        var temp = Xor(l, F(r, key));
        Array.Copy(r, 0, result, 0, k);
        Array.Copy(temp, 0, result, k, k);
        return result;
    }

    private static byte[] DecodeDesOneRound(byte[] input, byte[] key)
    {
        var k = input.Length / 2;
        var result = new byte[input.Length];
        var l = new byte[k];
        var r = new byte[k];
        Array.Copy(input, 0, l, 0, k);
        Array.Copy(input, k, r, 0, k);
        var temp = Xor(F(l, key), r);
        Array.Copy(temp, 0, result, 0, k);
        Array.Copy(l, 0, result, k, k);
        return result;
    }

    private static byte[] KeyToNextRound(byte[] key)
    {
        var bits = ByteAllToBit(key);
        for (var i = 0; i < ShiftKey; i++)
        {
            var first = bits[^1];
            for (var j = bits.Length - 1; j > 0; j--) bits[j] = bits[j - 1];
            bits[0] = first;
        }

        key = BitAllToByte(bits);
        return key;
    }

    private static byte[] KeyToPreviousRound(byte[] key)
    {
        var bits = ByteAllToBit(key);
        for (var i = 0; i < ShiftKey; i++)
        {
            var first = bits[0];
            for(var j = 0; j < bits.Length - 1; j++) bits[j] = bits[j + 1];
            bits[^1] = first;
        }
        key = BitAllToByte(bits);
        return key;
    }
}