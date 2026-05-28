using Microsoft.AspNetCore.Mvc;

namespace Zadanie1Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : ControllerBase
    {
        private static List<string> cities = new List<string>();

        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            _logger.LogInformation("Pobrano listę miast. Liczba miast: {CitiesCount}", cities.Count);

            return Ok(cities);
        }

        [HttpPost]
        public IActionResult AddCity([FromBody] CityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogWarning("Próba dodania miasta z pustą nazwą.");

                return BadRequest("Nazwa miasta nie może być pusta.");
            }

            cities.Add(request.Name);

            _logger.LogInformation("Dodano nowe miasto: {CityName}", request.Name);

            return Ok(new
            {
                message = "Miasto dodane poprawnie",
                addedCity = request.Name
            });
        }
    }

    public class CityRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}