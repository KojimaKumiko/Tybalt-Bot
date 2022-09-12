using Discord.Interactions;
using Discord;

namespace TybaltBot.Modules
{
    public class ApplicationModule : InteractionModuleBase
    {
        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }

        [SlashCommand("application", "Erzeugt ein embed mit einem Button für Bewerbungen.")]
        public async Task Application()
        {
            var builder = new ComponentBuilder()
                .WithButton("Bewerben", "application-button");

            var embedBuilder = new EmbedBuilder()
                .WithDescription(Resources.Application.EmbedDescription);

            await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());
            await RespondAsync("Done!", ephemeral: true);
        }
    }
}
