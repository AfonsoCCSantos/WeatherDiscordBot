using Discord.WebSocket;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot.CommandHandlers.WeatherIn
{
    public class OpenWeatherApiAdapter : IWeatherProvider
    {
        public async Task<WeatherResponse> GetWeatherInformation(SocketSlashCommand command, HttpClient httpClient)
        {
            var city = HelperMethods.GetFormattedCityName(command);
            var apiKey = Environment.GetEnvironmentVariable("OpenWeatherApiKey");

            var geocodingResponse = await httpClient.CallGeocodingAPI(city, apiKey);
            var openWeatherResponse = await httpClient.CallOpenWeatherAPI(geocodingResponse, apiKey);

            return await Task.FromResult(TranslateToWeatherResponse(openWeatherResponse, city));
        }

        private WeatherResponse TranslateToWeatherResponse(OpenWeatherResponse response, string city)
        {
            var weatherResponse = new WeatherResponse(city,
                                                      response.main.temp_max,
                                                      response.main.temp_min,
                                                      response.main.temp);
            return weatherResponse;
        }
    }
}
