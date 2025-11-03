using FluentAssertions;
using LoggingMonitoring.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LoggingMonitoring.Core.Tests;

/// <summary>
/// Unit tests for the PerformanceTracker class.
/// </summary>
public class PerformanceTrackerTests
{
    #region TrackOperation (with return value) Tests

    [Fact]
    public void TrackOperation_WithSuccessfulOperation_ShouldReturnResult()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        var expectedResult = "Test Result";

        // Act
        var result = PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => expectedResult
        );

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void TrackOperation_WithSuccessfulOperation_ShouldLogInformation()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => "Success"
        );

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("completed successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void TrackOperation_WithSuccessfulOperation_ShouldAddSuccessMetric()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => "Success"
        );

        // Assert
        mockMetricsCollector.Verify(
            x => x.AddMetric(It.Is<Models.PerformanceMetric>(m =>
                m.Component == "TestComponent" &&
                m.Operation == "TestOperation" &&
                m.Success == true &&
                m.ErrorCount == 0)),
            Times.Once);
    }

    [Fact]
    public void TrackOperation_WithFailedOperation_ShouldThrowException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var action = () => PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => throw expectedException
        );

        action.Should().Throw<InvalidOperationException>().WithMessage("Test exception");
    }

    [Fact]
    public void TrackOperation_WithFailedOperation_ShouldLogError()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        try
        {
            PerformanceTracker.TrackOperation(
                mockLogger.Object,
                "TestComponent",
                "TestOperation",
                mockMetricsCollector.Object,
                mockErrorTracker.Object,
                () => throw new InvalidOperationException("Test error")
            );
        }
        catch
        {
            // Expected exception
        }

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("failed")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void TrackOperation_WithFailedOperation_ShouldAddFailureMetric()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        try
        {
            PerformanceTracker.TrackOperation(
                mockLogger.Object,
                "TestComponent",
                "TestOperation",
                mockMetricsCollector.Object,
                mockErrorTracker.Object,
                () => throw new InvalidOperationException("Test error")
            );
        }
        catch
        {
            // Expected exception
        }

        // Assert
        mockMetricsCollector.Verify(
            x => x.AddMetric(It.Is<Models.PerformanceMetric>(m =>
                m.Component == "TestComponent" &&
                m.Operation == "TestOperation" &&
                m.Success == false &&
                m.ErrorCount == 1)),
            Times.Once);
    }

    [Fact]
    public void TrackOperation_WithFailedOperation_ShouldRecordError()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        try
        {
            PerformanceTracker.TrackOperation(
                mockLogger.Object,
                "TestComponent",
                "TestOperation",
                mockMetricsCollector.Object,
                mockErrorTracker.Object,
                () => throw new InvalidOperationException("Test error")
            );
        }
        catch
        {
            // Expected exception
        }

        // Assert
        mockErrorTracker.Verify(
            x => x.RecordError(
                "InvalidOperationException",
                "Test error",
                "TestComponent"),
            Times.Once);
    }

    [Fact]
    public void TrackOperation_ShouldMeasureExecutionTime()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act
        PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () =>
            {
                System.Threading.Thread.Sleep(50); // Simulate work
                return "Success";
            }
        );

        // Assert
        mockMetricsCollector.Verify(
            x => x.AddMetric(It.Is<Models.PerformanceMetric>(m =>
                m.DurationMs >= 50)), // Should be at least 50ms
            Times.Once);
    }

    #endregion

    #region TrackOperation (void) Tests

    [Fact]
    public void TrackOperation_Void_WithSuccessfulOperation_ShouldExecuteAction()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();
        var actionExecuted = false;

        // Act
        PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => { actionExecuted = true; }
        );

        // Assert
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void TrackOperation_Void_WithFailedOperation_ShouldThrowException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var mockMetricsCollector = new Mock<IMetricsCollector>();
        var mockErrorTracker = new Mock<IErrorTracker>();

        // Act & Assert
        var action = () => PerformanceTracker.TrackOperation(
            mockLogger.Object,
            "TestComponent",
            "TestOperation",
            mockMetricsCollector.Object,
            mockErrorTracker.Object,
            () => throw new InvalidOperationException("Test error")
        );

        action.Should().Throw<InvalidOperationException>();
    }

    #endregion
}


