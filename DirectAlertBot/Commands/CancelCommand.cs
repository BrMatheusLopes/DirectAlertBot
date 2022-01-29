using DirectAlertBot.Data;
using DirectAlertBot.Entities;
using DirectAlertBot.Jobs;
using System;
using System.Linq;
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
            string text = "Não consigo encontrar uma mensagem com este ID.";

            if (args.Length == 0)
            {
                text = "Por favor, coloque um ID de mensagem ao lado deste comando.";
            }
            else
            {
                bool success = int.TryParse(args[0], out int value);
                var alert = context.AlertService.FindAlertById(value);
                if (success && alert?.ChatId == context.User.Id && IsValidAlert(alert))
                {
                    var jobs = SchedulerJob.GetAllJobs();
                    var myJob = jobs.FirstOrDefault(x => ((TelegramJob)x).Alert.Id == alert.Id);
                    if (myJob != null)
                    {
                        context.AlertService.RemoveAlert(alert.Id);
                        SchedulerJob.RemoveJob(myJob);
                        text = "Sua mensagem foi cancelada.";
                    }
                }
            }

            await botClient.SendTextMessageAsync(context.Chat.Id, text);
        }

        private bool IsValidAlert(Alert alert)
        {
            return alert.ScheduledTime.Subtract(DateTime.UtcNow) > TimeSpan.Zero;
        }
    }
}
