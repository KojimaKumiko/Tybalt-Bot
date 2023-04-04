using Discord.Interactions;
using Discord;
using TybaltBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using TybaltBot.Resources;
using System.Text.RegularExpressions;
using Serilog;
using Discord.Rest;

namespace TybaltBot.Modules
{
    public class ApplicationModule : InteractionModuleBase
    {
        private readonly IServiceProvider services;
        private readonly ILogger logger;
        private readonly ConfigService config;

        public ApplicationModule(IServiceProvider services)
        {
            logger = services.GetRequiredService<LoggingService>().Logger;
            config = services.GetRequiredService<ConfigService>().LoadJson(ConfigService.configFileName)!;
            this.services = services;
        }

        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }

        [SlashCommand("application", "Erzeugt ein embed mit einem Button für Bewerbungen.")]
        public async Task ApplicationCommand()
        {
            var builder = new ComponentBuilder()
                .WithButton("Bewerben", "application-button");

            var embedBuilder = new EmbedBuilder()
                .WithDescription(Resources.Application.EmbedDescription);

            await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());
            await RespondAsync("Done!", ephemeral: true);
        }

        [ComponentInteraction("application-button")]
        public async Task HandleApplicationButton()
        {
            await RespondWithModalAsync<ApplicationModal>("application-modal");
        }

        [ModalInteraction("application-modal")]
        public async Task HandleApplicationModal(ApplicationModal modal)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();

            await DeferAsync(ephemeral: true);

            logger.Information($"AccountName: {modal.AccountName}");

            var match = Regex.IsMatch(modal.AccountName, @"^[a-zA-Z\s]+\.\d{4}$");
            if (!match)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Fehler")
                    .WithDescription(Application.AccountName_Wrong)
                    .WithCurrentTimestamp();

                embed.AddField(Application.AccountName, modal.AccountName);
                await FollowupAsync(embed: embed.Build(), ephemeral: true);
                return;
            }

            logger.Information($"Reason: {modal.Reason}");
            logger.Information($"Found: {modal.Found}");
            logger.Information($"Skill: {modal.Skill}");

            RestUserMessage? botMessage = null;

            try
            {
                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(Context.User)
                    .WithTitle(Application.Title)
                    .WithDescription(Context.User.Mention)
                    .WithCurrentTimestamp()
                    .AddField(Application.AccountName, modal.AccountName)
                    .AddField(Application.Reason, modal.Reason)
                    .AddField(Application.Found, modal.Found)
                    .AddField(Application.Skill, modal.Skill);

                var channel = client.Guilds.First(g => g.Id == config!.GuildId).Channels.First(c => c.Id == config!.Channels["bewerbung"]);
                if (channel.GetChannelType() == ChannelType.Text)
                {
                    ulong roleId = config!.Roles["leitungsteam"];
                    string rolePing = $"<@&{roleId}>";
                    botMessage = await ((SocketTextChannel)channel).SendMessageAsync(rolePing, embed: embedBuilder.Build());

                    var emoteTrue = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_true") != -1);
                    var emoteFalse = client.Guilds.SelectMany(g => g.Emotes).FirstOrDefault(e => e.Name.IndexOf("raid_false") != -1);

                    if (emoteTrue != null)
                    {
                        await botMessage.AddReactionAsync(emoteTrue);
                    }
                    else
                    {
                        await botMessage.AddReactionAsync(new Emoji("\u2705")); // ✅
                    }

                    if (emoteFalse != null)
                    {
                        await botMessage.AddReactionAsync(emoteFalse);
                    }
                    else
                    {
                        await botMessage.AddReactionAsync(new Emoji("\u274C")); // ❌
                    }
                }

                var roleIds = new List<ulong> { config!.Roles["application"], config!.Roles["raids"], config!.Roles["strikes"] };
                var guild = client.Guilds.First(g => g.Id == Context.Guild.Id);
                var guildUser = guild.Users.First(u => u.Id == Context.User.Id);
                await guildUser.AddRolesAsync(roleIds);

                var successEmbed = new EmbedBuilder().WithDescription(Application.EmbedSuccess);
                var embeds = new Embed[] { embedBuilder.Build(), successEmbed.Build() };
                await Context.User.SendMessageAsync(embeds: embeds);

                await FollowupAsync(Application.ModalSuccess, ephemeral: true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                var appInfo = await client.GetApplicationInfoAsync();
                await appInfo.Owner.SendMessageAsync($"Exception: {ex}\nException Message: {ex.Message}");

                if (botMessage != null)
                {
                    await botMessage.ReplyAsync(Application.EmbedFail);
                }

                string message = string.Format(Application.ModalFail, config.Channels["questions"], appInfo.Owner.Mention);
                await FollowupAsync(message, ephemeral: true);
            }
        }

        [SlashCommand("roles", "Erzeugt ein embed mit mehreren Buttons für die verschiedenen Rollen.")]
        public async Task Roles()
        {
            var builder = new ComponentBuilder()
                .WithButton("Raids", "rolesButton_raids")
                .WithButton("Strikes", "rolesButton_strikes")
                .WithButton("Fraktale", "rolesButton_fractals")
                .WithButton("Andere Spiele", "rolesButton_otherGames");

            var embedBuilder = new EmbedBuilder()
                .WithDescription(Application.EmbedRole);

            await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());
            await RespondAsync("Done!", ephemeral: true);
        }

        [ComponentInteraction("rolesButton_*")]
        public async Task HandleRolesButtons(string role)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();

            ulong roleId = config!.Roles[role];

            var guild = client.Guilds.First(g => g.Id == Context.Guild.Id);
            var guildUser = guild.Users.First(u => u.Id == Context.User.Id);

            string message = "";
            string? roleName = guild.Roles.FirstOrDefault(r => r.Id == roleId)?.Name;

            if (guildUser.Roles.Any(r => r.Id == roleId))
            {
                await guildUser.RemoveRoleAsync(roleId);
                message = string.Format(Application.RoleRemoved, roleName);
            }
            else
            {
                await guildUser.AddRoleAsync(roleId);
                message = string.Format(Application.RoleAdded, roleName);
            }

            await RespondAsync(message, ephemeral: true);
        }

        public class ApplicationModal : IModal
        {
            public string Title => Application.Title;

            [InputLabel("Dein GW2-Accountname")]
            [ModalTextInput("account_name", TextInputStyle.Short, "name.1234")]
            public string AccountName { get; set; }

            [InputLabel("Warum möchtest du Mitglied werden?")]
            [ModalTextInput("reason", TextInputStyle.Paragraph)]
            public string Reason { get; set; }

            [InputLabel("Wie bist du auf Rising Light gestoßen?")]
            [ModalTextInput("found", TextInputStyle.Paragraph)]
            public string Found { get; set; }

            [InputLabel("Wie würdest du deine Raid-Erfahrung bewerten?")]
            [ModalTextInput("skill", TextInputStyle.Paragraph)]
            public string Skill { get; set; }
        }
    }
}
