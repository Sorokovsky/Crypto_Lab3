/*
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
*/

using Point = Lab3.Core.EllipticalCurves.Point;

var first = new Point(3, 10, 1, 1, 23);
var second = new Point(9, 7, 1, 1, 23);
var third = first + second;
Console.WriteLine($"{first} + {second} = {third}");
Console.WriteLine($"2 * {third} = {2 * third}");