using DirectAlertBot.Services;
using Telegram.Bot.Types;

namespace DirectAlertBot.Data
{
    public class CommandContext
    {
        public CommandContext(IAlertService alertService, Message message, User user, Chat chat, SchedulerJob schedulerJob, string[] args)
        {
            AlertService = alertService;
            Message = message;
            User = user;
            Chat = chat;
            Args = args;
            SchedulerJob = schedulerJob;
        }

        public IAlertService AlertService { get; private set; }
        public Message Message { get; private set; }
        public User User { get; private set; }
        public Chat Chat { get; private set; }
        public SchedulerJob SchedulerJob { get; private set; }
        public string[] Args { get; private set; }
    }
}
