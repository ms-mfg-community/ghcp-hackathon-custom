using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Extension methods for ILogger to provide convenient performance tracking APIs.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Tracks the performance of an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="logger">The logger instance.</param>
    /// <param name="component">The component executing the operation.</param>
    /// <param name="operation">The operation name.</param>
    /// <param name="metricsCollector">The metrics collector service.</param>
    /// <param name="errorTracker">The error tracker service.</param>
    /// <param name="func">The function to execute and track.</param>
    /// <returns>The result of the executed function.</returns>
    public static T TrackOperation<T>(
        this ILogger logger,
        string component,
        string operation,
        IMetricsCollector metricsCollector,
        IErrorTracker errorTracker,
        Func<T> func)
    {
        return PerformanceTracker.TrackOperation(
            logger,
            component,
            operation,
            metricsCollector,
            errorTracker,
            func);
    }

    /// <summary>
    /// Tracks the performance of a void operation.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="component">The component executing the operation.</param>
    /// <param name="operation">The operation name.</param>
    /// <param name="metricsCollector">The metrics collector service.</param>
    /// <param name="errorTracker">The error tracker service.</param>
    /// <param name="action">The action to execute and track.</param>
    public static void TrackOperation(
        this ILogger logger,
        string component,
        string operation,
        IMetricsCollector metricsCollector,
        IErrorTracker errorTracker,
        Action action)
    {
        PerformanceTracker.TrackOperation(
            logger,
            component,
            operation,
            metricsCollector,
            errorTracker,
            action);
    }
}
