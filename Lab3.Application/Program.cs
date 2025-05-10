using System.Text;
using Lab3.Core.Encryptions.RSA;

const string input = "Hello, my world";
Console.WriteLine(input);
var encryption = new RsaEncryption();
var (encryptKey, decryptKey) = encryption.GenerateKeys();
var output = Encoding.UTF8.GetString(encryption.Encrypt(Encoding.UTF8.GetBytes(input), encryptKey));
Console.WriteLine($"Encrypted: {output}");
var decrypted = Encoding.UTF8.GetString(encryption.Decrypt(Encoding.UTF8.GetBytes(output), decryptKey));
Console.WriteLine("===========================================");
Console.WriteLine($"Decrypted: {decrypted}");
Console.WriteLine("End");