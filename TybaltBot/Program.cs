using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TybaltBot.Services;

namespace TybaltBot
{
    public class Program
    {
        private readonly IServiceProvider serviceProvider;

        public Program()
        {
            serviceProvider = ConfigureServices();
        }

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public ILogger Logger { get; set; }

        public async Task MainAsync()
        {
            var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            var configService = serviceProvider.GetRequiredService<ConfigService>();
            Logger = serviceProvider.GetRequiredService<LoggingService>().Logger;

            var config = await configService.LoadJsonAsync(ConfigService.configFileName);
            if (config == null)
            {
                Logger.Error("No Config was found.");
                return;
            }

            await serviceProvider.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private IServiceProvider ConfigureServices()
        {
            var config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true,
                DefaultRetryMode = RetryMode.AlwaysRetry
            };

            var client = new DiscordSocketClient(config);

            var interactionService = new InteractionService(client.Rest);

            var collection = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(interactionService)
                .AddSingleton<LoggingService>()
                .AddSingleton<ConfigService>()
                .AddSingleton<CommandHandlingService>();

            return collection.BuildServiceProvider();
        }
    }
}