using System.Diagnostics;

namespace Lab3.Core.SystemInfo;

public class MacRegistry : MainRegistry
{
    public override string ProcessorName => GetProcessorName();
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