using System.Text;
using Lab3.Core;
using Lab3.Core.SystemInfo;
using Lab3.Core.SystemInfo.Registry;

namespace Lab3.Client;

public class SecurityContext
{
    private static SecurityContext? _instance;

    private readonly SocketClient _client;
    private readonly IRegistry _registry = RegistryManager.GetRegistryForCurrentPlatform();

    private SecurityContext()
    {
        if (!SocketClient.TryCreate("127.0.0.1", 8080, Encoding.UTF8, out _client))
            throw new InvalidOperationException("Сервера не знайдено");
    }

    public static SecurityContext Instance => GetInstance();

    public bool IsPro => GetIsPro();

    public void Activate()
    {
        var info = UserInfo.FromRegistry(_registry);
        var response = _client.Send($"{MessageType.Register}:{info}");
        if (response.StartsWith(ResponseStatus.Ok))
            Console.WriteLine("Покупка успішно пройшла.");
        else
            Console.WriteLine("Покупка не вдалася");
    }

    private bool GetIsPro()
    {
        var info = UserInfo.FromRegistry(_registry);
        var response = _client.Send($"{MessageType.Check}:{info}");
        return response.Equals(ServerBoolean.Yes);
    }

    private static SecurityContext GetInstance()
    {
        if (_instance is null) _instance = new SecurityContext();
        return _instance;
    }
}