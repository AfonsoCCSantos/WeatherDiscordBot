using Discord.WebSocket;
namespace WeatherDiscordBot.CommandHandlers
{
    public class WeatherInCommandHandler : CommandHandler
    {
        public override async Task Handle(SocketSlashCommand command, HttpClient httpClient)
        {
            var city = command.Data.Options.First().Value.ToString();
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");

            var geocodingResponse = await httpClient.CallGeocodingAPI(city, apiKey);
            var openWeatherResponse = await httpClient.CallOpenWeatherAPI(geocodingResponse, apiKey);

            await command.RespondAsync($"Temperature in {city} is {openWeatherResponse.main.temp}");
        }
    }
}
