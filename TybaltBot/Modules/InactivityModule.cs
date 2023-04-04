using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.RegularExpressions;
using TybaltBot.Resources;
using TybaltBot.Services;

namespace TybaltBot.Modules
{
    public class InactivityModule : InteractionModuleBase
    {
        private readonly IServiceProvider services;
        private readonly ILogger logger;
        private readonly ConfigService config;

        public InactivityModule(IServiceProvider services)
        {
            logger = services.GetRequiredService<LoggingService>().Logger;
            config = services.GetRequiredService<ConfigService>().LoadJson(ConfigService.configFileName)!;
            this.services = services;
        }

        [SlashCommand("inactivity", "Erzeugt ein embed mit Buttons für die Inaktivität.")]
        public async Task InactivityCommand()
        {
            var builder = new ComponentBuilder()
                .WithButton("Inaktiv melden", "inactive-button")
                .WithButton("Aktiv melden", "active-button");

            string description = string.Format(Inactivity.EmbedDescription, "<#380353536745275393>", "<@112284025783156736>"); ;

            var embedBuilder = new EmbedBuilder()
                .WithDescription(description);

            await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());
            await RespondAsync("Done!", ephemeral: true);
        }

        [ComponentInteraction("inactive-button")]
        public async Task HandleInactiveButton()
        {
            await RespondWithModalAsync<InactivityModal>("inactivity-modal");
        }

        [ModalInteraction("inactivity-modal")]
        public async Task HandleInactivityModal(InactivityModal modal)
        {
            var client = services.GetRequiredService<DiscordSocketClient>();

            await DeferAsync(ephemeral: true);

            var match = Regex.IsMatch(modal.AccountName, @"[a-zA-Z]+\.\d{4}$");
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

            logger.Information($"AccountName: {modal.AccountName}");
            logger.Information($"Duration: {modal.Duration}");
            logger.Information($"Reason: {modal.Reason}");

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithTitle(Inactivity.Title)
                .WithDescription(Context.User.Mention)
                .WithCurrentTimestamp()
                .AddField(Inactivity.AccountName, modal.AccountName)
                .AddField(Inactivity.Duration, modal.Duration)
                .AddField(Inactivity.Reason, modal.Reason)
                .WithFooter(Inactivity.Embed_Footer);

            ulong roleId = config!.Roles["inactivity"];

            var guild = client.Guilds.First(g => g.Id == Context.Guild.Id);
            var guildUser = guild.Users.First(u => u.Id == Context.User.Id);
            var channel = guild.Channels.First(c => c.Id == config!.Channels["inactivity"]);

            await guildUser.AddRoleAsync(roleId);

            if (channel.GetChannelType() == ChannelType.Text)
            {
                await ((SocketTextChannel)channel).SendMessageAsync(embed: embedBuilder.Build());
            }

            await FollowupAsync(Inactivity.Inactive_Success, ephemeral: true);
        }

        [ComponentInteraction("active-button")]
        public async Task HandleActiveButton()
        {
            var client = services.GetRequiredService<DiscordSocketClient>();

            await RespondAsync(Inactivity.Active_Success, ephemeral: true);

            ulong roleId = config!.Roles["inactivity"];

            var guild = client.Guilds.First(g => g.Id == Context.Guild.Id);
            var guildUser = guild.Users.First(u => u.Id == Context.User.Id);
            var channel = guild.Channels.First(c => c.Id == config!.Channels["inactivity"]);

            // check if the user has the inactivity role
            if (!guildUser.Roles.Any(r => r.Id == roleId))
            {
                return;
            }

            await guildUser.RemoveRoleAsync(roleId);

            if (channel.GetChannelType() == ChannelType.Text)
            {
                string message = string.Format(Inactivity.Active_Inform, Context.User.Mention);
                await ((SocketTextChannel)channel).SendMessageAsync(message);
            }
        }

        public class InactivityModal : IModal
        {
            public string Title => Inactivity.Title;
            
            [InputLabel("Dein GW2-Accountname")]
            [ModalTextInput("account_name", TextInputStyle.Short, "name.1234")]
            public string AccountName { get; set; }

            [InputLabel("Wie lange möchtest du pausieren?")]
            [ModalTextInput("duration", TextInputStyle.Short)]
            public string Duration { get; set; }

            [InputLabel("Bitte geb den Grund für die pause an")]
            [ModalTextInput("reason", TextInputStyle.Paragraph)]
            public string Reason { get; set; }
        }
    }
}
