using Newtonsoft.Json;
using WeatherWorkerService.Data;
using WeatherWorkerService.Models;

namespace WeatherWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cities = new[] { "Warszawa", "Che³m", "Lublin" };
        var apiKey = _configuration["OpenWeather:ApiKey"];
        var httpClient = _httpClientFactory.CreateClient();

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var city in cities)
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=pl";

                try
                {
                    var response = await httpClient.GetAsync(url, stoppingToken);
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync(stoppingToken);
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                    if (weatherData is null)
                    {
                        _logger.LogWarning("Brak danych pogodowych dla miasta {City}", city);
                        continue;
                    }

                    var weather = weatherData.ToWeather();

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    dbContext.Weathers.Add(weather);
                    await dbContext.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation(
                        "Zapisano pogodê: {City}, {Temperature}°C, wilgotnoœæ {Humidity}%, opis: {Description}",
                        weather.City,
                        weather.Temperature,
                        weather.Humidity,
                        weather.Description);
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "B³¹d pobierania pogody dla miasta {City}", city);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Inny b³¹d dla miasta {City}", city);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}