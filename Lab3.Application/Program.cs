using System.Text;
using Lab3.Core.Encryptions.RSA;

const string input = "Hello, my world";
Console.WriteLine("Input: " + input);

var encryption = new RsaEncryption();
var (encryptKey, decryptKey) = encryption.GenerateKeys();

// Encrypt
var encryptedBytes = encryption.Encrypt(Encoding.UTF8.GetBytes(input), encryptKey);
var encrypted = Encoding.UTF8.GetString(encryptedBytes);
Console.WriteLine("Encrypted: " + encrypted);

// Decrypt
var decrypted = encryption.Decrypt(Encoding.UTF8.GetBytes(encrypted), decryptKey);
var decryptedText = Encoding.UTF8.GetString(decrypted);
Console.WriteLine("===========================================");
Console.WriteLine("Decrypted: " + decryptedText);
Console.WriteLine("End");