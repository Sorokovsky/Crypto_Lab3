using Lab3.Core.SystemInfo;

namespace Lab3.Application;

public static class Program
{
    public static void Main()
    {
        var registry = RegistryManager.GetRegistryForCurrentPlatform();
        Console.WriteLine(registry.CurrentUser);
        Console.WriteLine(registry.ProcessorName);
        Console.WriteLine(registry.Ram);
        Console.WriteLine(registry.MachineName);
    }
}