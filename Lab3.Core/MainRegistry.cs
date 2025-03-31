namespace Lab3.Core;

public abstract class MainRegistry : IRegistry
{
    public string CurrentUser => Environment.UserName;
    public abstract string ProcessorName { get; }
    public abstract string Ram { get; }
    public string MachineName => Environment.MachineName;
}