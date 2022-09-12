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
        private readonly ConfigService configService;
        private readonly ILogger logger;
        private readonly IServiceProvider services;

        public CommandHandlingService(IServiceProvider services)
        {
            interactionService = services.GetRequiredService<InteractionService>();
            client = services.GetRequiredService<DiscordSocketClient>();
            configService = services.GetRequiredService<ConfigService>();
            logger = services.GetRequiredService<LoggingService>().Logger;
            this.services = services;

            var config = configService.LoadJsonAsync(ConfigService.configFileName).GetAwaiter().GetResult();

            client.Ready += async () =>
            {
                logger.Debug("Client ready");

                await interactionService.RegisterCommandsToGuildAsync(config!.GuildId);
            };

            client.InteractionCreated += async (x) =>
            {
                logger.Debug("Interaction created");
                var ctx = new SocketInteractionContext(client, x);
                await interactionService.ExecuteCommandAsync(ctx, services);
            };

            client.ButtonExecuted += async (button) =>
            {
                logger.Debug("Button executed");

                ModalBuilder mb;
                switch (button.Data.CustomId)
                {
                    case "application-button":
                        mb = new ModalBuilder()
                            .WithTitle(Application.Title)
                            .WithCustomId("application-modal")
                            .AddTextInput(Application.ModalAcountName, "account_name", TextInputStyle.Short, "name.1234")
                            .AddTextInput(Application.Reason, "reason", TextInputStyle.Paragraph)
                            .AddTextInput(Application.Found, "found", TextInputStyle.Paragraph)
                            .AddTextInput(Application.Skill, "skill", TextInputStyle.Short);

                        await button.RespondWithModalAsync(mb.Build());
                        break;
                    case "inactive-button":
                        mb = new ModalBuilder()
                            .WithTitle(Inactivity.Title)
                            .WithCustomId("inactivity-modal")
                            .AddTextInput(Inactivity.Modal_AccountName, "account_name", TextInputStyle.Short, "name.1234")
                            .AddTextInput(Inactivity.Modal_Duration, "duration", TextInputStyle.Short)
                            .AddTextInput(Inactivity.Modal_Reason, "reason", TextInputStyle.Paragraph);

                        await button.RespondWithModalAsync(mb.Build());
                        break;
                    case "active-button":
                        await button.RespondAsync(Inactivity.Active_Success, ephemeral: true);

                        ulong roleId = config!.Roles["inactivity"];

                        var guild = client.Guilds.First(g => g.Id == button.GuildId);
                        var guildUser = guild.Users.First(u => u.Id == button.User.Id);
                        var channel = guild.Channels.First(c => c.Id == config!.Channels["inactivity"]);

                        await guildUser.RemoveRoleAsync(roleId);

                        if (channel.GetChannelType() == ChannelType.Text)
                        {
                            string message = button.User.Mention + " ist nicht mehr inaktiv!";
                            await ((SocketTextChannel)channel).SendMessageAsync(message);
                        }
                        break;
                }
            };

            client.ModalSubmitted += async (modal) =>
            {
                logger.Debug("Modal submitted");

                switch (modal.Data.CustomId)
                {
                    case "application-modal":
                        await HandleApplicationModal(modal);
                        break;
                    case "inactivity-modal":
                        await HandleInactivityModal(modal);
                        break;
                }
            };
        }

        public async Task InitializeAsync()
        {
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleApplicationModal(SocketModal modal)
        {
            var components = modal.Data.Components.ToList();

            string accountName = components.First(x => x.CustomId == "account_name").Value;
            string reason = components.First(x => x.CustomId == "reason").Value;
            string found = components.First(x => x.CustomId == "found").Value;
            string skill = components.First(x => x.CustomId == "skill").Value;

            var match = Regex.IsMatch(accountName, @"[a-zA-Z]+\.\d{4}$");
            if (!match)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Fehler")
                    .WithDescription(Application.AccountName_Wrong)
                    .WithCurrentTimestamp();
                await modal.RespondAsync(embed: embed.Build(), ephemeral: true);
                return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(modal.User)
                .WithTitle(Application.Title)
                .WithDescription(modal.User.Mention)
                .WithCurrentTimestamp()
                .AddField(Application.AccountName, accountName)
                .AddField(Application.Reason, reason)
                .AddField(Application.Found, found)
                .AddField(Application.Skill, skill);

            var config = await configService.LoadJsonAsync(ConfigService.configFileName);
            var channel = client.Guilds.First(g => g.Id == config!.GuildId).Channels.First(c => c.Id == config!.Channels["bewerbung"]);
            if (channel.GetChannelType() == ChannelType.Text)
            {
                ulong roleId = config!.Roles["leitungsteam"];
                string rolePing = $"<@&{roleId}>";
                var message = await((SocketTextChannel)channel).SendMessageAsync(rolePing, embed: embedBuilder.Build());

                var emoteTrue = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_true") != -1);
                var emoteFalse = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_false") != -1);
                //await message.AddReactionsAsync(new[] { emoteTrue, emoteFalse });

                if (emoteTrue != null)
                {
                    await message.AddReactionAsync(emoteTrue);
                }
                else
                {
                    await message.AddReactionAsync(new Emoji("\u2705")); // ✅
                }

                if (emoteFalse != null)
                {
                    await message.AddReactionAsync(emoteFalse);
                }
                else
                {
                    await message.AddReactionAsync(new Emoji("\u274C")); // ❌
                }
            }

            try
            {
                await modal.User.SendMessageAsync(embed: embedBuilder.Build());
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            await modal.RespondAsync(Application.ModalSuccess, ephemeral: true);
        }

        private async Task HandleInactivityModal(SocketModal modal)
        {
            var components = modal.Data.Components.ToList();

            string accountName = components.First(x => x.CustomId == "account_name").Value;
            string duration = components.First(x => x.CustomId == "duration").Value;
            string reason = components.First(x => x.CustomId == "reason").Value;

            var match = Regex.IsMatch(accountName, @"[a-zA-Z]+\.\d{4}$");
            if (!match)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Fehler")
                    .WithDescription(Application.AccountName_Wrong)
                    .WithCurrentTimestamp();
                await modal.RespondAsync(embed: embed.Build(), ephemeral: true);
                return;
            }

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(modal.User)
                .WithTitle(Inactivity.Title)
                .WithDescription(modal.User.Mention)
                .WithCurrentTimestamp()
                .AddField(Inactivity.AccountName, accountName)
                .AddField(Inactivity.Duration, duration)
                .AddField(Inactivity.Reason, reason)
                .WithFooter(Inactivity.Embed_Footer);

            var config = await configService.LoadJsonAsync(ConfigService.configFileName);
            ulong roleId = config!.Roles["inactivity"];

            var guild = client.Guilds.First(g => g.Id == modal.GuildId);
            var guildUser = guild.Users.First(u => u.Id == modal.User.Id);
            var channel = guild.Channels.First(c => c.Id == config!.Channels["inactivity"]);

            await guildUser.AddRoleAsync(roleId);

            if (channel.GetChannelType() == ChannelType.Text)
            {
                await ((SocketTextChannel)channel).SendMessageAsync(embed: embedBuilder.Build());
            }

            await modal.RespondAsync(Inactivity.Inactive_Success, ephemeral: true);
        }
    }
}
