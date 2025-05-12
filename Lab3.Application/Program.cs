using System.Text;
using Lab3.Core.EllipticalCurves;
using Lab3.Core.Utils;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        try
        {
            const string input = "Привіт, мій 3 світ";
            var encryption = new EllipticalEncryption();
            var (encryptKey, decryptKey) = encryption.GenerateKeys();
            var encryptedBytes = encryption.Encrypt(Encoding.UTF8.GetBytes(input), encryptKey);
            var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
            var decryptedBytes = encryption.Decrypt(Convert.FromBase64String(encryptedBase64), decryptKey);
            var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            Console.WriteLine("Input: " + input);
            Console.WriteLine("Encrypted: " + encryptedBase64);

            Console.WriteLine("===========================================");
            Console.WriteLine("Decrypted: " + decryptedText);
            Console.WriteLine("End");
            var json = encryptKey.ToString();
            Console.WriteLine(json);
            encryptKey = new EllipticalEncryptionKey().FromJson(json);
            Console.WriteLine("Encrypted: " + encryptKey.ToString());
        }
        catch (InvalidOperationException je)
        {
            Console.WriteLine(je);
        }
        catch (Exception)
        {
            Main();
        }
    }
}