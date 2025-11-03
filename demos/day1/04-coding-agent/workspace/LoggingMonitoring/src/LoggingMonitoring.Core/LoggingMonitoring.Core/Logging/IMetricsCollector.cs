using LoggingMonitoring.Core.Models;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Interface for collecting and aggregating performance metrics.
/// </summary>
public interface IMetricsCollector
{
    /// <summary>
    /// Adds a performance metric to the collector.
    /// </summary>
    /// <param name="metric">The performance metric to add.</param>
    void AddMetric(PerformanceMetric metric);

    /// <summary>
    /// Gets a summary of all collected metrics aggregated by component.
    /// </summary>
    /// <returns>A dictionary containing metrics summary statistics.</returns>
    Dictionary<string, object> GetSummary();
}
