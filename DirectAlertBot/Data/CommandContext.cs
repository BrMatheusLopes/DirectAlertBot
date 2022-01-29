using DirectAlertBot.Interfaces;
using Telegram.Bot.Types;

namespace DirectAlertBot.Data
{
    public class CommandContext
    {
        public CommandContext(IAlertService alertService, Message message, User user, Chat chat, string[] args)
        {
            AlertService = alertService;
            Message = message;
            User = user;
            Chat = chat;
            Args = args;
        }

        public IAlertService AlertService { get; private set; }
        public Message Message { get; private set; }
        public User User { get; private set; }
        public Chat Chat { get; private set; }
        public string[] Args { get; private set; }
    }
}
