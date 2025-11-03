using LoggingMonitoring.Core.Models;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Interface for tracking and aggregating errors across the application.
/// </summary>
public interface IErrorTracker
{
    /// <summary>
    /// Records an error entry for tracking and aggregation.
    /// </summary>
    /// <param name="errorType">The error type or exception name.</param>
    /// <param name="message">The error message.</param>
    /// <param name="component">The component where the error occurred.</param>
    void RecordError(string errorType, string message, string component);

    /// <summary>
    /// Gets a summary of all tracked errors including counts and recent entries.
    /// </summary>
    /// <returns>A dictionary containing error summary statistics.</returns>
    Dictionary<string, object> GetErrorSummary();
}
