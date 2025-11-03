# LoggingMonitoring - .NET 9.0 Logging and Monitoring System

## Overview

This is a comprehensive logging and monitoring system for .NET 9.0 applications, demonstrating best practices for:

- **Multi-channel logging** (Console with ANSI colors, JSON file output)
- **Performance metrics collection** and aggregation
- **Error tracking** with bounded history
- **Dependency injection** patterns
- **Clean architecture** with separate Core library for code reuse

## Solution Structure

```
LoggingMonitoring/
├── LoggingMonitoring.sln               # Solution file
├── src/
│   ├── LoggingMonitoring.Core/         # Shared library with reusable components
│   │   ├── Models/                     # Data models (record types)
│   │   │   ├── LogEntryModel.cs
│   │   │   ├── PerformanceMetric.cs
│   │   │   └── ErrorEntry.cs
│   │   ├── Logging/                    # Core logging functionality
│   │   │   ├── IErrorTracker.cs
│   │   │   ├── ErrorTracker.cs
│   │   │   ├── IMetricsCollector.cs
│   │   │   ├── MetricsCollector.cs
│   │   │   ├── PerformanceTracker.cs
│   │   │   ├── LoggingExtensions.cs
│   │   │   └── Providers/              # Custom logger providers
│   │   │       ├── ColoredConsoleLoggerProvider.cs
│   │   │       ├── ColoredConsoleLogger.cs
│   │   │       ├── JsonFileLoggerProvider.cs
│   │   │       ├── JsonFileLogger.cs
│   │   │       ├── MetricsLoggerProvider.cs
│   │   │       └── MetricsLogger.cs
│   └── LoggingMonitoring.Console/      # Console application demonstrating the system
│       └── Program.cs
└── tests/
    └── LoggingMonitoring.Core.Tests/   # xUnit test project
        ├── ErrorTrackerTests.cs        # 11 tests for error tracking
        ├── MetricsCollectorTests.cs    # 12 tests for metrics
        ├── PerformanceTrackerTests.cs  # 12 tests for performance tracking
        └── JsonFileLoggerProviderTests.cs  # 2 tests for file logging
```

## Key Features

### 1. Custom Logger Providers

- **ColoredConsoleLogger**: ANSI color-coded console output based on log level
  - Debug: Cyan
  - Information: Green
  - Warning: Yellow
  - Error: Red
  - Critical: Bold Red

- **JsonFileLogger**: Buffered JSON file logging with NDJSON format
  - Configurable buffer size (default: 10 entries)
  - Automatic flush on buffer full or disposal

- **MetricsLogger**: Integrates with metrics collection system

### 2. Error Tracking

- Bounded queue with FIFO eviction (max 100 entries by default)
- Thread-safe operations with concurrent dictionary for error counts
- Aggregation by error type
- Recent errors tracking (last 5)

### 3. Performance Metrics

- Component-based aggregation
- Calculates:
  - Total operations
  - Average duration
  - Success rate
  - Total errors per component
- Thread-safe concurrent operations

### 4. Performance Tracker

- Static utility for operation tracking
- Automatic Stopwatch timing
- Success/failure logging
- Metric recording
- Exception re-throwing to maintain exception flow

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 / VS Code / Rider (optional)

### Building the Solution

```bash
cd LoggingMonitoring
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

Expected output: **37 tests passed** (100% success rate)

### Running the Console Application

```bash
cd src/LoggingMonitoring.Console/LoggingMonitoring.Console
dotnet run
```

## Usage Example

```csharp
using LoggingMonitoring.Core.Logging;
using LoggingMonitoring.Core.Logging.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Setup dependency injection
var services = new ServiceCollection();
services.AddSingleton<IErrorTracker, ErrorTracker>();
services.AddSingleton<IMetricsCollector, MetricsCollector>();
var serviceProvider = services.BuildServiceProvider();

// Get services
var errorTracker = serviceProvider.GetRequiredService<IErrorTracker>();
var metricsCollector = serviceProvider.GetRequiredService<IMetricsCollector>();

