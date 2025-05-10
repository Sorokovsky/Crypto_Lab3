using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;

namespace Lab3.Server.Controller;

public interface IController
{
    public MessageType MessageType { get; }

    public Response ProcessMessage(string body);
}