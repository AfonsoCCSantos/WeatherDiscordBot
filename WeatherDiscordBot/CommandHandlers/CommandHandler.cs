using Discord.WebSocket;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers
{
    public abstract class CommandHandler
    {
        public abstract Task Handle(SocketSlashCommand command, HttpClient httpClient);
    }
}
