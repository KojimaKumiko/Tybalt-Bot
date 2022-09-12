using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TybaltBot.Modules
{
    public class InactivityModule : InteractionModuleBase
    {
        [SlashCommand("inactivity", "Erzeugt ein embed mit Buttons für die Inaktivität.")]
        public async Task Inactivity()
        {
            var builder = new ComponentBuilder()
                .WithButton("Inaktiv melden", "inactive-button")
                .WithButton("Aktiv melden", "active-button");

            string description = string.Format(Resources.Inactivity.EmbedDescription, "<#380353536745275393>", "<@112284025783156736>"); ;

            var embedBuilder = new EmbedBuilder()
                .WithDescription(description);

            await ReplyAsync(embed: embedBuilder.Build(), components: builder.Build());
            await RespondAsync("Done!", ephemeral: true);
        }
    }
}
