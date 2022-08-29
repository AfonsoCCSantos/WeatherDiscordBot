using Discord.WebSocket;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers.WeatherIn
{
    public interface IWeatherProvider
    {
        public Task<WeatherResponse> GetWeatherInformation(SocketSlashCommand command, HttpClient httpClient);
    }
}
