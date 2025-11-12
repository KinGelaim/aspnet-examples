namespace Caching.InMemory.Models;

public sealed class Response
{
    public required string City { get; set; }
    public required WeatherForecast[] Weathers { get; set; }
}