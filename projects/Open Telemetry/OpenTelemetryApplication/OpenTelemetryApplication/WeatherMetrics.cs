using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenTelemetryApplication;

public sealed class WeatherMetrics : IDisposable
{
    public const string MeterName = "WeatherApp.Api";

    public ActivitySource Tracer { get; }
    public Meter Recorder { get; }
    public Counter<long> WeatherRequestCounter { get; }

    public WeatherMetrics()
    {
        var version = typeof(WeatherMetrics).Assembly.GetName().Version?.ToString();
        Tracer = new ActivitySource(MeterName, version);
        Recorder = new Meter(MeterName, version);
        WeatherRequestCounter = Recorder.CreateCounter<long>("app.incoming.requests",
            description: "The number of incoming requests to the backend API");
    }

    public void Dispose()
    {
        Tracer.Dispose();
        Recorder.Dispose();
    }
}