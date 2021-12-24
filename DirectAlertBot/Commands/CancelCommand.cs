using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Commands
{
    public class CancelCommand : IBotCommand
    {
        public string Name => "cancel";

        public string Description => "Cancelar uma mensagem previamente programada por seu ID";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            var msg = "Cancel Command";

            await botClient.SendTextMessageAsync(context.Chat.Id, msg);
        }
    }
}
