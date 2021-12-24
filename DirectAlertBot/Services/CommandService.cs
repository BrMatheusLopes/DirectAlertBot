using DirectAlertBot.Commands;
using System.Collections.Generic;

namespace DirectAlertBot.Services
{
    public class CommandService : ICommandService
    {
        private readonly List<IBotCommand> _commands;
        public CommandService()
        {
            _commands = new List<IBotCommand>() {
                new AboutCommand(),
                new AlertCommand(),
                new CancelCommand(),
                new HelpCommand()
            };
        }

        public bool TryGetCommand(string name, out IBotCommand command)
        {
            command = _commands.Find(x => x.Name == name);
            return command != null;
        }
    }
}
