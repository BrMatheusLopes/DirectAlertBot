using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Commands
{
    public class AboutCommand : IBotCommand
    {
        public string Name => "about";

        public string Description => "Informações sobre o bot";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            var msg = "About Command";

            await botClient.SendTextMessageAsync(context.Chat.Id, msg);
        }
    }
}
