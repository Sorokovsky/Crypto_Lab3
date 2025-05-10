namespace Lab3.Core.Encryptions;

public interface IEncryption
{
    public Type EncodeKeyType { get; set; }
    public byte[] Encrypt(byte[] input, IKey encryptionKey);

    public byte[] Decrypt(byte[] input, IKey decryptionKey);

    public (IKey encryptKey, IKey decryptKey) GenerateKeys();

    public string ToString();
}