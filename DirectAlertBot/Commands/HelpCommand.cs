using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "help";

        public string Description => "Veja a lista de comandos";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            var msg = "Help Command";


            await botClient.SendTextMessageAsync(context.Chat.Id, msg);
        }
    }
}
