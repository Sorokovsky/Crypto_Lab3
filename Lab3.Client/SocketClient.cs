using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab3.Client;

public class SocketClient
{
    private readonly IPEndPoint _ipPoint;
    private readonly Socket _socket;

    private SocketClient(IPEndPoint ipPoint)
    {
        _ipPoint = ipPoint;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public static bool TryCreate(string ip, int port, out SocketClient socketClient)
    {
        try
        {
            var ipAddress = IPAddress.Parse(ip);
            var ipEndPoint = new IPEndPoint(ipAddress, port);
            socketClient = new SocketClient(ipEndPoint);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            socketClient = null!;
            return false;
        }
    }

    public void Send(string message)
    {
        try
        {
            var data = Encoding.Unicode.GetBytes(message);
            _socket.Connect(_ipPoint);
            _socket.Send(data, SocketFlags.None);
            data = new byte[256];
            var builder = new StringBuilder();
            do
            {
                var bytes = _socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (_socket.Available > 0);

            Console.WriteLine("Server respond:" + builder);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Message not delivered");
        }

        Console.ReadLine();
    }
}