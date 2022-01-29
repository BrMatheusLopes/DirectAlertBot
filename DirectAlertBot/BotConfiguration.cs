namespace DirectAlertBot
{
    public class BotConfiguration
    {
        public string? BotToken { get; set; }
        public string? HostAddress { get; set; }
        public bool? DropPendingUpdates { get; set; }
        public bool RemoveWebhook { get; set; }
    }
}
