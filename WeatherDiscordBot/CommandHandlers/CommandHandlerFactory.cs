namespace WeatherDiscordBot.CommandHandlers
{
    public class CommandHandlerFactory
    {
        public static CommandHandler GetCommandHandler(string name)
        {
            return name switch
            {
                _ => throw new ArgumentException($"No Command named {name}"),
            };
        }
    }
}
