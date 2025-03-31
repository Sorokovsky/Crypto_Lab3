using System.Text.Json;

namespace Lab3.Core.SystemInfo;

public record UserInfo(string UserName, string MachineName, string Ram, string ProcessorName)
{
    public static UserInfo FromRegistry(IRegistry registry)
    {
        return new UserInfo(registry.CurrentUser, registry.MachineName, registry.Ram, registry.ProcessorName);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    public static UserInfo FromString(string str)
    {
        return JsonSerializer.Deserialize<UserInfo>(str) ?? new UserInfo(str, string.Empty, string.Empty, string.Empty);
    }
}