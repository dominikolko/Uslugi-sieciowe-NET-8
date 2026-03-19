using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/weather", async (string city, IConfiguration config) =>
{
    var apiKey = config["OpenWeather:ApiKey"];

    if (string.IsNullOrWhiteSpace(city))
        return Results.BadRequest("Podaj miasto np. /weather?city=Warsaw");

    if (string.IsNullOrWhiteSpace(apiKey))
        return Results.Problem("Brak klucza API");

    using var httpClient = new HttpClient();

    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";

    var response = await httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return Results.BadRequest("Nie znaleziono miasta lub b³¹d API");

    var json = await response.Content.ReadAsStringAsync();
    var data = System.Text.Json.JsonSerializer.Deserialize<WeatherResponse>(json);

    if (data is null)
        return Results.Problem("Nie uda³o siê odczytaæ danych pogodowych");

    return Results.Ok(new
    {
        miasto = data.Name,
        temperatura = data.Main?.Temp,
        opis = data.Weather?.FirstOrDefault()?.Description ?? "brak opisu"
    });
});

app.Run();

class WeatherResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("main")]
    public MainData? Main { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherData>? Weather { get; set; }
}

class MainData
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
}

class WeatherData
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}