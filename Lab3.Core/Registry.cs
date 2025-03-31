namespace Lab3.Core;

public interface IRegistry
{
    public string CurrentUser { get; }
    
    public string ProcessorName { get; } 
    
    public string Ram { get; }
    
    public string MachineName { get; }
}