namespace Lab3.Core.Encryptions;

public partial class SymmetricEncryption
{
    private byte[] GenerateOutputBytes()
    {
        var result = new byte[_blocks.Length * _blocks[0].Length];
        for (var i = 0; i < _blocks.Length; i++)
        for (var j = 0; j < _blocks[i].Length; j++)
            result[i * _blocks[i].Length + j] = _blocks[i][j];

        return result;
    }

    private void ReadAndProcessBytes(string inputFile)
    {
        var array = ByteFilesService.ReadBytes(inputFile);
        array = ByteToRightLength(array);
        _blocks = CutByteIntoBlocks(array);
    }

    private byte[] ByteToRightLength(byte[] input)
    {
        _addOfByte = 0;
        if (input.Length % _sizeOfBlock == 0) return input;
        _addOfByte = _sizeOfBlock - input.Length % _sizeOfBlock;
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
                    bit[index] = 1;
                else
                    bit[index] = 0;
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

    private byte[][] CutByteIntoBlocks(byte[] input)
    {
        var blocks = new byte[input.Length / _sizeOfBlock][];
        var numberOfBlock = input.Length / _sizeOfBlock;
        for (var i = 0; i < numberOfBlock; i++) blocks[i] = new byte[_sizeOfBlock];
        for (var i = 0; i < numberOfBlock; i++) Array.Copy(input, i * _sizeOfBlock, blocks[i], 0, _sizeOfBlock);

        return blocks;
    }

    private static byte[] CorrectKeyWord(byte[] input, int keyLength)
    {
        Array.Resize(ref input, keyLength);
        return input;
    }
}