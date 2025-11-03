using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger provider that creates JSON file loggers with buffering.
/// </summary>
public class JsonFileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;
    private readonly JsonFileLogger _logger;

    /// <summary>
    /// Initializes a new instance of the JsonFileLoggerProvider class.
    /// </summary>
    /// <param name="filePath">The path to the log file.</param>
    public JsonFileLoggerProvider(string filePath)
    {
        _filePath = filePath;
        _logger = new JsonFileLogger(filePath);
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        _logger.SetCategoryName(categoryName);
        return _logger;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _logger.Flush();
    }

    /// <summary>
    /// Flushes any buffered log entries to disk.
    /// </summary>
    public void Flush()
    {
        _logger.Flush();
    }
}
