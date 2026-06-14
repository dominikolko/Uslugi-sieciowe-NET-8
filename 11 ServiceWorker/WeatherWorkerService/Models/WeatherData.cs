using Newtonsoft.Json;

namespace WeatherWorkerService.Models;

public class WeatherData
{
    [JsonProperty("name")]
    public string City { get; set; } = string.Empty;

    [JsonProperty("main")]
    public MainData Main { get; set; } = new();

    [JsonProperty("weather")]
    public List<WeatherDescription> Weather { get; set; } = [];
}

public class MainData
{
    [JsonProperty("temp")]
    public double Temperature { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }
}

public class WeatherDescription
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}