using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger provider that creates metrics collection loggers.
/// </summary>
public class MetricsLoggerProvider : ILoggerProvider
{
    private readonly IMetricsCollector _metricsCollector;

    /// <summary>
    /// Initializes a new instance of the MetricsLoggerProvider class.
    /// </summary>
    /// <param name="metricsCollector">The metrics collector service.</param>
    public MetricsLoggerProvider(IMetricsCollector metricsCollector)
    {
        _metricsCollector = metricsCollector;
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new MetricsLogger(categoryName, _metricsCollector);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // No resources to dispose
    }
}
