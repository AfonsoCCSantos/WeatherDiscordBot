using Newtonsoft.Json;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot
{
    public static class WeatherExtensions
    {
        public static async Task<GeocodingResponse> CallGeocodingAPI(this HttpClient httpClient, string city, string apiKey)
        {
            string url = $"{Constants.GEOCODING_BASE_URL}q={city}&appid={apiKey}";
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var trimmedResult = (await response.Content.ReadAsStringAsync())[1..^1];
            var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(trimmedResult);

            return await Task.FromResult(geocodingResponse);
        }

        public static async Task<OpenWeatherResponse> CallOpenWeatherAPI(this HttpClient httpClient, 
                                                                         GeocodingResponse geocodingResponse, string apiKey)
        {
            string url = $"{Constants.OPENWEATHER_BASE_URL}lat={geocodingResponse.lat}&lon={geocodingResponse.lon}" +
                $"&appid={apiKey}&units=metric";
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var result = await response.Content.ReadAsStringAsync();
            var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(result);

            return await Task.FromResult(openWeatherResponse);
        }

    }
}
