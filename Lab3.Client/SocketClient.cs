using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab3.Client;

public class SocketClient
{
    private readonly Encoding _encoding;
    private readonly IPEndPoint _ipPoint;
    private readonly Socket _socket;

    private SocketClient(IPEndPoint ipPoint, Encoding encoding)
    {
        _ipPoint = ipPoint;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _encoding = encoding;
    }

    public static bool TryCreate(string ip, int port, Encoding encoding, out SocketClient socketClient)
    {
        try
        {
            var ipAddress = IPAddress.Parse(ip);
            var ipEndPoint = new IPEndPoint(ipAddress, port);
            socketClient = new SocketClient(ipEndPoint, encoding);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            socketClient = null!;
            return false;
        }
    }

    public string Send(string message)
    {
        try
        {
            var data = _encoding.GetBytes(message);
            _socket.Connect(_ipPoint);
            _socket.Send(data, SocketFlags.None);
            data = new byte[256];
            var builder = new StringBuilder();
            do
            {
                _socket.Receive(data, data.Length, SocketFlags.None);
                builder.Append(_encoding.GetString(data));
            } while (_socket.Available > 0);

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            return builder.ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Повідомлення з сервера не прийшло.");
            return string.Empty;
        }
    }
}