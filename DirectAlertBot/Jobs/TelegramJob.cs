using DirectAlertBot.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DirectAlertBot.Jobs
{
    public interface IJob
    {
        Task Execute();
        DateTime TriggerTime { get;}
        bool Finished { get; set; }
    }

    public abstract class Job : IJob
    {
        protected Job(DateTime triggerTime)
        {
            TriggerTime = triggerTime;
        }

        public DateTime TriggerTime { get; }
        public bool Finished { get; set; }

        public abstract Task Execute();
    }

    public class TelegramJob : Job
    {
        private readonly ITelegramBotClient _botClient;
        private readonly Alert _alert;

        public TelegramJob(ITelegramBotClient botClient, Alert alert, DateTime triggerTime) : base(triggerTime)
        {
            _botClient = botClient;
            _alert = alert;
        }

        public override async Task Execute()
        {
            await SendTextMessageAsync(_alert.ChatId, _alert.Text);
        }

        private async Task SendTextMessageAsync(long chatId, string text)
        {
            await _botClient.SendTextMessageAsync(chatId, text);
        }
    }
}
