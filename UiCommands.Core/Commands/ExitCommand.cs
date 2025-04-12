using UiCommands.Core.Interfaces;

namespace UiCommands.Core.Commands;

public class ExitCommand : BaseCommand
{
    public override string Title => "Вийти";

    public override void Invoke(IExitable? exitable = null)
    {
        exitable?.Exit();
    }
}