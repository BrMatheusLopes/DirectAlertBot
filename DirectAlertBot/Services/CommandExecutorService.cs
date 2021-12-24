using DirectAlertBot.Data;
using DirectAlertBot.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot.Services
{
    public class CommandExecutorService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ICommandService _commandService;
        private readonly IAlertService _alertService;
        private readonly SchedulerJob _schedulerJob;
        private readonly ILogger<CommandExecutorService> _logger;

        public CommandExecutorService(ITelegramBotClient botClient, ICommandService commandService, IAlertService alertService, 
            SchedulerJob schedulerJob, ILogger<CommandExecutorService> logger)
        {
            _botClient = botClient;
            _commandService = commandService;
            _alertService = alertService;
            _schedulerJob = schedulerJob;
            _logger = logger;
        }
        public async Task ExecuteAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => ExecuteCommand(update.Message!),
                _ => Task.CompletedTask
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }


        private async Task ExecuteCommand(Message message)
        {
            if (!message.IsPrivateChat())
                return;

            var args = message.ParseCommandArgs(out var commandName);
            var isSuccess = _commandService.TryGetCommand(commandName, out var command);
            if (isSuccess)
            {
                var commandContext = new CommandContext(_alertService, message, message.From, message.Chat, _schedulerJob, args);
                await command.Execute(_botClient, commandContext, args);
            }
        }

        private Task HandleErrorAsync(Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError("HandleError: {errorMessage}", errorMessage);
            return Task.CompletedTask;
        }
    }
}
