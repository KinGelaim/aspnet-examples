using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.RollingFile;
using ExternalSerilog = Serilog;

namespace Logging.Serilog;

public static class LoggingConfigurator
{
    public static ExternalSerilog.ILogger GetLogger(string applicationName)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddDefaultConfigs()
            .Build();

        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment ?? "Development")
            .Enrich.WithCorrelationId()
            .ReadFrom.Configuration(configuration);

        if (configuration.GetValue<bool>("ElasticConfiguration:Enabled"))
        {
            loggerConfiguration = loggerConfiguration
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, applicationName));
        }

        return loggerConfiguration.CreateLogger();
    }

    private static IConfigurationBuilder AddDefaultConfigs(this IConfigurationBuilder configuration)
    {
        return configuration
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true,
                reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(
        IConfiguration configuration,
        string applicationName)
    {
        var indexPrefix = configuration["ElasticConfiguration:IndexPrefix"] ??
                          $"{applicationName.ToLower().Replace(".", "-")}";
        var indexDateSuffixFormat = configuration["ElasticConfiguration:IndexDateSuffixFormat"] ?? "yyyy.MM.dd";
        return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]!))
        {
            AutoRegisterTemplate = true,
            OverwriteTemplate = true,
            IndexFormat = $"{indexPrefix}-{{0:{indexDateSuffixFormat}}}",
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                               EmitEventFailureHandling.WriteToFailureSink,
            FailureSink = new RollingFileSink("./fail-{Date}.txt", new JsonFormatter(), null, null)
        };
    }
}