namespace Lab3.Client.Features;

public interface IFeature
{
    public string Name { get; }

    public bool IsFree { get; }

    public void Run();
}