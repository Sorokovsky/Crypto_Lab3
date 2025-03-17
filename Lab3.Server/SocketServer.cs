using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab3.Server;

public class SocketServer
{
    private readonly IPEndPoint _ipEndPoint;
    private readonly Socket _socket;
    
    public static bool TryCreate(string ipAddress, int port, out SocketServer socketServer)
    {
        try
        {
            var iPAddress = IPAddress.Parse(ipAddress);
            var iPEndPoint = new IPEndPoint(iPAddress, port);
            socketServer = new SocketServer(iPEndPoint);
            return true;
        }
        catch (Exception)
        {
            socketServer = null!;
            return false;
        }
    }

    public void Listen()
    {
        _socket.Bind(_ipEndPoint);
        _socket.Listen(10);
        Console.WriteLine("Listening for connections...");
        try
        {
            while (true)
            {
                var handler = _socket.Accept();
                var builder = new StringBuilder();
                var data = new byte[256];

                do
                {
                    var bytesCount = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytesCount));
                } while (handler.Available > 0);

                Console.WriteLine(DateTime.Now.ToShortTimeString() + ":" + builder);

                var message = "ваше повідомлення доставлено";
                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private SocketServer(IPEndPoint ipPoint)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipEndPoint = ipPoint;
    }
    
}