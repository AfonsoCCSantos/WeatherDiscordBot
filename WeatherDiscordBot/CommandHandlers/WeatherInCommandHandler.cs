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

            string url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={Environment.GetEnvironmentVariable("ApiKey")}";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var trimmedResult = result.Substring(1, result.Length - 2);

            var response2 = JsonConvert.DeserializeObject<GeocodingResponse>(trimmedResult);

            await command.RespondAsync(response2.lon.ToString() + " " + response2.lat.ToString());

            request.Dispose();
            response.Dispose();
        }
    }
}
