namespace UiCommands.Core.Interfaces;

public interface ICommand
{
    public string Title { get; }

    public int Number { get; set; }

    public void Invoke(IExitable? exitable = null);
}