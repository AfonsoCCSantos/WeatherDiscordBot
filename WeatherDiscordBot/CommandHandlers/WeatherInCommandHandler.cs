using Discord.WebSocket;
using Newtonsoft.Json;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers
{
    public class WeatherInCommandHandler : CommandHandler
    {
        public override async Task Handle(SocketSlashCommand command, HttpClient httpClient)
        {
            var city = command.Data.Options.First().Value.ToString();

            string url = $"{Constants.GEOCODING_BASE_URL}q={city}&appid={Environment.GetEnvironmentVariable("ApiKey")}";

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var trimmedResult = (await response.Content.ReadAsStringAsync())[1..^1];

            var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(trimmedResult);

            string openWeatherUrl = $"{Constants.OPENWEATHER_BASE_URL}lat={geocodingResponse.lat}&lon={geocodingResponse.lon}" +
                $"&appid={Environment.GetEnvironmentVariable("ApiKey")}&units=metric";

            var weatherResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, openWeatherUrl));
            var result = await weatherResponse.Content.ReadAsStringAsync();

            var weatherResponse2 = JsonConvert.DeserializeObject<OpenWeatherResponse>(result);

            await command.RespondAsync($"Temperature in {city} is {weatherResponse2.main.temp}");

            response.Dispose();
        }
    }
}
