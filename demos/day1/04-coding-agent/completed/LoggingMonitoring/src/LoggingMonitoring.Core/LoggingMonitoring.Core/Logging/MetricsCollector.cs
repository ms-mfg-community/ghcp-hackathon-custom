using System.Collections.Concurrent;
using LoggingMonitoring.Core.Models;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Collects and aggregates performance metrics by component.
/// </summary>
public class MetricsCollector : IMetricsCollector
{
    private readonly ConcurrentDictionary<string, List<PerformanceMetric>> _metrics = new();
    private readonly object _lock = new();

    /// <inheritdoc />
    public void AddMetric(PerformanceMetric metric)
    {
        _metrics.AddOrUpdate(
            metric.Component,
            _ => [metric],
            (_, list) =>
            {
                lock (_lock)
                {
                    list.Add(metric);
                    return list;
                }
            }
        );
    }

    /// <inheritdoc />
    public Dictionary<string, object> GetSummary()
    {
        var summary = new Dictionary<string, object>();

        foreach (var (component, metrics) in _metrics)
        {
            lock (_lock)
            {
                if (metrics.Count == 0) continue;

                var componentSummary = new Dictionary<string, object>
                {
                    ["total_operations"] = metrics.Count,
                    ["avg_duration_ms"] = metrics.Average(m => m.DurationMs),
                    ["success_rate"] = (metrics.Count(m => m.Success) / (double)metrics.Count) * 100,
                    ["total_errors"] = metrics.Sum(m => m.ErrorCount)
                };

                summary[component] = componentSummary;
            }
        }

        return summary;
    }
}
