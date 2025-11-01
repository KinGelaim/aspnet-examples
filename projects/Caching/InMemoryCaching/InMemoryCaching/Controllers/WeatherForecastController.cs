using InMemoryCaching.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMemoryCache _memoryCache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public Response Get(string city)
    {
        _logger.LogInformation("Start Get method");

        var weathers = _memoryCache.Get(city) as WeatherForecast[];

        if (weathers is null)
        {
            weathers = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

            var options = new MemoryCacheEntryOptions()
                // This method sets a fixed point in time at which a cache entry will expire, regardless of whether it has been accessed or not
                //.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                // If the cache entry is accessed within this sliding expiration window, its expiration time is reset, effectively extending its lifetime
                .SetSlidingExpiration(TimeSpan.FromSeconds(20))

                // It is called at the time of repeated access to the cache, and not at the time of expiration
                .RegisterPostEvictionCallback((object key, object? value, EvictionReason reason, object? state) =>
                {
                    if (state == null)
                    {
                        _logger.LogInformation("State is null");
                    }

                    _logger.LogInformation($"Key '{key}' with value '{value}' was removed because of '{reason}'");
                });

            _logger.LogInformation($"The cache for the city is installed: {city}");
            _memoryCache.Set(city, weathers, options);
        }

        // Deleting the cache
        //_memoryCache.Remove(city);

        return new Response
        {
            City = city,
            Weathers = weathers
        };
    }
}