using DirectAlertBot.Data;
using DirectAlertBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using DirectAlertBot.Interfaces;
using Telegram.Bot;

namespace DirectAlertBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var herukuEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_HEROKU_ENVIRONMENT");
            if (herukuEnvironment is ("Development" or "Production"))
            {
                BotConfig = new BotConfiguration
                {
                    BotToken = Environment.GetEnvironmentVariable("BotToken"),
                    HostAddress = Environment.GetEnvironmentVariable("HostAddress"),
                    DropPendingUpdates =
                        bool.TryParse(Environment.GetEnvironmentVariable("DropPendingUpdates"), out bool dropPedingUpdates) && dropPedingUpdates,
                    RemoveWebhook =
                        bool.TryParse(Environment.GetEnvironmentVariable("RemoveWebhook"), out bool removeWebhook) && removeWebhook
                };
            }
            else
            {
                BotConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
            }
        }

        public IConfiguration Configuration { get; }
        public static BotConfiguration BotConfig { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"))
            //options.UseSqlServer(Configuration.GetConnectionString("SQLServer"))
            );

            services.AddHostedService<ConfigureWebhook>();
            services.AddScoped<CommandExecutorService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddSingleton<ICommandService, CommandService>();

            services.AddHttpClient("tgwebhook")
                    .AddTypedClient<ITelegramBotClient>(httpclient => new TelegramBotClient(BotConfig.BotToken ?? throw new NullReferenceException(nameof(BotConfig.BotToken)), httpclient));

            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = BotConfig.BotToken;
                endpoints.MapControllerRoute(name: "tgwebhook",
                                             pattern: $"bot/{token}",
                                             new { controller = "Webhook", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}
