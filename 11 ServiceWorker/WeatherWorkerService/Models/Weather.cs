namespace WeatherWorkerService.Models;

public class Weather
{
    public int Id { get; set; }

    public string City { get; set; } = string.Empty;

    public double Temperature { get; set; }

    public int Humidity { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}