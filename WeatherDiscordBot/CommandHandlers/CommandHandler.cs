using Discord.WebSocket;

namespace WeatherDiscordBot.CommandHandlers
{
    public abstract class CommandHandler
    {
        public abstract Task Handle(SocketSlashCommand command);
    }
}
