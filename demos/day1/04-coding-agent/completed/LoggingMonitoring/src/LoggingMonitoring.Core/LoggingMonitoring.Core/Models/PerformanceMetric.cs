namespace LoggingMonitoring.Core.Models;

/// <summary>
/// Represents performance metrics for a tracked operation.
/// </summary>
/// <param name="Component">The component or subsystem that executed the operation.</param>
/// <param name="Operation">The name of the operation being tracked.</param>
/// <param name="DurationMs">The operation duration in milliseconds.</param>
/// <param name="MemoryUsedMb">The estimated memory used in megabytes.</param>
/// <param name="Success">Whether the operation completed successfully.</param>
/// <param name="ErrorCount">The number of errors encountered during the operation.</param>
public record PerformanceMetric(
    string Component,
    string Operation,
    double DurationMs,
    double MemoryUsedMb,
    bool Success,
    int ErrorCount = 0
);
