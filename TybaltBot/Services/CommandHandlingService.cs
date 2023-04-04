using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TybaltBot.Resources;

namespace TybaltBot.Services
{
    public class CommandHandlingService
    {
        private readonly InteractionService interactionService;
        private readonly DiscordSocketClient client;
        private readonly ConfigService config;
        private readonly ILogger logger;
        private readonly IServiceProvider services;

        public CommandHandlingService(IServiceProvider services)
        {
            interactionService = services.GetRequiredService<InteractionService>();
            client = services.GetRequiredService<DiscordSocketClient>();
            config = services.GetRequiredService<ConfigService>();
            logger = services.GetRequiredService<LoggingService>().Logger;
            this.services = services;

            config = config.LoadJson(ConfigService.configFileName)!;

            client.Ready += async () =>
            {
                logger.Information("Client ready");

                await interactionService.RegisterCommandsToGuildAsync(config!.GuildId);
            };

            client.InteractionCreated += async (interaction) =>
            {
                logger.Information($"Interaction created by User: {interaction.User}");

                try
                {
                    var ctx = new SocketInteractionContext(client, interaction);
                    await interactionService.ExecuteCommandAsync(ctx, services);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex.ToString());
                    var appInfo = await client.GetApplicationInfoAsync();
                    await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");
                }
            };

            client.ButtonExecuted += async (button) =>
            {
                logger.Information($"Button executed: {button.Data.CustomId}");

                try
                {
                    var ctx = new SocketInteractionContext(client, button);
                    await interactionService.ExecuteCommandAsync(ctx, services);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex.ToString());
                    var appInfo = await client.GetApplicationInfoAsync();
                    await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");
                }
            };

            client.ModalSubmitted += async (modal) =>
            {
                logger.Information($"Modal submitted: {modal.Data.CustomId}");

                try
                {
                    var ctx = new SocketInteractionContext(client, modal);
                    await interactionService.ExecuteCommandAsync(ctx, services);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex.ToString());
                    var appInfo = await client.GetApplicationInfoAsync();
                    await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");
                }
            };
        }

        public async Task InitializeAsync()
        {
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }
    }
}
