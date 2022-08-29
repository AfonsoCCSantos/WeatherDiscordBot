using Discord.WebSocket;
using System.Text;
using WeatherDiscordBot.CommandHandlers.WeatherIn;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers
{
    public class WeatherInCommandHandler : CommandHandler
    {
        public override async Task Handle(SocketSlashCommand command, HttpClient httpClient)
        {
            IWeatherProvider weatherProvider = new OpenWeatherApiAdapter();
            WeatherResponse weatherResponse = await weatherProvider.GetWeatherInformation(command, httpClient);
            await command.RespondAsync(GetFormattedWeatherResponse(weatherResponse));
        }
        
        private string GetFormattedWeatherResponse(WeatherResponse weather)
        {
            StringBuilder sb = new();
            sb.AppendLine($"Today's weather for {weather.City}:");
            sb.AppendLine($"   -Current Temperature: {(int) (weather.CurrentTemperature + 0.5)}");
            sb.AppendLine($"   -Highest Temperature: {(int) (weather.MaxTemperature + 0.5)}");
            sb.AppendLine($"   -Lowest Temperature: {(int) (weather.MinTemperature + 0.5)}");
            return sb.ToString();
        }
    }
}
