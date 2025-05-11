using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Server.Security;

namespace Lab3.Server.Controller;

public class RegisterController : IController
{
    public MessageType MessageType => MessageType.Register;

    public Response ProcessMessage(string body)
    {
        var request = RegisterRequest.FromJson(body);
        var encryptKey = SecurityCenter.Register(request.HashOfData, request.AlgorithmName);
        if (encryptKey is null) return new Response(ResponseStatus.Error, "Метод шифрування не знайдено.");
        var textKey = encryptKey.ToString();
        return new Response(ResponseStatus.Ok, encryptKey);
    }
}