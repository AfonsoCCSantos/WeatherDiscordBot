using Discord;

namespace WeatherDiscordBot.Models
{
    public class SlashCommandModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<SlashCommandOptionBuilder>? Options { get; set; }

        public SlashCommandModel(string name, string description, List<SlashCommandOptionBuilder> options)
        {
            Name = name;
            Description = description;
            Options = options;
        }

        public SlashCommandModel(string name, string description)
        {
            Name = name;
            Description = description;
            Options = null;
        }
    }
}
