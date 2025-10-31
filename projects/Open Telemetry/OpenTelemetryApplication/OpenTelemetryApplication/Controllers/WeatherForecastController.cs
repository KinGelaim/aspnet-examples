using Microsoft.AspNetCore.Mvc;

namespace OpenTelemetryApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WeatherMetrics _weatherMetrics;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        WeatherMetrics weatherMetrics)
    {
        _logger = logger;
        _weatherMetrics = weatherMetrics;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("Gather weather information");
        _weatherMetrics.WeatherRequestCounter.Add(1,
            new KeyValuePair<string, object?>("operation", "GetWeatherForecast"),
            new KeyValuePair<string, object?>("controller", nameof(WeatherForecastController)));

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
