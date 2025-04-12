using UiCommands.Core.Selectors;

namespace Lab3.Client.Features;

public class MultiplyNumbersFeature : IFeature
{
    public string Name => "Перемножити числа";
    public bool IsFree => false;

    public void Run()
    {
        var a = Choosing.GetNumber("перше число", null, null);
        var b = Choosing.GetNumber("друге число", null, null);
        Console.WriteLine($"{a} * {b} = {a * b}");
    }
}