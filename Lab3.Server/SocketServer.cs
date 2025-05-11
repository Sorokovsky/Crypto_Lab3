using System.Net;
using System.Net.Sockets;
using System.Text;
using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Server.Controller;

namespace Lab3.Server;

public class SocketServer
{
    private readonly List<IController> _controllers = [];

    private readonly Encoding _encoding;

    private readonly IPEndPoint _ipEndPoint;
    private readonly Socket _socket;

    private SocketServer(IPEndPoint ipPoint, Encoding encoding)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _ipEndPoint = ipPoint;
        _encoding = encoding;
    }

    public void AddControllers(params IController[] controllers)
    {
        _controllers.AddRange(controllers);
    }

    public static bool TryCreate(string ipAddress, int port, Encoding encoding, out SocketServer socketServer)
    {
        try
        {
            var iPAddress = IPAddress.Parse(ipAddress);
            var iPEndPoint = new IPEndPoint(iPAddress, port);
            socketServer = new SocketServer(iPEndPoint, encoding);
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
        while (true)
        {
            var socket = _socket.Accept();
            try
            {
                var request = ReadFromSocket(socket);
                var response = FindController(request.Type).ProcessMessage(request.Value);
                Console.WriteLine(response.ToString());
                socket.Send(_encoding.GetBytes(response.ToString()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Message not received");
                var response = new Response(ResponseStatus.Error, e.Message);
                socket.Send(_encoding.GetBytes(response.ToString()));
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }

    private Request ReadFromSocket(Socket socket)
    {
        var builder = new StringBuilder();
        var data = new byte[256];
        do
        {
            var bytesCount = socket.Receive(data);
            builder.Append(_encoding.GetString(data, 0, bytesCount));
        } while (socket.Available > 0);

        return Request.FromJson(builder.ToString());
    }

    private IController FindController(MessageType messageType)
    {
        return _controllers.FirstOrDefault(controller => controller.MessageType == messageType,
            new NotFoundController());
    }
}