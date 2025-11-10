var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The priority of logging settings is given to appsettings
builder.Logging.SetMinimumLevel(LogLevel.Warning); // Minimum logging level

// Configurable logging filters
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Show only the warning log and above from everything that contains Microsoft
builder.Logging.AddFilter("AppLogger", LogLevel.Warning); // Pretty much show everything from AppLogger

builder.Logging.AddConsole();
/*builder.Logging.AddJsonConsole(options =>
{
    options.JsonWriterOptions = new JsonWriterOptions { Indented = true };
});*/

builder.Services.AddSingleton<ExampleHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .MapGet("/create-all-logs-levels", () =>
    {
        var logger = app.Logger;
        logger.LogTrace("Trace message");
        logger.LogDebug("Debug message");
        logger.LogInformation("Information message");
        logger.LogWarning("Warning message");
        logger.LogError("Error message");
        logger.LogCritical("Critical message");
        return "All levels of logs have been created";
    })
    .WithName("CreateAllLogsLevels")
    .WithOpenApi();

var handler = app.Services.GetRequiredService<ExampleHandler>();
app
    .MapGet("/create-log-with-di-and-source-code-generation", handler.HandleRequest)
    .WithName("CreateLogWithDiAndSourceCodeGeneration")
    .WithOpenApi();

app.Run();

partial class ExampleHandler(ILogger<ExampleHandler> logger)
{
    public string HandleRequest()
    {
        LogHandleRequest(logger, "my value");
        return "Hello World";
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "ExampleHandler.HandleRequest was called with value: {value}")]
    static partial void LogHandleRequest(ILogger logger, string value);
}