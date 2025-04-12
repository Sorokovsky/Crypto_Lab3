namespace Lab3.Core.SystemInfo.Registry;

public abstract class MainRegistry : IRegistry
{
    public string CurrentUser => Environment.UserName;
    public abstract string ProcessorName { get; }
    public string Ram => GetRam();
    public string MachineName => Environment.MachineName;
    public abstract string MacAddress { get; }

    protected abstract long ExtractRam();

    private string GetRam()
    {
        return ConvertRamResponse(ExtractRam());
    }

    private static string ConvertRamResponse(long ram)
    {
        var mb = ram / (1024 * 1024);
        return mb + "mb";
    }
}