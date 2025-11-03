using FluentAssertions;
using LoggingMonitoring.Core.Logging;
using Xunit;

namespace LoggingMonitoring.Core.Tests;

/// <summary>
/// Unit tests for the ErrorTracker class.
/// </summary>
public class ErrorTrackerTests
{
    #region RecordError Tests

    [Fact]
    public void RecordError_ShouldAddErrorToHistory()
    {
        // Arrange
        var tracker = new ErrorTracker();

        // Act
        tracker.RecordError("TestException", "Test error message", "TestComponent");

        // Assert
        var summary = tracker.GetErrorSummary();
        summary["total_errors"].Should().Be(1);
    }

    [Fact]
    public void RecordError_ShouldIncrementErrorCountByType()
    {
        // Arrange
        var tracker = new ErrorTracker();

        // Act
        tracker.RecordError("ArgumentException", "Invalid argument", "Component1");
        tracker.RecordError("ArgumentException", "Another invalid argument", "Component2");
        tracker.RecordError("NullReferenceException", "Null reference", "Component3");

        // Assert
        var summary = tracker.GetErrorSummary();
        var errorTypes = summary["error_types"] as Dictionary<string, int>;
        errorTypes.Should().NotBeNull();
        errorTypes!["ArgumentException"].Should().Be(2);
        errorTypes["NullReferenceException"].Should().Be(1);
    }

    [Fact]
    public void RecordError_ShouldMaintainBoundedQueue()
    {
        // Arrange
        var tracker = new ErrorTracker(maxHistory: 5);

        // Act - Add 10 errors, only last 5 should be retained
        for (int i = 0; i < 10; i++)
        {
            tracker.RecordError("TestException", $"Error {i}", "TestComponent");
        }

        // Assert
        var summary = tracker.GetErrorSummary();
        var recentErrors = summary["recent_errors"] as List<LoggingMonitoring.Core.Models.ErrorEntry>;
        recentErrors.Should().NotBeNull();
        recentErrors!.Count.Should().BeLessThanOrEqualTo(5);
    }

    #endregion

    #region GetErrorSummary Tests

    [Fact]
    public void GetErrorSummary_ShouldReturnCorrectTotalErrors()
    {
        // Arrange
        var tracker = new ErrorTracker();
        tracker.RecordError("Exception1", "Error 1", "Component1");
        tracker.RecordError("Exception2", "Error 2", "Component2");
        tracker.RecordError("Exception1", "Error 3", "Component3");

        // Act
        var summary = tracker.GetErrorSummary();

        // Assert
        summary["total_errors"].Should().Be(3);
    }

    [Fact]
    public void GetErrorSummary_ShouldReturnRecentErrorsInCorrectOrder()
    {
        // Arrange
        var tracker = new ErrorTracker();
        tracker.RecordError("Exception1", "First error", "Component1");
        System.Threading.Thread.Sleep(10); // Ensure different timestamps
        tracker.RecordError("Exception2", "Second error", "Component2");

        // Act
        var summary = tracker.GetErrorSummary();
        var recentErrors = summary["recent_errors"] as List<LoggingMonitoring.Core.Models.ErrorEntry>;

        // Assert
        recentErrors.Should().NotBeNull();
        recentErrors!.Count.Should().Be(2);
        recentErrors[0].Message.Should().Be("First error");
        recentErrors[1].Message.Should().Be("Second error");
    }

    [Fact]
    public void GetErrorSummary_ShouldReturnMaximum5RecentErrors()
    {
        // Arrange
        var tracker = new ErrorTracker();
        for (int i = 0; i < 10; i++)
        {
            tracker.RecordError("TestException", $"Error {i}", "TestComponent");
        }

        // Act
        var summary = tracker.GetErrorSummary();
        var recentErrors = summary["recent_errors"] as List<LoggingMonitoring.Core.Models.ErrorEntry>;

        // Assert
        recentErrors.Should().NotBeNull();
        recentErrors!.Count.Should().Be(5);
    }

    [Fact]
    public void GetErrorSummary_WithNoErrors_ShouldReturnZeroTotal()
    {
        // Arrange
        var tracker = new ErrorTracker();

        // Act
        var summary = tracker.GetErrorSummary();

        // Assert
        summary["total_errors"].Should().Be(0);
        var recentErrors = summary["recent_errors"] as List<LoggingMonitoring.Core.Models.ErrorEntry>;
        recentErrors.Should().NotBeNull();
        recentErrors!.Should().BeEmpty();
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public void RecordError_ShouldBeThreadSafe()
    {
        // Arrange
        var tracker = new ErrorTracker();
        var tasks = new List<Task>();

        // Act - Simulate concurrent error recording
        for (int i = 0; i < 10; i++)
        {
            int errorNum = i;
            tasks.Add(Task.Run(() =>
            {
                tracker.RecordError("TestException", $"Error {errorNum}", "TestComponent");
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        var summary = tracker.GetErrorSummary();
        summary["total_errors"].Should().Be(10);
    }

    #endregion
}


