using DirectAlertBot.Data;
using DirectAlertBot.Jobs;
using DirectAlertBot.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Commands
{
    public class AlertCommand : IBotCommand
    {
        public string Name => "alert";

        public string Description => "Agendar uma mensagem no chat atual para ser enviada no futuro";

        public async Task Execute(ITelegramBotClient botClient, CommandContext context, string[] args)
        {
            string? text;

            char letter = GetLetter(args[0]);
            string value = args[0].Split(letter, StringSplitOptions.RemoveEmptyEntries)[0];

            bool isSuccess = int.TryParse(value, out var result);
            if (isSuccess && result > 0)
            {
                var schuduledTime = GetScheduleTime(letter, result);
                var alert = new Alert
                {
                    Id = 1,
                    ChatId = context.Chat.Id,
                    ScheduledTime = schuduledTime,
                    Text = context.Message.Text ?? "SEM MENSAGEM"
                };

                TimeSpan timeUntilTriger = alert.ScheduledTime.Subtract(DateTime.Now);
                text = $"Sua mensagem foi agendada para as {alert.ScheduledTime}, seu ID é {alert.Id}.";

                IJob job = new TelegramJob(botClient, alert, alert.ScheduledTime);
                context.SchedulerJob.AddJob(job);

            }
            else if (result < 0)
            {
                text = "Desculpe, não tenho uma máquina do tempo, então não posso enviar sua mensagem para o passado.";
            }
            else
            {
                text = "Formato inválido";
            }


            await botClient.SendTextMessageAsync(context.Chat.Id, text);
        }

        private DateTime GetScheduleTime(char type, int value)
        {
            var date = DateTime.Now;
            // w = week
            switch (type)
            {
                case 'h':
                    date = date.AddHours(value);
                    break;
                case 'd':
                    date = date.AddDays(value);
                    break;
                case 'w':
                    date = date.AddDays(7 * value);
                    break;
                default:
                    // Minutes
                    date = date.AddMinutes(value);
                    break;
            }

            return date;
        }

        private bool IsValid(string arg)
        {
            arg = arg.ToLower();

            if (!char.IsDigit(arg[0]))
                return false;

            return arg.Contains('m')
                || arg.Contains('h')
                || arg.Contains('d')
                || arg.Contains('w');
        }

        static char GetLetter(string input)
        {
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
