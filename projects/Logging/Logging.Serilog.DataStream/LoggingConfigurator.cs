using Elastic.Channels;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Serilog;
using ExternalSerilog = Serilog;

namespace Logging.Serilog.DataStream;

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
            .ReadFrom.Configuration(configuration);

        if (configuration.GetValue<bool>("ElasticConfiguration:Enabled"))
        {
            loggerConfiguration = loggerConfiguration
                .WriteTo.FallbackChain(
                    fc => fc.Elasticsearch(
                        [new Uri(configuration["ElasticConfiguration:Uri"]!)],
                        options =>
                        {
                            options.BootstrapMethod = BootstrapMethod.Silent;
                            options.DataStream = new DataStreamName(
                                "logs",
                                configuration["ElasticConfiguration:IndexPrefix"] ??
                                    $"{applicationName.ToLower().Replace(".", "-")}",
                                environment ?? "Development");
                            options.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>();
                            options.ConfigureChannel = channelOptions =>
                            {
                                channelOptions.BufferOptions = new BufferOptions();
                            };
                        }),
                    fc => fc.File("./fail-.txt", rollingInterval: RollingInterval.Day));
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
}