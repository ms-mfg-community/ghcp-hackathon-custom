using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Models;

/// <summary>
/// Represents a single log entry with all relevant metadata.
/// </summary>
/// <param name="Timestamp">The ISO 8601 timestamp when the log entry was created.</param>
/// <param name="Level">The log severity level.</param>
/// <param name="Component">The component or subsystem that generated the log.</param>
/// <param name="Message">The log message content.</param>
/// <param name="DurationMs">Optional operation duration in milliseconds.</param>
/// <param name="ErrorDetails">Optional error details or exception message.</param>
/// <param name="Metadata">Optional additional metadata as key-value pairs.</param>
public record LogEntryModel(
    string Timestamp,
    LogLevel Level,
    string Component,
    string Message,
    double? DurationMs = null,
    string? ErrorDetails = null,
    Dictionary<string, object>? Metadata = null
);
