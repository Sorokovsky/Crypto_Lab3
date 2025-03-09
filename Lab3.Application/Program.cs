using Lab3.Core;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        var generator = new PrimeNumberGenerator();
        var k = generator.Generate();
        var addByte = 0;
        var key = new byte[8];
        var encryption = new SymmetricEncryptionByte();
        var array = SymmetricEncryptionByte.UlongTyByte((long)k);
        Array.Copy(array, 0, key, 0, 4);
        k = generator.Generate();
        array = SymmetricEncryptionByte.UlongTyByte((long)k);
        Array.Copy(array, 0, key, 4, 4);
        var decodeKey = encryption.EncryptFile("files/in.txt", "files/out.txt", key, out addByte);
        Console.WriteLine("---> {0}", decodeKey);
        encryption.DecryptFile("files/out.txt", "files/out2.txt", decodeKey, addByte);
    }
}