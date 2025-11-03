using System.Collections.Concurrent;
using LoggingMonitoring.Core.Models;

namespace LoggingMonitoring.Core.Logging;

/// <summary>
/// Tracks and aggregates errors with bounded history using FIFO eviction.
/// </summary>
public class ErrorTracker : IErrorTracker
{
    private readonly Queue<ErrorEntry> _errors = new();
    private readonly ConcurrentDictionary<string, int> _errorCounts = new();
    private readonly int _maxHistory;
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the ErrorTracker class.
    /// </summary>
    /// <param name="maxHistory">Maximum number of error entries to retain (default: 100).</param>
    public ErrorTracker(int maxHistory = 100)
    {
        _maxHistory = maxHistory;
    }

    /// <inheritdoc />
    public void RecordError(string errorType, string message, string component)
    {
        var errorEntry = new ErrorEntry(
            DateTime.Now.ToString("O"),
            errorType,
            message,
            component
        );

        lock (_lock)
        {
            _errors.Enqueue(errorEntry);

            // Maintain bounded queue with FIFO eviction
            while (_errors.Count > _maxHistory)
            {
                _errors.Dequeue();
            }
        }

        // Increment error count by type
        _errorCounts.AddOrUpdate(errorType, 1, (_, count) => count + 1);
    }

    /// <inheritdoc />
    public Dictionary<string, object> GetErrorSummary()
    {
        lock (_lock)
        {
            var recentErrors = _errors.TakeLast(5).ToList();

            return new Dictionary<string, object>
            {
                ["total_errors"] = _errorCounts.Values.Sum(),
                ["error_types"] = new Dictionary<string, int>(_errorCounts),
                ["recent_errors"] = recentErrors
            };
        }
    }
}
