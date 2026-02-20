var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseHttpsRedirection();

var windDirections = new[]
{
    "N", "NE", "E", "SE", "S", "SW", "W", "NW"
};

app.MapGet("/temperature", () =>
{
    var temp = Random.Shared.Next(-20, 41); 
    return Results.Ok(temp);
});

app.MapGet("/wind-direction", () =>
{
    var direction = windDirections[Random.Shared.Next(windDirections.Length)];
    return Results.Ok(direction);
});

app.Run();