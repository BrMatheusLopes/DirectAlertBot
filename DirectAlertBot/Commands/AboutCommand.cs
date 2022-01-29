using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Commands
{
    public class AboutCommand : IBotCommand
    {
        public string Name => "about";
        public string Description => "Informações sobre o bot";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            await botClient.SendTextMessageAsync(context.Chat.Id, AboutMessage, ParseMode.Markdown);
        }

        private const string AboutMessage = $"*Sobre*\n" +
                              $"Bot para Telegram criado por {MyTelegramUser}\n\n" +
                              $"*Github*\n" +
                              $"https://github.com/BrMatheusLopes/DirectAlertBot";

        private const string MyTelegramUser = "[Matheus Lopes](tg://user?id=1045332664)";
    }
}
