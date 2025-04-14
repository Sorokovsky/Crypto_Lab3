using System.Text;
using Lab3.Application.Commands;
using Lab3.Core;
using UiCommands.Core.Context;

var context = new CommandContext("Головне меню", Encoding.UTF8);
context.AppendCommands(new EncryptCommand(), new DecryptCommand());
context.Invoke();