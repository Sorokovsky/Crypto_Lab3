namespace Lab3.Core.SystemInfo.Registry;

public static class RegistryManager
{
    private static Dictionary<Platform, IRegistry> Registries { get; } = new()
    {
        { Platform.Macos, new CommonRegistry() },
        { Platform.Linux, new LinuxRegistry() },
        { Platform.Windows, new WindowsRegistry() }
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