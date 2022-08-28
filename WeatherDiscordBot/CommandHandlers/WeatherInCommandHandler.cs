using Discord.WebSocket;
using System.Text;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers
{
    public class WeatherInCommandHandler : CommandHandler
    {
        public override async Task Handle(SocketSlashCommand command, HttpClient httpClient)
        {
            var city = GetFormattedCityName(command);
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");

            var geocodingResponse = await httpClient.CallGeocodingAPI(city, apiKey);
            var openWeatherResponse = await httpClient.CallOpenWeatherAPI(geocodingResponse, apiKey);

            await command.RespondAsync(GetFormattedWeatherResponse(openWeatherResponse, city));
        }
        
        private string GetFormattedWeatherResponse(OpenWeatherResponse openWeatherResponse, string city)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Today's weather for {city}:");
            sb.AppendLine($" Current Temperature: {openWeatherResponse.main.temp}");
            sb.AppendLine($" Highest Temperature: {openWeatherResponse.main.temp_max}");
            sb.AppendLine($" Lowest Temperature: {openWeatherResponse.main.temp_min}");
            return sb.ToString();
        }

        private static string GetFormattedCityName(SocketSlashCommand command)
        {
            var cityName = command.Data.Options.First().Value.ToString();
            if (cityName is not null)
            {
                return FirstLetterCapitalCase(cityName.ToLower());
            }
            else
            {
                throw new ArgumentException("No city name was provided!");
            }
        }

        private static string FirstLetterCapitalCase(string toFormat)
        {
            char firstLetter = toFormat[0];
            string formattedString = toFormat.Substring(1).Insert(0, firstLetter.ToString().ToUpper());
            return formattedString;
        }
    }
}
