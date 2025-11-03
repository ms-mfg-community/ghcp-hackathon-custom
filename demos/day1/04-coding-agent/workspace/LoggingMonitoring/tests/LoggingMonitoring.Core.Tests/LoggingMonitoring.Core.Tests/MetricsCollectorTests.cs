using FluentAssertions;
using LoggingMonitoring.Core.Logging;
using LoggingMonitoring.Core.Models;
using Xunit;

namespace LoggingMonitoring.Core.Tests;

/// <summary>
/// Unit tests for the MetricsCollector class.
/// </summary>
public class MetricsCollectorTests
{
    #region AddMetric Tests

    [Fact]
    public void AddMetric_ShouldStoreMetric()
    {
        // Arrange
        var collector = new MetricsCollector();
        var metric = new PerformanceMetric(
            Component: "TestComponent",
            Operation: "TestOperation",
            DurationMs: 100.5,
            MemoryUsedMb: 1.2,
            Success: true
        );

        // Act
        collector.AddMetric(metric);

        // Assert
        var summary = collector.GetSummary();
        summary.Should().ContainKey("TestComponent");
    }

    [Fact]
    public void AddMetric_ShouldGroupMetricsByComponent()
    {
        // Arrange
        var collector = new MetricsCollector();
        var metric1 = new PerformanceMetric("Component1", "Op1", 100, 1.0, true);
        var metric2 = new PerformanceMetric("Component2", "Op2", 200, 2.0, true);
        var metric3 = new PerformanceMetric("Component1", "Op3", 150, 1.5, true);

        // Act
        collector.AddMetric(metric1);
        collector.AddMetric(metric2);
        collector.AddMetric(metric3);

        // Assert
        var summary = collector.GetSummary();
        summary.Should().ContainKey("Component1");
        summary.Should().ContainKey("Component2");

        var component1Summary = summary["Component1"] as Dictionary<string, object>;
        component1Summary.Should().NotBeNull();
        component1Summary!["total_operations"].Should().Be(2);
    }

    #endregion

    #region GetSummary Tests

    [Fact]
    public void GetSummary_ShouldCalculateCorrectAverageDuration()
    {
        // Arrange
        var collector = new MetricsCollector();
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op1", 100, 1.0, true));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op2", 200, 1.0, true));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op3", 300, 1.0, true));

        // Act
        var summary = collector.GetSummary();

        // Assert
        var componentSummary = summary["TestComponent"] as Dictionary<string, object>;
        componentSummary.Should().NotBeNull();
        componentSummary!["avg_duration_ms"].Should().Be(200.0);
    }

    [Fact]
    public void GetSummary_ShouldCalculateCorrectSuccessRate()
    {
        // Arrange
        var collector = new MetricsCollector();
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op1", 100, 1.0, true));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op2", 200, 1.0, false));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op3", 300, 1.0, true));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op4", 400, 1.0, true));

        // Act
        var summary = collector.GetSummary();

        // Assert
        var componentSummary = summary["TestComponent"] as Dictionary<string, object>;
        componentSummary.Should().NotBeNull();
        componentSummary!["success_rate"].Should().Be(75.0); // 3 out of 4 succeeded
    }

    [Fact]
    public void GetSummary_ShouldCalculateCorrectTotalErrors()
    {
        // Arrange
        var collector = new MetricsCollector();
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op1", 100, 1.0, true, ErrorCount: 0));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op2", 200, 1.0, false, ErrorCount: 1));
        collector.AddMetric(new PerformanceMetric("TestComponent", "Op3", 300, 1.0, false, ErrorCount: 2));

        // Act
        var summary = collector.GetSummary();

        // Assert
        var componentSummary = summary["TestComponent"] as Dictionary<string, object>;
        componentSummary.Should().NotBeNull();
        componentSummary!["total_errors"].Should().Be(3);
    }

    [Fact]
    public void GetSummary_ShouldCountTotalOperations()
    {
        // Arrange
        var collector = new MetricsCollector();
        for (int i = 0; i < 5; i++)
        {
            collector.AddMetric(new PerformanceMetric("TestComponent", $"Op{i}", 100, 1.0, true));
        }

        // Act
        var summary = collector.GetSummary();

        // Assert
        var componentSummary = summary["TestComponent"] as Dictionary<string, object>;
        componentSummary.Should().NotBeNull();
        componentSummary!["total_operations"].Should().Be(5);
    }

    [Fact]
    public void GetSummary_WithNoMetrics_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var collector = new MetricsCollector();

        // Act
        var summary = collector.GetSummary();

        // Assert
        summary.Should().BeEmpty();
    }

    [Fact]
    public void GetSummary_ShouldHandleMultipleComponents()
    {
        // Arrange
        var collector = new MetricsCollector();
        collector.AddMetric(new PerformanceMetric("Component1", "Op1", 100, 1.0, true));
        collector.AddMetric(new PerformanceMetric("Component2", "Op2", 200, 1.0, true));
        collector.AddMetric(new PerformanceMetric("Component3", "Op3", 300, 1.0, true));

        // Act
        var summary = collector.GetSummary();

        // Assert
        summary.Should().HaveCount(3);
        summary.Should().ContainKeys("Component1", "Component2", "Component3");
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public void AddMetric_ShouldBeThreadSafe()
    {
        // Arrange
        var collector = new MetricsCollector();
        var tasks = new List<Task>();

        // Act - Simulate concurrent metric addition
        for (int i = 0; i < 100; i++)
        {
            int metricNum = i;
            tasks.Add(Task.Run(() =>
            {
                collector.AddMetric(new PerformanceMetric(
                    "TestComponent",
                    $"Op{metricNum}",
                    100 + metricNum,
                    1.0,
                    true
                ));
            }));
        }

        Task.WaitAll(tasks.ToArray());

        // Assert
        var summary = collector.GetSummary();
        var componentSummary = summary["TestComponent"] as Dictionary<string, object>;
        componentSummary.Should().NotBeNull();
        componentSummary!["total_operations"].Should().Be(100);
    }

    #endregion
}


