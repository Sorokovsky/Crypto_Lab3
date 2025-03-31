namespace Lab3.Core;

public static class RegistryManager
{
    private static Dictionary<Platform, IRegistry> Registries { get; } = new()
    {
        { Platform.Macos, new MacRegistry() },
        { Platform.Linux, null },
        { Platform.Windows, null }
    };

    public static IRegistry GetRegistryForCurrentPlatform()
    {
        if (OperatingSystem.IsMacOS())
            return Registries[Platform.Macos];
        if (OperatingSystem.IsLinux())
            return Registries[Platform.Linux];
        if (OperatingSystem.IsWindows())
            return Registries[Platform.Windows];
        throw new PlatformNotSupportedException();
    }
}