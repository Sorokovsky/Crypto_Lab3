namespace Lab3.Server;

public static class Program
{
    public static void Main()
    {
        var socketServerCreated = SocketServer.TryCreate("127.0.0.1", 8080, out var serverSocket);
        if (socketServerCreated) serverSocket?.Listen();
    }
}