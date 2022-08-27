namespace WeatherDiscordBot.Models
{
    public class HttpClientHolder
    {
        public HttpClient OpenWeatherClient { get; set; }

        public HttpClient GeocodigClient { get; set; }

        public HttpClientHolder()
        {
            OpenWeatherClient = new HttpClient();
            GeocodigClient = new HttpClient();
        }

    }
}
