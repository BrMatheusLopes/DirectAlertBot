using DirectAlertBot.Entities;
using System;
using Telegram.Bot;

namespace DirectAlertBot.Jobs
{
    public class TelegramJob : Job
    {
        private readonly ITelegramBotClient _botClient;
        private readonly Alert _alert;

        public TelegramJob(ITelegramBotClient botClient, Alert alert, DateTime triggerTime) : base(triggerTime)
        {
            _botClient = botClient;
            _alert = alert;
        }

        public Alert Alert => _alert;

        public override async void Execute()
        {
            await _botClient.SendTextMessageAsync(_alert.ChatId, _alert.Text);
        }
    }
}
