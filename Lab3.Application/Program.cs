using Lab3.Core.SystemInfo;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        var registry = RegistryManager.GetRegistryForCurrentPlatform();
        Console.WriteLine(UserInfo.FromRegistry(registry));
    }
}