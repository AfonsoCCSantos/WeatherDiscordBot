using Discord.WebSocket;

namespace WeatherDiscordBot.CommandHandlers
{
    public abstract class CommandHandler
    {

        protected IHttpClientFactory _httpClientFactory;

        public CommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;             
        }

        public abstract Task Handle(SocketSlashCommand command);
    }
}
