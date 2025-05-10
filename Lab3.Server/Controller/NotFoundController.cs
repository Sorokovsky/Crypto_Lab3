using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;

namespace Lab3.Server.Controller;

internal sealed class NotFoundController : IController
{
    public MessageType MessageType => MessageType.None;

    public Response ProcessMessage(string body)
    {
        return new Response(ResponseStatus.Error, "Не відома операція");
    }
}