using UiCommands.Core.Commands;
using UiCommands.Core.Interfaces;

namespace Lab3.Client.Commands;

public class BuyCommand : BaseCommand
{
    public override string Title { get; } = "Купити повну версію";

    public override void Invoke(IExitable? exitable = null)
    {
        SecurityContext.Instance.Register();
    }
}