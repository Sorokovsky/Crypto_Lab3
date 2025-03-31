using System.Diagnostics;

namespace Lab3.Core;

public class MacRegistry : MainRegistry
{
    public override string ProcessorName => GetProcessorName();
    public override string Ram => GetRam();

    private static string GetProcessorName()
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "/usr/sbin/sysctl",
                Arguments = "-n machdep.cpu.brand_string",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output.Trim();
    }

    private static string GetRam()
    {
        var process = new Process()
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
        var number = long.Parse(details.Trim());
        var mb = number / (1024 * 1024);
        return mb + "mb";
    }
}
    