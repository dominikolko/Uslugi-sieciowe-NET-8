using Microsoft.EntityFrameworkCore;
using WeatherWorkerService;
using WeatherWorkerService.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

var host = builder.Build();

host.Run();