using Discord.WebSocket;

namespace WeatherDiscordBot.CommandHandlers
{
    public class WeatherInCommandHandler : CommandHandler
    {

        public override async Task Handle(SocketSlashCommand command)
        {
            await command.RespondAsync("Hello!");
        }
    }
}
