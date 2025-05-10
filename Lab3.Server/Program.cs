using System.Text;
using Lab3.Server.Controller;

namespace Lab3.Server;

public static class Program
{
    public static void Main()
    {
        var createdSocket = SocketServer.TryCreate("127.0.0.1", 8080, Encoding.UTF8, out var serverSocket);
        if (!createdSocket)
        {
            Console.WriteLine("Сокет не створено");
            return;
        }

        var setEncryptionController = new SetEncryptionController();
        var registerController = new RegisterController();
        var checkController = new CheckController();
        var unregisterController = new UnregisterController();
        serverSocket.AddControllers(setEncryptionController, registerController, checkController, unregisterController);
        serverSocket.Listen();
    }
}