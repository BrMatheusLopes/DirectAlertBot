using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Commands
{
    public interface IBotCommand
    {
        string Name { get; }
        string Description { get; }

        Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args);
    }
}
