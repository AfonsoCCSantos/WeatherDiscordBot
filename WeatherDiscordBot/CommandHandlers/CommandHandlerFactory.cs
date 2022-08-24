namespace WeatherDiscordBot.CommandHandlers
{
    public class CommandHandlerFactory
    {
        public static CommandHandler GetCommandHandler(string name)
        {
            return name switch
            {
                ("weather-in") => new WeatherInCommandHandler(),
                _ => throw new ArgumentException($"No Command named {name}"),
            };
        }
    }
}
