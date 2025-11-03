using System.Diagnostics;
using LoggingMonitoring.Core.Models;
using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Utility class for tracking operation performance with automatic timing and metrics collection.
/// </summary>
public static class PerformanceTracker
{
    /// <summary>
    /// Tracks the performance of an operation with automatic timing, logging, and metrics collection.
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
        ILogger logger,
        string component,
        string operation,
        IMetricsCollector metricsCollector,
        IErrorTracker errorTracker,
        Func<T> func)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = func();
            stopwatch.Stop();

            var durationMs = stopwatch.Elapsed.TotalMilliseconds;

            // Record successful metric
            var metric = new PerformanceMetric(
                component,
                operation,
                durationMs,
                MemoryUsedMb: 0.5, // Placeholder for demo
                Success: true,
                ErrorCount: 0
            );
            metricsCollector.AddMetric(metric);

            // Log success with duration
            logger.LogInformation(
                "Operation '{Operation}' completed successfully (Duration: {DurationMs:F2}ms)",
                operation,
                durationMs);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var durationMs = stopwatch.Elapsed.TotalMilliseconds;

            // Record failed metric
            var metric = new PerformanceMetric(
                component,
                operation,
                durationMs,
                MemoryUsedMb: 0.5,
                Success: false,
                ErrorCount: 1
            );
            metricsCollector.AddMetric(metric);

            // Log error with duration
            logger.LogError(
                ex,
                "Operation '{Operation}' failed (Duration: {DurationMs:F2}ms)",
                operation,
                durationMs);

            // Track error
            errorTracker.RecordError(ex.GetType().Name, ex.Message, component);

            // Re-throw to maintain exception flow
            throw;
        }
    }

    /// <summary>
    /// Tracks the performance of a void operation.
    /// </summary>
    public static void TrackOperation(
        ILogger logger,
        string component,
        string operation,
        IMetricsCollector metricsCollector,
        IErrorTracker errorTracker,
        Action action)
    {
        TrackOperation<object?>(logger, component, operation, metricsCollector, errorTracker, () =>
        {
            action();
            return null;
        });
    }
}
