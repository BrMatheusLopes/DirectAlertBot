using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Extensions
{
    public static class MessageExtensions
    {
        public static long GetUserId(this Message message)
        {
            return message.From.Id;
        }

        public static long GetChatId(this Message message)
        {
            return message.Chat.Id;
        }

        public static bool IsPrivateChat(this Message message)
        {
            return message.Chat.Type == ChatType.Private;
        }

        public static string[] ParseCommandArgs(this Message message, out string commandName)
        {
            commandName = GetCommandName(message);
            return GetArgs(GetMessageTextWithoutCommand(message));
        }

        public static string[] GetArgs(this string input)
        {
            return input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToArray();
        }

        public static string ArgsToText(this string[] input, int index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = index; i < input.Length; i++)
            {
                sb.Append($"{input[i]} ");
            }
            return sb.ToString();
        }

        public static string GetMessageTextWithoutCommand(this Message message)
        {
            var commandLength = message.Entities?.FirstOrDefault(x => x.Type == MessageEntityType.BotCommand)?.Length ?? 0;
            return message.Text.Substring(commandLength).Trim();
        }

        public static string GetCommandName(this Message message)
        {
            var commandLength = message.Entities?.FirstOrDefault(x => x.Type == MessageEntityType.BotCommand)?.Length ?? 0;
            var commandName = message.Text.Substring(0, commandLength);

            return commandName.Length > 0 ? commandName.Substring(1) : commandName;
        }
    }
}
