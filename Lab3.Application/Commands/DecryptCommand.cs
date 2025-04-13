using System.Text;
using Lab3.Core;
using Lab3.Core.Encryptions;
using UiCommands.Core.Commands;
using UiCommands.Core.Interfaces;
using UiCommands.Core.Selectors;

namespace Lab3.Application.Commands;

public class DecryptCommand : BaseCommand
{
    public override string Title { get; } = "Дешифрувати";

    public override void Invoke(IExitable? exitable = null)
    {
        var service = new FilesService();
        var file = service.GetPath(Choosing.GetText("файл"));
        if (service.Exists(file, false))
        {
            var bytes = ByteFilesService.ReadBytes(file);
            var encryption = new SymmetricEncryption();
            var key = Choosing.GetNumber("ключ", null, null);
            var output = service.GetPath(Choosing.GetText("вихідний файл"));
            try
            {
                var result = encryption.Decrypt(bytes, BitConverter.GetBytes(key));
                var text = Encoding.UTF8.GetString(result);
                service.Write(output, text, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine($"Розшифрований текст: {service.Read(output, false)}");
        }
    }
}