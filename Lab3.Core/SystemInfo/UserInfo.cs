using System.Text.Json;
using Lab3.Core.SystemInfo.Registry;

namespace Lab3.Core.SystemInfo;

public record UserInfo(string UserName, string MachineName, string Ram, string ProcessorName, string MacAddress)
{
    public static UserInfo FromRegistry(IRegistry registry)
    {
        return new UserInfo(registry.CurrentUser, registry.MachineName, registry.Ram, registry.ProcessorName,
            registry.MacAddress);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static UserInfo FromString(string str)
    {
        return JsonSerializer.Deserialize<UserInfo>(str) ??
               new UserInfo(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
    }
}