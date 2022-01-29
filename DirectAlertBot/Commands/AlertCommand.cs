using DirectAlertBot.Data;
using DirectAlertBot.Entities;
using DirectAlertBot.Extensions;
using DirectAlertBot.Jobs;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Commands
{
    public class AlertCommand : IBotCommand
    {
        public string Name => "alert";
        public string Description => "Agenda uma mensagem no chat atual para ser enviada no futuro";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            string text = HelpCommand.HelpMessageText;
            char letter = GetLetter(args[0]);
            string value = args[0].Split(letter, StringSplitOptions.RemoveEmptyEntries)[0];

            _ = int.TryParse(value, out int result);
            if (result <= 0)
            {
                text = "Desculpe, não tenho uma máquina do tempo, então não posso enviar sua mensagem para o passado.";
            }
            else
            {
                var schuduledTime = GetScheduleTime(letter, result);
                var alert = new Alert
                {
                    ChatId = context.Chat.Id,
                    ScheduledTime = schuduledTime,
                    Text = args.ArgsToText(startIndex: 1) ?? "😐"
                };
                context.AlertService.InsertAlert(alert);
                SchedulerJob.AddJob(new TelegramJob(botClient, alert, alert.ScheduledTime));
                text = $"Sua mensagem foi agendada, seu ID é {alert.Id}.";
            }

            await botClient.SendTextMessageAsync(context.Chat.Id, text, ParseMode.Markdown);
        }

        private DateTime GetScheduleTime(char type, int value)
        {
            var date = DateTime.UtcNow;

            date = type switch
            {
                'h' => date.AddHours(value),
                'd' => date.AddDays(value),
                'w' => date.AddDays(7 * value),
                _ => date.AddMinutes(value),
            };
            return date;
        }

        private char GetLetter(string input)
        {
            input = input.ToLower();
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    return input[i];
                }
            }

            return 'm';
        }
    }
}
