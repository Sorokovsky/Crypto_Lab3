using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab3.Client;

public class SocketClient
{
    private readonly IPEndPoint _ipPoint;
    private readonly Socket _socket;

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
            _socket.Send(data);
            // отримуємо відповідь
            data = new byte[256]; // буфер для відповіді
            var builder = new StringBuilder();
            var bytes = 0;

            do
            {
                bytes = _socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (_socket.Available > 0);
            Console.WriteLine("відповідь сервера:" + builder);
            // закриваємо сокет
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        Console.ReadLine();
    }

    private SocketClient(IPEndPoint ipPoint)
    {
        _ipPoint = ipPoint;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
}