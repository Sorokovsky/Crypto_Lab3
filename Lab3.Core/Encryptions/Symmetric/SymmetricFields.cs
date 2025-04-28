namespace Lab3.Core.Encryptions.Symmetric;

public partial class SymmetricEncryption
{
    private readonly int _quantityOfRounds;
    private readonly int _shiftKey;
    private readonly int _sizeOfBlock;
    private int _addOfByte;
    private byte[][] _blocks = [];
}