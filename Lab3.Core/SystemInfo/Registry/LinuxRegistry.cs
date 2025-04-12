using System.Net.NetworkInformation;

namespace Lab3.Core.SystemInfo.Registry;

public class LinuxRegistry : MainRegistry
{
    public override string ProcessorName => GetProcessorName();
    public override string MacAddress => GetMacAddress();

    protected override long ExtractRam()
    {
        try
        {
            var lines = File.ReadAllLines("/proc/meminfo");
            foreach (var line in lines)
            {
                if (!line.StartsWith("MemTotal:")) continue;
                var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && long.TryParse(parts[1], out var memKb)) return memKb;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return 0;
    }

    private static string GetProcessorName()
    {
        try
        {
            var cpuInfo = File.ReadAllLines("/proc/cpuinfo");
            var modelNameLine = cpuInfo.FirstOrDefault(line => line.StartsWith("model name"));
            if (modelNameLine != null)
            {
                var parts = modelNameLine.Split(":");
                return parts[1].Trim();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return "Unknown processor name";
    }

    private static string GetMacAddress()
    {
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            {
                var mac = networkInterface.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(mac)) return FormatMacAddress(mac);
            }
            else
            {
                return string.Empty;
            }

        return string.Empty;
    }

    private static string FormatMacAddress(string mac)
    {
        return string.Join(":", Enumerable.Range(0, mac.Length / 2).Select(i => mac.Substring(i * 2, 2)));
    }
}