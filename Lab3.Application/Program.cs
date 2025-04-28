using System.Text;
using Lab3.Application.Commands;
using Lab3.Core;
using Lab3.Core.Encryptions.RSA;
using UiCommands.Core.Context;

// var context = new CommandContext("Головне меню", Encoding.UTF8);
// context.AppendCommands(new EncryptCommand(), new DecryptCommand());
// context.Invoke();

var generator = new PrimeNumberGenerator();
var encryptor = new RSAEncryption();
var k = generator.Generate();
var k1 = generator.Generate();
var n = encryptor.GenerateKey(43, 59, out var d, out var e);
var inputBytes = ByteFilesService.ReadBytes("files/in.txt");
var outputBytes = encryptor.Encrypt(inputBytes, d, n);
ByteFilesService.WriteBytes("files/output.txt", outputBytes, outputBytes.Length);
inputBytes = ByteFilesService.ReadBytes("files/output.txt");
outputBytes = encryptor.Decrypt(inputBytes, e, n);
ByteFilesService.WriteBytes("files/output-2.txt", outputBytes, outputBytes.Length);
