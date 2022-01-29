using DirectAlertBot.Data;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "help";
        public string Description => "Veja a lista de comandos";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            await botClient.SendTextMessageAsync(context.Chat.Id, HelpMessageText, ParseMode.Markdown);
        }

        public static string HelpMessageText =>
            "Use este formato para criar alertas:" +
            "\n" +
            "/alert time text" +
            "\n" +
            "\n" +
            "*'time'* pode ser um dos seguintes:" +
            "\n" +
            "• O número de minutos, horas, dias ou semanas na forma 20m, 3h, 5d ou 2w respectivamente." +
            "\n" +
            "*'text'* é qualquer coisa que você queira que o bot diga." +
            "\n" +
            "*Exemplo:*" +
            "\n" +
            "/alert 1m Olá mundo";
    }
}