// Create logger factory
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.ClearProviders();
    builder.AddProvider(new ColoredConsoleLoggerProvider());
    builder.AddProvider(new JsonFileLoggerProvider("app.log"));
    builder.AddProvider(new MetricsLoggerProvider(metricsCollector));
    builder.SetMinimumLevel(LogLevel.Debug);
});

var logger = loggerFactory.CreateLogger<Program>();

// Track an operation with automatic metrics and error handling
logger.TrackOperation(
    "MyComponent",
    "my_operation",
    metricsCollector,
    errorTracker,
    () => {
        // Your operation code here
        return "Success!";
    });

// Get aggregated metrics
var metricsSummary = metricsCollector.GetSummary();

// Get error summary
var errorSummary = errorTracker.GetErrorSummary();
```

## Architecture Benefits

### Separation of Concerns

- **LoggingMonitoring.Core**: Reusable library that can be referenced by:
  - Console applications
  - Web applications (Blazor Server/WebAPI)
  - Azure Functions
  - Any other .NET 9.0 project

- **LoggingMonitoring.Console**: Standalone console app demonstrating usage

- **LoggingMonitoring.Core.Tests**: Comprehensive test suite ensuring reliability

### Future Extensibility

This structure allows easy addition of:
- Web UI projects (Blazor Server, ASP.NET Core MVC)
- Additional console utilities
- Azure Function apps
- All sharing the same core logging infrastructure

## Testing

The solution includes 37 comprehensive unit tests:

- **ErrorTrackerTests** (11 tests):
  - Basic error recording
  - Multiple error types
  - Bounded queue behavior
  - Error summary generation
  - Thread safety

- **MetricsCollectorTests** (12 tests):
  - Metric addition and aggregation
  - Success rate calculations
  - Component-based grouping
  - Thread safety

- **PerformanceTrackerTests** (12 tests):
  - Successful operation tracking
  - Failed operation tracking
  - Duration measurement
  - Void operation support

- **JsonFileLoggerProviderTests** (2 tests):
  - File logging functionality
  - Buffer flushing behavior

## Dependencies

### Core Library
- `Microsoft.Extensions.Logging` (9.0.10)
- `Microsoft.Extensions.DependencyInjection` (9.0.10)

### Console Application
- References `LoggingMonitoring.Core`
- `Microsoft.Extensions.Logging` (9.0.10)
- `Microsoft.Extensions.DependencyInjection` (9.0.10)
- `Microsoft.Extensions.Hosting` (9.0.10)

### Test Project
- References `LoggingMonitoring.Core`
- `xunit` (2.9.2)
- `xunit.runner.visualstudio` (2.8.2)
- `Moq` (4.20.72)
- `FluentAssertions` (8.8.0)

## Modern .NET 9.0 Features Used

- **Record types** for immutable data models
- **File-scoped namespaces** for cleaner code
- **Collection expressions** (`[metric]` instead of `new List { metric }`)
- **Nullable reference types** enabled throughout
- **ImplicitUsings** for reduced boilerplate

## Build Information

- **Framework**: .NET 9.0
- **Language Version**: C# 13 (latest)
- **Nullable**: Enabled
- **ImplicitUsings**: Enabled

## License

This project is part of the GitHub Copilot Hackathon demonstration materials.

## Related Documentation

- [Product Requirements Document (PRD)](../prd-dotnet-monitoring-system.md)
- [Implementation Summary](../IMPLEMENTATION_SUMMARY.md)

## Demo Context

This system was created as part of **Day 1.4: GitHub Copilot Coding Agent** demonstration, showcasing how GitHub Copilot can:

1. Decompose complex requirements into manageable tasks
2. Generate production-quality code with proper architecture
3. Include comprehensive testing
4. Follow modern .NET best practices
5. Create reusable, extensible solutions

The refactored solution structure demonstrates clean architecture principles and prepares the codebase for future extensions such as web UIs and cloud deployments.
