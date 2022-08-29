using Discord.WebSocket;
using Newtonsoft.Json;
using WeatherDiscordBot.Models;

namespace WeatherDiscordBot
{
    public static class HelperMethods
    {
        public static async Task<GeocodingResponse> CallGeocodingAPI(this HttpClient httpClient, string city, string apiKey)
        {
            string url = GetGeocodingRequestUrl(city, apiKey);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var trimmedResult = (await response.Content.ReadAsStringAsync())[1..^1];
            var geocodingResponse = JsonConvert.DeserializeObject<GeocodingResponse>(trimmedResult);

            return await Task.FromResult(geocodingResponse);
        }

        public static async Task<OpenWeatherResponse> CallOpenWeatherAPI(this HttpClient httpClient, 
                                                                         GeocodingResponse geocodingResponse, string apiKey)
        {
            string url = GetOpenWeatherRequestUrl(geocodingResponse, apiKey);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var result = await response.Content.ReadAsStringAsync();
            var openWeatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(result);

            return await Task.FromResult(openWeatherResponse);
        }

        private static string GetGeocodingRequestUrl(string city, string apiKey)
        {
            return $"{Constants.GEOCODING_BASE_URL}q={city}&appid={apiKey}";
        }

        private static string GetOpenWeatherRequestUrl(GeocodingResponse geocodingResponse, string apiKey)
        {
            return $"{Constants.OPENWEATHER_BASE_URL}lat={geocodingResponse.lat}&lon={geocodingResponse.lon}" +
                $"&appid={apiKey}&units=metric";
        }

        public static string GetFormattedCityName(SocketSlashCommand command)
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
