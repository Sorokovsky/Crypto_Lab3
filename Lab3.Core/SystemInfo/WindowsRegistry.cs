using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace Lab3.Core.SystemInfo;

public class WindowsRegistry : MainRegistry
{
    public override string ProcessorName => Registry.GetValue(
        @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
        "ProcessorNameString",
        "Unknown") as string ?? string.Empty;

    public override string MacAddress => GetMacAddress();

    private static string GetMacAddress()
    {
        var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
            .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up && 
                                 n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

        return networkInterface?.GetPhysicalAddress().ToString() ?? "00-00-00-00-00-00";
    }

    protected override long ExtractRam()
    {
        throw new NotImplementedException();
    }
}