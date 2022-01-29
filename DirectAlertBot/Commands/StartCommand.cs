using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Commands
{
    public class StartCommand : IBotCommand
    {
        public string Name => "start";

        public string Description => "Iniciar conversa com o bot";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            await botClient.SendTextMessageAsync(context.Chat.Id, HelpCommand.HelpMessageText, ParseMode.Markdown);
        }
    }
}
