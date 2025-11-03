using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger that outputs to console with ANSI color coding based on log level.
/// </summary>
public class ColoredConsoleLogger : ILogger
{
    private readonly string _categoryName;

    private static readonly Dictionary<LogLevel, string> LevelColors = new()
    {
        [LogLevel.Trace] = "\x1b[37m",       // White
        [LogLevel.Debug] = "\x1b[36m",       // Cyan
        [LogLevel.Information] = "\x1b[32m", // Green
        [LogLevel.Warning] = "\x1b[33m",     // Yellow
        [LogLevel.Error] = "\x1b[31m",       // Red
        [LogLevel.Critical] = "\x1b[1;31m"   // Bold Red
    };

    private const string ResetColor = "\x1b[0m";

    /// <summary>
    /// Initializes a new instance of the ColoredConsoleLogger class.
    /// </summary>
    /// <param name="categoryName">The category name for this logger.</param>
    public ColoredConsoleLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var color = LevelColors.GetValueOrDefault(logLevel, "");
        var timestamp = DateTime.Now.ToString("O");
        var levelName = logLevel.ToString().ToUpper();
        var component = _categoryName.Split('.').LastOrDefault() ?? _categoryName;
        var message = formatter(state, exception);

        Console.WriteLine($"{color}[{timestamp}] {levelName,-11} | {component,-15} | {message}{ResetColor}");

        // Print exception details if present
        if (exception != null)
        {
            Console.WriteLine($"{color}  Error: {exception.Message}{ResetColor}");
        }
    }
}
