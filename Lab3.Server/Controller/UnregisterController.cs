using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Server.Security;

namespace Lab3.Server.Controller;

public class UnregisterController : IController
{
    public MessageType MessageType { get; } = MessageType.Unregister;

    public Response ProcessMessage(string body)
    {
        var dto = UnregisterRequest.FromJson(body);
        var isSuccess = SecurityCenter.Unregister(dto.Encrypted);
        var result = isSuccess ? ServerBoolean.Yes : ServerBoolean.No;
        return new Response(ResponseStatus.Ok, result.ToString());
    }
}