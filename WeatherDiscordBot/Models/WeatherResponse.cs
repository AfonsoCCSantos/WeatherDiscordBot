namespace WeatherDiscordBot.Models
{
    public class WeatherResponse
    {
        public string City { get; set; }

        public float MaxTemperature { get; set; }

        public float MinTemperature { get; set; }

        public float CurrentTemperature { get; set; }

        public WeatherResponse(string city, float maxTemperature, float minTemperature, float currentTemperature)
        {
            City = city;
            MaxTemperature = maxTemperature;
            MinTemperature = minTemperature;
            CurrentTemperature = currentTemperature;
        }
    }
}
