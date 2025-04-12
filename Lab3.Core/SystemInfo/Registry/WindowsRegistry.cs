using System.Management;

namespace Lab3.Core.SystemInfo.Registry;

public class WindowsRegistry : MainRegistry
{
    public override string ProcessorName => Microsoft.Win32.Registry.GetValue(
        @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
        "ProcessorNameString",
        "Unknown") as string ?? string.Empty;

    public override string MacAddress => GetMacAddress();

    private static string GetMacAddress()
    {
        var mgmt = new ManagementClass("Win32_NetworkAdapterConfiguration");
        var col = mgmt.GetInstances();
        var address = string.Empty;
        foreach (var obj in col)
        {
            if (address == string.Empty)
                if ((bool)obj["IPEnabled"])
                    address = obj["MacAddress"].ToString();

            obj.Dispose();
        }

        return address;
    }

    protected override long ExtractRam()
    {
        var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
        foreach (var obj in searcher.Get())
        {
            var totalBytes = Convert.ToInt64(obj["TotalPhysicalMemory"]);
            return totalBytes;
        }

        return 0;
    }
}