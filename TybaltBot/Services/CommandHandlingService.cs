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

            config = config.LoadJsonAsync(ConfigService.configFileName).GetAwaiter().GetResult()!;

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
                logger.Debug($"Button executed: {button.Data.CustomId}");

                try
                {
                    switch (button.Data.CustomId)
                    {
                        case "application-button":
                            await HandleApplicationButton(button);
                            break;
                        case "inactive-button":
                            await HandleInactiveButton(button);
                            break;
                        case "active-button":
                            await HandleActiveButton(button);
                            break;
                    }
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
                logger.Debug($"Modal submitted: {modal.Data.CustomId}");

                try
                {
                    switch (modal.Data.CustomId)
                    {
                        case "application-modal":
                            await HandleApplicationModal(modal);
                            break;
                        case "inactivity-modal":
                            await HandleInactivityModal(modal);
                            break;
                    }
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

        private async Task HandleApplicationButton(SocketMessageComponent button)
        {
            var mb = new ModalBuilder()
                .WithTitle(Application.Title)
                .WithCustomId("application-modal")
                .AddTextInput(Application.ModalAcountName, "account_name", TextInputStyle.Short, "name.1234")
                .AddTextInput(Application.Reason, "reason", TextInputStyle.Paragraph)
                .AddTextInput(Application.Found, "found", TextInputStyle.Paragraph)
                .AddTextInput(Application.Skill, "skill", TextInputStyle.Short);

            await button.RespondWithModalAsync(mb.Build());
        }

        private async Task HandleInactiveButton(SocketMessageComponent button)
        {
            var mb = new ModalBuilder()
                .WithTitle(Inactivity.Title)
                .WithCustomId("inactivity-modal")
                .AddTextInput(Inactivity.Modal_AccountName, "account_name", TextInputStyle.Short, "name.1234")
                .AddTextInput(Inactivity.Modal_Duration, "duration", TextInputStyle.Short)
                .AddTextInput(Inactivity.Modal_Reason, "reason", TextInputStyle.Paragraph);

            await button.RespondWithModalAsync(mb.Build());
        }

        private async Task HandleActiveButton(SocketMessageComponent button)
        {
            await button.RespondAsync(Inactivity.Active_Success, ephemeral: true);

            ulong roleId = config!.Roles["inactivity"];

            var guild = client.Guilds.First(g => g.Id == button.GuildId);
            var guildUser = guild.Users.First(u => u.Id == button.User.Id);
            var channel = guild.Channels.First(c => c.Id == config!.Channels["inactivity"]);

            await guildUser.RemoveRoleAsync(roleId);

            if (channel.GetChannelType() == ChannelType.Text)
            {
                string message = string.Format(Inactivity.Active_Inform, button.User.Mention);
                await ((SocketTextChannel)channel).SendMessageAsync(message);
            }
        }

        private async Task HandleApplicationModal(SocketModal modal)
        {
            await modal.DeferAsync(ephemeral: true);
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

            logger.Information($"AccountName: {accountName}");
            logger.Information($"Reason: {reason}");
            logger.Information($"Found: {found}");
            logger.Information($"Skill: {skill}");

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(modal.User)
                .WithTitle(Application.Title)
                .WithDescription(modal.User.Mention)
                .WithCurrentTimestamp()
                .AddField(Application.AccountName, accountName)
                .AddField(Application.Reason, reason)
                .AddField(Application.Found, found)
                .AddField(Application.Skill, skill);

            var channel = client.Guilds.First(g => g.Id == config!.GuildId).Channels.First(c => c.Id == config!.Channels["bewerbung"]);
            if (channel.GetChannelType() == ChannelType.Text)
            {
                ulong roleId = config!.Roles["leitungsteam"];
                string rolePing = $"<@&{roleId}>";
                var message = await((SocketTextChannel)channel).SendMessageAsync(rolePing, embed: embedBuilder.Build());

                var emoteTrue = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_true") != -1);
                var emoteFalse = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_false") != -1);

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
                ulong roleId = config!.Roles["application"];
                var guild = client.Guilds.First(g => g.Id == modal.GuildId);
                var guildUser = guild.Users.First(u => u.Id == modal.User.Id);
                await guildUser.AddRoleAsync(roleId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                var appInfo = await client.GetApplicationInfoAsync();
                await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");
            }

            try
            {
                var successEmbed = new EmbedBuilder().WithDescription(Application.EmbedSuccess);
                var embeds = new Embed[] { successEmbed.Build(), embedBuilder.Build() };
                await modal.User.SendMessageAsync(embeds: embeds);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                var appInfo = await client.GetApplicationInfoAsync();
                await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");
            }

            await modal.FollowupAsync(Application.ModalSuccess, ephemeral: true);
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

            logger.Information($"AccountName: {accountName}");
            logger.Information($"Duration: {duration}");
            logger.Information($"Reason: {reason}");

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(modal.User)
                .WithTitle(Inactivity.Title)
                .WithDescription(modal.User.Mention)
                .WithCurrentTimestamp()
                .AddField(Inactivity.AccountName, accountName)
                .AddField(Inactivity.Duration, duration)
                .AddField(Inactivity.Reason, reason)
                .WithFooter(Inactivity.Embed_Footer);

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
