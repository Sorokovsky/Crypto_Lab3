using Microsoft.Win32;

namespace Lab3.Core.SystemInfo;

public class WindowsRegistry : MainRegistry
{
    public override string ProcessorName => Registry.GetValue(
        @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
        "ProcessorNameString",
        "Unknown") as string ?? string.Empty;

    protected override long ExtractRam()
    {
        throw new NotImplementedException();
    }
}