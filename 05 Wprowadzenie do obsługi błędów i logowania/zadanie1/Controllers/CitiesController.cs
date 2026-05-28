using Microsoft.AspNetCore.Mvc;

namespace Zadanie1Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : ControllerBase
    {
        private static List<string> cities = new List<string>();

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(cities);
        }

        [HttpPost]
        public IActionResult AddCity([FromBody] CityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Nazwa miasta nie może być pusta.");
            }

            cities.Add(request.Name);

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