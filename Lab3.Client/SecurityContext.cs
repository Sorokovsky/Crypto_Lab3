using System.Text;
using Lab3.Core.Contracts;
using Lab3.Core.CustomProtocol;
using Lab3.Core.Encryptions;
using Lab3.Core.Services;
using Lab3.Core.SystemInfo;
using Lab3.Core.SystemInfo.Registry;
using Lab3.Core.Utils;
using UiCommands.Core.Selectors;

namespace Lab3.Client;

public class SecurityContext
{
    private const string KeyFileName = "key";
    private const string AlgorithmFileName = "algorithm";
    private static SecurityContext? _instance;
    private readonly FilesService _filesService = new();

    private readonly IRegistry _registry = RegistryManager.GetRegistryForCurrentPlatform();

    private SecurityContext()
    {
        GenerateSocket();
    }

    private SocketClient Client => GenerateSocket();

    public static SecurityContext Instance => GetInstance();

    public bool IsPro => Verify();

    public void Register()
    {
        var userInfo = UserInfo.FromRegistry(_registry);
        var hashed = Hashing.GetHash(userInfo.ToString());
        var encryption = Choosing.GetFromList(
            EncryptionsDictionary.Instance.All.ToList(), "Методи шифрування:",
            item => item.Key,
            input => input,
            (first, second) => first.Equals(second.Key)
        ).Value;
        var body = new RegisterRequest(encryption.ToString(), hashed);
        var request = new Request(MessageType.Register, body.ToString());
        var responseText = Client.Send(request.ToString());
        var response = Response.FromJson(responseText);
        if (response.Status == ResponseStatus.Error) throw new ArgumentException(response.Value);
        Console.WriteLine("Успішна покупка про весрії.");
        _filesService.Write(KeyFileName, response.Value);
        _filesService.Write(AlgorithmFileName, encryption.ToString());
    }

    public bool Verify()
    {
        var info = UserInfo.FromRegistry(_registry);
        var hashed = Hashing.GetHash(info.ToString());
        if (_filesService.Exists(KeyFileName) is false || _filesService.Exists(AlgorithmFileName) is false)
            return false;
        var keyText = _filesService.Read(KeyFileName);
        var algorithmText = _filesService.Read(AlgorithmFileName);
        var found = EncryptionsDictionary.Instance.TryGet(algorithmText, out var encryption);
        if (found is false) return false;
        var key = (IKey?)Activator.CreateInstance(encryption.EncodeKeyType);
        if (key is null) return false;
        key = key.FromJson(keyText);
        var encrypted = Encoding.UTF8.GetString(encryption.Encrypt(Encoding.UTF8.GetBytes(hashed), key));
        var request = new Request(MessageType.Check, new CheckRequest(encrypted).ToString());
        var responseText = Client.Send(request.ToString());
        var response = Response.FromJson(responseText);
        if (response.Status == ResponseStatus.Error) throw new ArgumentException(response.Value);
        return response.Value.Equals(nameof(ServerBoolean.Yes));
    }

    private static SocketClient GenerateSocket()
    {
        if (!SocketClient.TryCreate("127.0.0.1", 8080, Encoding.UTF8, out var client))
            throw new InvalidOperationException("Сервера не знайдено");
        return client;
    }

    private static SecurityContext GetInstance()
    {
        if (_instance is null) _instance = new SecurityContext();
        return _instance;
    }
}