using Lab3.Core;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        var encryption = new SymmetricEncryptionByte();
        var key = Convertor.StringToBytes("Text");
        var decodeKey = encryption.EncryptFile("files/in.txt", "files/out.txt", key);
        Console.WriteLine("---> {0}", Convertor.BytesToString(decodeKey));
        encryption.DecryptFile("files/out.txt", "files/out2.txt", decodeKey);
    }
}