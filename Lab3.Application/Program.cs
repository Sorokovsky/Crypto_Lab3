using System.Text;
using Lab3.Core.Encryptions.RSA;

const string input = "Hello, my world";
Console.WriteLine("Input: " + input);

var encryption = new RsaEncryption();
var (encryptKey, decryptKey) = encryption.GenerateKeys();

// Encrypt
var encryptedBytes = encryption.Encrypt(Encoding.UTF8.GetBytes(input), encryptKey);
var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
Console.WriteLine("Encrypted: " + encryptedBase64);

// Decrypt
var decryptedBytes = encryption.Decrypt(Convert.FromBase64String(encryptedBase64), decryptKey);
var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
Console.WriteLine("===========================================");
Console.WriteLine("Decrypted: " + decryptedText);
Console.WriteLine("End");