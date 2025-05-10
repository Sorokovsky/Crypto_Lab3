using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Server.Security;

namespace Lab3.Server.Controller;

public class CheckController : IController
{
    public MessageType MessageType => MessageType.Check;

    public Response ProcessMessage(string body)
    {
        var dto = CheckRequest.FromJson(body);
        var isCorrect = SecurityCenter.IsCorrectSign(dto.Encrypted);
        var binary = isCorrect ? ServerBoolean.Yes : ServerBoolean.No;
        return new Response(ResponseStatus.Ok, binary.ToString());
    }
}