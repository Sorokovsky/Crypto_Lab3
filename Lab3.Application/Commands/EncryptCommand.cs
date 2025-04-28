using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;
using UiCommands.Core.Commands;
using UiCommands.Core.Interfaces;
using UiCommands.Core.Selectors;
using SymmetricEncryption = Lab3.Core.Encryptions.Symmetric.SymmetricEncryption;

namespace Lab3.Application.Commands;

public class EncryptCommand : BaseCommand
{
    public override string Title { get; } = "Зашифрувати";

    public override void Invoke(IExitable? exitable = null)
    {
        var service = new FilesService();
        var file = service.GetPath(Choosing.GetText("файл"));
        if (service.Exists(file, false))
        {
            var encryption = new SymmetricEncryption();
            var key = Choosing.GetNumber("ключ", null, null);
            var bytes = ByteFilesService.ReadBytes(file);
            var output = service.GetPath(Choosing.GetText("вихідний файл"));
            var (encrypted, decodeKey) = encryption.EncryptFile(bytes, BitConverter.GetBytes(key));
            var text = Encoding.UTF8.GetString(encrypted);
            service.Write(output, text, false);
            Console.WriteLine($"Ключ дешифрування: {BitConverter.ToInt64(decodeKey, 0)}");
            Console.WriteLine($"Зашифрований текст: {service.Read(output, false)}");
        }
    }
}