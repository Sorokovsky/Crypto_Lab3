using System.Text;
using Lab3.Core;
using Lab3.Core.SystemInfo;
using Lab3.Core.SystemInfo.Registry;

namespace Lab3.Client;

public class SecurityContext
{
    private static SecurityContext? _instance;

    private static readonly string _fileName = "key";

    private readonly IRegistry _registry = RegistryManager.GetRegistryForCurrentPlatform();

    private SecurityContext()
    {
        GenerateSocket();
    }

    private SocketClient Client => GenerateSocket();

    public static SecurityContext Instance => GetInstance();

    public bool IsPro => GetIsPro();

    private static SocketClient GenerateSocket()
    {
        if (!SocketClient.TryCreate("127.0.0.1", 8080, Encoding.UTF8, out var client))
            throw new InvalidOperationException("Сервера не знайдено");
        return client;
    }

    public void Activate()
    {
        var info = UserInfo.FromRegistry(_registry);
        var response = Client.Send($"{MessageType.Register}:{info}");
        if (response.StartsWith(ResponseStatus.Ok))
        {
            var key = response[(ResponseStatus.Ok.Length + 1)..];
            Console.WriteLine("Покупка успішно пройшла.");
        }
        else
        {
            Console.WriteLine("Покупка не вдалася");
        }
    }

    private bool GetIsPro()
    {
        var info = UserInfo.FromRegistry(_registry);
        var stringInfo = info.ToString();
        var response = Client.Send($"{MessageType.Check}:{stringInfo}");
        return response.StartsWith(ServerBoolean.Yes);
    }

    private static SecurityContext GetInstance()
    {
        if (_instance is null) _instance = new SecurityContext();
        return _instance;
    }
}