using System.Diagnostics;

namespace Lab3.Core.SystemInfo.Registry;

public class CommonRegistry : MainRegistry
{
    public override string ProcessorName => GetProcessorName();
    public override string MacAddress => GetMacAddress();

    private static string GetMacAddress()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"/sbin/ifconfig en0 | awk '/ether/{print $2}'\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var mac = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        return string.IsNullOrEmpty(mac) ? "00:00:00:00:00:00" : mac;
    }

    protected override long ExtractRam()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/sbin/sysctl",
                Arguments = "-n hw.memsize",
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };

        process.Start();
        var details = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return long.Parse(details.Trim());
    }

    private static string GetProcessorName()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/sbin/sysctl",
                Arguments = "-n machdep.cpu.brand_string",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Trim();
    }
}