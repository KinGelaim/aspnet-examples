using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using RedisCaching.Models;
using System.Data.SqlTypes;
using System.Text.Json;

namespace RedisCaching.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDistributedCache _cache;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<Response> Get(string city)
    {
        _logger.LogInformation("Start Get method");

        var weathersString = await _cache.GetStringAsync(city);
        var weathers = weathersString is not null
            ? JsonSerializer.Deserialize<WeatherForecast[]>(weathersString)
            : null;

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

            var options = new DistributedCacheEntryOptions()
                // This method sets a fixed point in time at which a cache entry will expire, regardless of whether it has been accessed or not
                //.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

                // If the cache entry is accessed within this sliding expiration window, its expiration time is reset, effectively extending its lifetime
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            _logger.LogInformation($"The cache for the city is installed: {city}");

            weathersString = JsonSerializer.Serialize(weathers);
            await _cache.SetStringAsync(city, weathersString, options);
        }

        // Deleting the cache
        //_cache.Remove(city);

        return new Response
        {
            City = city,
            Weathers = weathers
        };
    }
}
