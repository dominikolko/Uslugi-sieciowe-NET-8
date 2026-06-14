namespace WeatherWorkerService.Models;

public static class WeatherMapper
{
    public static Weather ToWeather(this WeatherData weatherData)
    {
        return new Weather
        {
            City = weatherData.City,
            Temperature = weatherData.Main.Temperature,
            Humidity = weatherData.Main.Humidity,
            Description = weatherData.Weather.FirstOrDefault()?.Description ?? string.Empty,
            CreatedAt = DateTime.Now
        };
    }
}