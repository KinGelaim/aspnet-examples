using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToker) =>
    {
        document.Info.Version = "1.0.0";
        document.Info.Title = "OpenAPI.Scalar";
        document.Info.Description = "Description of the documentation";
        document.Info.TermsOfService = new Uri("https://mycompany.ru/terms");
        document.Info.Contact = new OpenApiContact
        {
            Name = "Test Testovich",
            Email = "ttestovich@mycompany.ru",
            Url = new Uri("https://mycompany.ru/")
        };

        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };

        document.Servers?.Add(new OpenApiServer
        {
            Url = "https://localhost:7289",
            Description = "Description of the service"
        });

        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Purple);
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app
    .MapGet("/weather-forecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast") // Идентификатор операции
    .WithSummary("Getting a weather forecast") // Заголовок
    .WithDescription("The method that returns the weather forecast")  // Полное описание
    .WithTags("WeatherForecast")  // Теги для категоризации
    .Produces<WeatherForecast[]>(200)  // Статус ответа 200 и возвращаемый тип данных
    .Produces(400)  // Статус ответа 400
    .ProducesProblem(500);  // Статус ответа 500 при неуспешной операции
    //.RequireAuthorization();  // Необходимость авторизации

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}