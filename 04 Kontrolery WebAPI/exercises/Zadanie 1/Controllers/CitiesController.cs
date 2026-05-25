using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CitiesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<City>>> GetAll()
    {
        return await _db.Cities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetById(int id)
    {
        var city = await _db.Cities.FindAsync(id);

        if (city == null)
            return NotFound();

        return city;
    }

    [HttpPost]
    public async Task<ActionResult<City>> Create(string cityName)
    {
        var city = new City { Name = cityName };

        _db.Cities.Add(city);
        await _db.SaveChangesAsync();

        return Ok(city);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, string cityName)
    {
        var city = await _db.Cities.FindAsync(id);

        if (city == null)
            return NotFound();

        city.Name = cityName;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var city = await _db.Cities.FindAsync(id);

        if (city == null)
            return NotFound();

        _db.Cities.Remove(city);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}