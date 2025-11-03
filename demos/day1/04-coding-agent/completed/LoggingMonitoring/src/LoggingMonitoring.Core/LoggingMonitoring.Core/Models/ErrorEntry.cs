namespace LoggingMonitoring.Core.Models;

/// <summary>
/// Represents an error entry for tracking and aggregation.
/// </summary>
/// <param name="Timestamp">The ISO 8601 timestamp when the error occurred.</param>
/// <param name="Type">The error type or exception name.</param>
/// <param name="Message">The error message.</param>
/// <param name="Component">The component or subsystem where the error occurred.</param>
public record ErrorEntry(
    string Timestamp,
    string Type,
    string Message,
    string Component
);
