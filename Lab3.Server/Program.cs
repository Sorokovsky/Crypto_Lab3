using System.Text;
using Lab3.Core;

namespace Lab3.Server;

public static class Program
{
    public static void Main()
    {
        var socketServerCreated = SocketServer.TryCreate("127.0.0.1", 8080, Encoding.UTF8, out var serverSocket);
        if (socketServerCreated)
            serverSocket?.Listen((message, socket) =>
            {
                var splitIndex = message.IndexOf(':');
                var type = message[..splitIndex];
                var value = message[(splitIndex + 1)..];
                if (type == MessageType.Register)
                {
                    string response;
                    try
                    {
                        SecurityContext.Instance.Register(value);
                        response = $"{ResponseStatus.Ok}";
                    }
                    catch (Exception)
                    {
                        response = ResponseStatus.Error;
                    }

                    socket.Send(Encoding.UTF8.GetBytes(response));
                }
                else if (type == MessageType.Check)
                {
                    var contains = SecurityContext.Instance.Contains(value);
                    var result = contains ? ServerBoolean.Yes : ServerBoolean.No;
                    socket.Send(Encoding.UTF8.GetBytes(result));
                }
                else
                {
                    socket.Send("Unknown message"u8.ToArray());
                }
            });
    }
}