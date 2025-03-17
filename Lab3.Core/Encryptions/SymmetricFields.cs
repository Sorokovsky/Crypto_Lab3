namespace Lab3.Core.Encryptions;

public partial class SymmetricEncryption
{
    private readonly int _sizeOfBlock;
    private int _addOfByte;
    private readonly int _shiftKey;
    private readonly int _quantityOfRounds;
    private byte[][] _blocks = [];
}