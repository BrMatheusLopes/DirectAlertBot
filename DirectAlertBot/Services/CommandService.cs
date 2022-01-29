using DirectAlertBot.Commands;
using System;
using System.Collections.Generic;
using DirectAlertBot.Interfaces;

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
                new HelpCommand(),
                new StartCommand()
            };
        }

        public IEnumerable<IBotCommand> GetAllCommands()
            => _commands;

        public bool TryGetCommand(string name, out IBotCommand? command)
        {
            command = _commands.Find(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return command != null;
        }
    }
}
