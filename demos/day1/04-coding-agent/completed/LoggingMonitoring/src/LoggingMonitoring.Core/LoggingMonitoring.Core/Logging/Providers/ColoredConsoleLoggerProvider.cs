using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger provider that creates colored console loggers with ANSI escape codes.
/// </summary>
public class ColoredConsoleLoggerProvider : ILoggerProvider
{
    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new ColoredConsoleLogger(categoryName);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // No resources to dispose
    }
}
