using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Core.Encryptions;
using Lab3.Server.Security;

namespace Lab3.Server.Controller;

public class SetEncryptionController : IController
{
    public MessageType MessageType => MessageType.SetEncryption;

    public Response ProcessMessage(string body)
    {
        var encryptorBody = SetEncryptorBody.FromJson(body);
        var found = EncryptionsDictionary.Instance.TryGet(encryptorBody.AlgorithmName, out var encryption);
        if (found is false) return new Response(ResponseStatus.Error, "Метод шифрування не знадено");
        var foundUser = UserTable.Instance.TryGet(encryptorBody.Hash, out var user);
        if (foundUser is false) return new Response(ResponseStatus.Error, "Користувача не знадено");
        user.Algorithm = encryption;
        return new Response(ResponseStatus.Ok, "Метод шифрування успісщно змінено.");
    }
}