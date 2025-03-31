using System.Runtime.InteropServices;
using Lab3.Core;
using Microsoft.Win32;
using SymmetricEncryption = Lab3.Core.Encryptions.SymmetricEncryption;

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