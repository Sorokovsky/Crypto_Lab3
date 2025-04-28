using System.Text;
using Lab3.Application.Commands;
using Lab3.Core;
using Lab3.Core.Encryptions.RSA;
using UiCommands.Core.Context;

// var context = new CommandContext("Головне меню", Encoding.UTF8);
// context.AppendCommands(new EncryptCommand(), new DecryptCommand());
// context.Invoke();

var n = RsaEncryption.GenerateKey(43, 59, out var d, out var e);
var inputBytes = ByteFilesService.ReadBytes("files/in.txt");
var outputBytes = RsaEncryption.Encrypt(inputBytes, d, n);
ByteFilesService.WriteBytes("files/output.txt", outputBytes, outputBytes.Length);
inputBytes = ByteFilesService.ReadBytes("files/output.txt");
outputBytes = RsaEncryption.Decrypt(inputBytes, e, n);
ByteFilesService.WriteBytes("files/output-2.txt", outputBytes, outputBytes.Length);
