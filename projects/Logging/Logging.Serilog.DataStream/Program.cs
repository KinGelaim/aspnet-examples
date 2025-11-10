using Serilog;
using Logging.Serilog.DataStream;

var appName = typeof(Program).Assembly.GetName().Name!;
var logger = LoggingConfigurator.GetLogger(appName);
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ExampleHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .MapGet("/create-all-logs-levels", (ExampleHandler handler) =>
    {
        return handler.HandleRequest();
    })
    .WithName("CreateAllLogsLevels")
    .WithOpenApi();

app.Run();

partial class ExampleHandler(ILogger<ExampleHandler> logger)
{
    public string HandleRequest()
    {
        var _ = logger.BeginScope("Serilog");

        logger.LogTrace("Trace message");
        logger.LogDebug("Debug message");
        logger.LogInformation("Information message");
        logger.LogWarning("Warning message");
        logger.LogError("Error message");
        logger.LogCritical("Critical message");

        return "All levels of logs have been created";
    }
}