using System.Text;
using UiCommands.Core.Commands;
using UiCommands.Core.Interfaces;
using UiCommands.Core.Selectors;

namespace UiCommands.Core.Context;

public sealed class CommandContext : ICommandContext
{
    private readonly List<ICommand> _commands = [];

    private readonly Encoding _encoding;

    private bool _canExit;

    private Encoding? _currentInputEncoding;
    private int _currentNumber;
    private Encoding? _currentOutputEncoding;

    public CommandContext(string title, Encoding encoding)
    {
        Title = title;
        _encoding = encoding;
        AppendCommands(new ExitCommand());
    }

    public string Title { get; }

    public int Number { get; set; }

    public void Invoke(IExitable? exitable = null)
    {
        _canExit = false;
        Loop();
    }

    public void Exit()
    {
        _canExit = true;
    }

    public void AppendCommands(params ICommand[] commands)
    {
        commands.ToList().ForEach(command =>
        {
            command.Number = _currentNumber++;
            _commands.Add(command);
        });
    }

    private void Loop()
    {
        SetupEncoding();
        try
        {
            while (_canExit is false) ChooseCommand().Invoke(this);
        }
        catch (StackOverflowException e)
        {
            Console.WriteLine(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine($"Сталася помилка: \"{e.Message}\".");
            Loop();
        }

        ClearEncoding();
    }

    private ICommand ChooseCommand()
    {
        return Choosing.GetFromList(
            _commands,
            Title,
            item => item.ToString() ?? string.Empty,
            int.Parse,
            (first, second) => first == second.Number,
            false
        );
    }

    private void SetupEncoding()
    {
        _currentInputEncoding = Console.InputEncoding;
        _currentOutputEncoding = Console.OutputEncoding;
        Console.OutputEncoding = _encoding;
        Console.InputEncoding = _encoding;
    }

    private void ClearEncoding()
    {
        if (_currentOutputEncoding is not null) Console.OutputEncoding = _currentOutputEncoding;

        if (_currentInputEncoding is not null) Console.InputEncoding = _currentInputEncoding;
    }
}