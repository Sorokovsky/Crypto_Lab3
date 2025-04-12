using UiCommands.Core.Selectors;

namespace Lab3.Client.Features;

public class AddNumbersFeature : IFeature
{
    public string Name => "Додати два числа";
    public bool IsFree => true;

    public void Run()
    {
        var a = Choosing.GetNumber("перше число", null, null);
        var b = Choosing.GetNumber("друге число", null, null);
        Console.WriteLine($"{a} + {b} = {a + b}");
    }
}