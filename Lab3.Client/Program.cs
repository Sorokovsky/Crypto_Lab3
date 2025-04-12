using System.Text;
using Lab3.Client.Commands;
using Lab3.Client.Features;
using UiCommands.Core.Context;

var context = new CommandContext("Головне меню", Encoding.UTF8);
context.AppendCommands(
    new FeatureCommand(new AddNumbersFeature()),
    new FeatureCommand(new MultiplyNumbersFeature())
);
context.Invoke();