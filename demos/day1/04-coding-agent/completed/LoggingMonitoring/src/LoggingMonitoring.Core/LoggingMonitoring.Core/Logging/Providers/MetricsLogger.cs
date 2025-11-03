using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger that processes log entries for metrics collection.
/// This logger doesn't output logs but extracts metrics from log messages.
/// </summary>
public class MetricsLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IMetricsCollector _metricsCollector;

    /// <summary>
    /// Initializes a new instance of the MetricsLogger class.
    /// </summary>
    /// <param name="categoryName">The category name for this logger.</param>
    /// <param name="metricsCollector">The metrics collector service.</param>
    public MetricsLogger(string categoryName, IMetricsCollector metricsCollector)
    {
        _categoryName = categoryName;
        _metricsCollector = metricsCollector;
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        // Metrics are added through the PerformanceTracker, not directly through this logger
        // This logger serves as a placeholder for the metrics provider
    }
}
