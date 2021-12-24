using DirectAlertBot.Commands;

namespace DirectAlertBot.Services
{
    public interface ICommandService
    {
        bool TryGetCommand(string name, out IBotCommand command);
    }
}