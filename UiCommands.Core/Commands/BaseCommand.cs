using UiCommands.Core.Interfaces;

namespace UiCommands.Core.Commands;

public abstract class BaseCommand : ICommand
{
    public abstract string Title { get; }

    public int Number { get; set; }

    public abstract void Invoke(IExitable? exitable = null);

    public override string ToString()
    {
        return $"{Number}-{Title}.";
    }
}