namespace Lab3.Core.SystemInfo.Registry;

public interface IRegistry
{
    public string CurrentUser { get; }

    public string ProcessorName { get; }

    public string Ram { get; }

    public string MachineName { get; }

    public string MacAddress { get; }
}