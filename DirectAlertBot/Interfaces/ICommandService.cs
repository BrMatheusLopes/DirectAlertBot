using DirectAlertBot.Commands;
using System.Collections.Generic;

namespace DirectAlertBot.Interfaces
{
    public interface ICommandService
    {
        IEnumerable<IBotCommand> GetAllCommands();
        bool TryGetCommand(string name, out IBotCommand? command);
    }
}