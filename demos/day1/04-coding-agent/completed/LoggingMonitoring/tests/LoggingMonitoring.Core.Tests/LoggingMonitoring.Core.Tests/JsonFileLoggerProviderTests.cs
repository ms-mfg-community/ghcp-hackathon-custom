using FluentAssertions;
using LoggingMonitoring.Core.Logging.Providers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LoggingMonitoring.Core.Tests;

/// <summary>
/// Unit tests for the JsonFileLoggerProvider and JsonFileLogger classes.
/// </summary>
public class JsonFileLoggerProviderTests
{
    private readonly string _testLogFile;

    public JsonFileLoggerProviderTests()
    {
        _testLogFile = Path.Combine(Path.GetTempPath(), $"test-log-{Guid.NewGuid()}.json");
    }

    #region JsonFileLoggerProvider Tests

    [Fact]
    public void CreateLogger_ShouldReturnLogger()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);

        // Act
        var logger = provider.CreateLogger("TestCategory");

        // Assert
        logger.Should().NotBeNull();
        logger.Should().BeOfType<JsonFileLogger>();
    }

    [Fact]
    public void Dispose_ShouldFlushBufferedLogs()
    {
        // Arrange
        var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act - Write some logs
        logger.LogInformation("Test message");
        provider.Dispose();

        // Assert - File should exist and contain the log
        File.Exists(_testLogFile).Should().BeTrue();
        var content = File.ReadAllText(_testLogFile);
        content.Should().Contain("Test message");

        // Cleanup
        File.Delete(_testLogFile);
    }

    #endregion

    #region JsonFileLogger Tests

    [Fact]
    public void Log_ShouldBufferEntriesBeforeWriting()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act - Write less than buffer size (10)
        for (int i = 0; i < 5; i++)
        {
            logger.LogInformation($"Message {i}");
        }

        // Assert - File should not exist yet (buffer not flushed)
        File.Exists(_testLogFile).Should().BeFalse();

        // Cleanup
        provider.Dispose();
        if (File.Exists(_testLogFile))
            File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_ShouldFlushWhenBufferFull()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act - Write exactly buffer size (10)
        for (int i = 0; i < 10; i++)
        {
            logger.LogInformation($"Message {i}");
        }

        // Assert - File should exist (buffer flushed)
        File.Exists(_testLogFile).Should().BeTrue();
        var lines = File.ReadAllLines(_testLogFile);
        lines.Should().HaveCount(10);

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_ShouldWriteValidJson()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act
        logger.LogInformation("Test message");
        logger.LogError(new Exception("Test error"), "Error message");
        provider.Flush();

        // Assert
        File.Exists(_testLogFile).Should().BeTrue();
        var lines = File.ReadAllLines(_testLogFile);
        lines.Should().HaveCountGreaterThanOrEqualTo(2);

        // Verify JSON is valid by parsing
        foreach (var line in lines)
        {
            var action = () => System.Text.Json.JsonDocument.Parse(line);
            action.Should().NotThrow();
        }

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_ShouldIncludeLogLevel()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act
        logger.LogWarning("Warning message");
        provider.Flush();

        // Assert
        var content = File.ReadAllText(_testLogFile);
        content.Should().Contain("Warning");

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_ShouldIncludeTimestamp()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act
        logger.LogInformation("Test message");
        provider.Flush();

        // Assert
        var content = File.ReadAllText(_testLogFile);
        content.Should().Contain("timestamp");

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_ShouldIncludeComponent()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("MyApplication.Services.UserService");

        // Act
        logger.LogInformation("Test message");
        provider.Flush();

        // Assert
        var content = File.ReadAllText(_testLogFile);
        content.Should().Contain("component");
        content.Should().Contain("UserService");

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Log_WithException_ShouldIncludeErrorDetails()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act
        logger.LogError(new InvalidOperationException("Test error"), "Operation failed");
        provider.Flush();

        // Assert
        var content = File.ReadAllText(_testLogFile);
        content.Should().Contain("error_details");
        content.Should().Contain("Test error");

        // Cleanup
        File.Delete(_testLogFile);
    }

    [Fact]
    public void Flush_WithEmptyBuffer_ShouldNotCreateFile()
    {
        // Arrange
        using var provider = new JsonFileLoggerProvider(_testLogFile);
        var logger = provider.CreateLogger("TestCategory");

        // Act
        provider.Flush();

        // Assert
        File.Exists(_testLogFile).Should().BeFalse();
    }

    #endregion
}


