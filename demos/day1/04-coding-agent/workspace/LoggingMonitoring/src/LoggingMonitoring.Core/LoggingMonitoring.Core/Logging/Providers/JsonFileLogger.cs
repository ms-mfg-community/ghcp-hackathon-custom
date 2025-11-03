using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace LoggingMonitoring.Core.Logging.Providers;

/// <summary>
/// Logger that writes JSON-formatted log entries to a file with buffering.
/// </summary>
public class JsonFileLogger : ILogger
{
    private readonly string _filePath;
    private readonly List<object> _buffer = new();
    private readonly object _lock = new();
    private const int BufferSize = 10;
    private string _categoryName = string.Empty;

    /// <summary>
    /// Initializes a new instance of the JsonFileLogger class.
    /// </summary>
    /// <param name="filePath">The path to the log file.</param>
    public JsonFileLogger(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Sets the category name for subsequent log entries.
    /// </summary>
    /// <param name="categoryName">The category name.</param>
    public void SetCategoryName(string categoryName)
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

        var component = _categoryName.Split('.').LastOrDefault() ?? _categoryName;
        var logEntry = new
        {
            timestamp = DateTime.Now.ToString("O"),
            level = logLevel.ToString(),
            component,
            message = formatter(state, exception),
            error_details = exception?.Message
        };

        lock (_lock)
        {
            _buffer.Add(logEntry);

            if (_buffer.Count >= BufferSize)
            {
                Flush();
            }
        }
    }

    /// <summary>
    /// Flushes buffered log entries to the file.
    /// </summary>
    public void Flush()
    {
        lock (_lock)
        {
            if (_buffer.Count == 0)
                return;

            try
            {
                using var writer = File.AppendText(_filePath);
                foreach (var entry in _buffer)
                {
                    var json = JsonSerializer.Serialize(entry, new JsonSerializerOptions
                    {
                        WriteIndented = false
                    });
                    writer.WriteLine(json);
                }
            }
            catch
            {
                // Suppress file I/O errors to prevent logging from crashing the application
            }
            finally
            {
                _buffer.Clear();
            }
        }
    }
}
