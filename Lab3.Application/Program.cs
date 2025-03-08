using Lab3.Core;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        var generator = new PrimeNumberGenerator();
        var key = generator.Generate();
        var encryption = new SymmetricEncryption();
        var s = encryption.EncryptFile("files/in.txt", "files/out.txt", key.ToString());
        Console.WriteLine("---> {0}", s);
        encryption.DecryptFile("files/out.txt", "files/out2.txt", s);
    }
}