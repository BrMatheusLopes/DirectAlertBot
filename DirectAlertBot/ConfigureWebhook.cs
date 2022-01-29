using DirectAlertBot.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DirectAlertBot
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly ILogger<ConfigureWebhook> _logger;
        private readonly IServiceProvider _services;
        private readonly BotConfiguration _botConfig;

        public ConfigureWebhook(ILogger<ConfigureWebhook> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _services = serviceProvider;
            _botConfig = Startup.BotConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var commandService = scope.ServiceProvider.GetRequiredService<ICommandService>();

            await ConfigureCommands(botClient, commandService);

            var webhookAddress = _botConfig.HostAddress + "/bot/" + _botConfig.BotToken;
            _logger.LogInformation("Setting webhook: " + webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                maxConnections: 10,
                allowedUpdates: Array.Empty<UpdateType>(),
                dropPendingUpdates: _botConfig.DropPendingUpdates,
                cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_botConfig.RemoveWebhook)
                return;

            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }

        private async Task ConfigureCommands(ITelegramBotClient botClient, ICommandService commandService)
        {
            _logger.LogInformation("Setting Commands");
            var privateCommands = new List<BotCommand>();
            foreach (var cmd in commandService.GetAllCommands())
            {
                if (cmd.Name != "start")
                    privateCommands.Add(new BotCommand { Command = cmd.Name, Description = cmd.Description });
            }

            await botClient.SetMyCommandsAsync(privateCommands, BotCommandScope.AllPrivateChats());
        }
    }
}
