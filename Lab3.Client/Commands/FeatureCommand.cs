using Lab3.Client.Features;
using UiCommands.Core.Commands;
using UiCommands.Core.Interfaces;

namespace Lab3.Client.Commands;

public class FeatureCommand : BaseCommand
{
    private readonly IFeature _feature;

    public FeatureCommand(IFeature feature)
    {
        Title = feature.Name;
        _feature = feature;
    }

    public override string Title { get; }

    public override void Invoke(IExitable? exitable = null)
    {
        var hasPermission = _feature.IsFree || SecurityContext.Instance.IsPro;
        if (hasPermission) _feature.Run();
        else Console.WriteLine("Купіть повну версію програми.");
    }
}