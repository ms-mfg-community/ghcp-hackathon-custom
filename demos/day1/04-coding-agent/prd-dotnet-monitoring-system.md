# Product Requirements Document (PRD): .NET 9.0 Logging and Monitoring System

## Document Information

- **Version:** 1.0
- **Author(s):** GitHub Copilot
- **Date:** November 3, 2025
- **Status:** Draft

## Executive Summary

This document defines the requirements for a comprehensive .NET 9.0 logging and monitoring system that demonstrates multi-channel output, error tracking, and performance metrics collection. The solution is a direct port of the Python implementation (`agent-demo-1.py`) showcasing how GitHub Copilot agents decompose and solve complex problems using modern .NET patterns, Microsoft.Extensions.Logging framework, and best practices for enterprise logging.

## Problem Statement

Teams need a robust, extensible, and performant logging and monitoring solution built on .NET 9.0 that supports multiple output channels, tracks errors with aggregation, monitors performance metrics, and provides comprehensive reporting capabilities. The system must demonstrate modern .NET patterns including dependency injection, custom logger providers, and clean architecture while maintaining simplicity for educational demonstration purposes.

## Goals and Objectives

- Port the Python logging and monitoring system to .NET 9.0 using native Microsoft.Extensions.Logging framework
- Implement custom logger providers for console (colored), file (JSON), and metrics channels
- Provide error tracking with bounded history and type-based aggregation
- Enable performance monitoring with operation tracking, timing, and success/failure metrics
- Demonstrate modern .NET 9.0 features including record types, primary constructors, and file-scoped namespaces
- Include comprehensive unit tests following xUnit patterns from workspace conventions
- Maintain code-based configuration for demo simplicity and educational clarity

## Scope

### In Scope

- .NET 9.0 console application project (`LoggingMonitoringDemo`)
- Separate xUnit test project (`LoggingMonitoringDemo.Tests`)
- Custom logger providers implementing `ILoggerProvider`:
  - `ColoredConsoleLoggerProvider` (ANSI color-coded output)
  - `JsonFileLoggerProvider` (lightweight buffered JSON file logging)
  - `MetricsLoggerProvider` (performance metrics aggregation)
- Core models using modern .NET 9.0 patterns:
  - `LogEntryModel` (record type)
  - `PerformanceMetric` (record type)
  - `ErrorEntry` (record type)
- Service implementations:
  - `ErrorTracker` (singleton service with bounded queue)
  - `MetricsCollector` (singleton service with component-based aggregation)
  - `PerformanceTracker` (static utility class with extension methods)
- Workload simulation matching Python demo functionality
- Comprehensive reporting (JSON-formatted metrics and error summaries)
- Unit tests covering all major components

### Out of Scope

- External logging frameworks (Serilog, NLog, log4net)
- Configuration file support (appsettings.json)
- Distributed tracing or APM integration
- Database persistence for logs or metrics
- Web API or hosted service implementations
- Advanced security or authentication
- Production deployment configurations
- Docker containerization

## User Stories / Use Cases

- As a developer, I want to see color-coded console output matching log severity levels for quick visual identification of errors and warnings.
- As a DevOps engineer, I want JSON-formatted file logs that can be easily parsed and ingested by log aggregation tools.
- As a team lead, I want to track performance metrics by component and operation to identify bottlenecks and optimization opportunities.
- As a quality engineer, I want to view error summaries with type aggregation and recent error history for troubleshooting.
- As a .NET developer, I want to use extension methods for clean operation tracking syntax (`logger.TrackOperation(...)`).
- As a student, I want clear, well-documented code that demonstrates modern .NET 9.0 patterns and best practices.
- As a tester, I want comprehensive unit tests that demonstrate proper mocking and assertion patterns using xUnit and Moq.

## Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | The system shall use .NET 9.0 as the target framework for all projects. |
| FR-2 | The system shall implement custom logger providers using `ILoggerProvider` interface. |
| FR-3 | The `ColoredConsoleLoggerProvider` shall output logs with ANSI color codes (Cyan=Debug, Green=Info, Yellow=Warning, Red=Error, Bold Red=Critical). |
| FR-4 | The `JsonFileLoggerProvider` shall buffer up to 10 log entries before flushing to disk. |
| FR-5 | The `JsonFileLoggerProvider` shall write logs as newline-delimited JSON (NDJSON) format. |
| FR-6 | The `MetricsLoggerProvider` shall aggregate performance metrics by component. |
| FR-7 | The `ErrorTracker` shall maintain a bounded queue of maximum 100 error entries using FIFO eviction. |
| FR-8 | The `ErrorTracker` shall count errors by type and provide summary statistics. |
| FR-9 | The `MetricsCollector` shall calculate average duration, success rate, and total errors per component. |
| FR-10 | The `PerformanceTracker` shall use `System.Diagnostics.Stopwatch` for accurate timing measurements. |
| FR-11 | The `PerformanceTracker` shall automatically log success or failure with duration metadata. |
| FR-12 | The `PerformanceTracker` shall record metrics and errors regardless of operation success/failure. |
| FR-13 | The system shall provide `ILogger` extension methods for clean API usage. |
| FR-14 | The system shall support dependency injection for all services (`ErrorTracker`, `MetricsCollector`). |
| FR-15 | The system shall register all services and providers in `Program.cs` without configuration files. |
| FR-16 | The workload simulation shall include successful operations, validation with exceptions, and error scenarios. |
| FR-17 | The system shall generate a comprehensive JSON report including metrics summary and error summary. |
| FR-18 | The system shall flush all buffered logs before report generation. |

## Non-Functional Requirements

- **Performance:** Log buffering shall minimize I/O operations; target <1ms overhead for performance tracking.
- **Reliability:** Error tracking shall never throw exceptions; bounded queue shall prevent memory leaks.
- **Maintainability:** Code shall use modern .NET 9.0 features (record types, file-scoped namespaces, primary constructors).
- **Testability:** All components shall support dependency injection and interface-based testing.
- **Portability:** ANSI color codes shall work on Windows Terminal, PowerShell 7+, and Unix terminals.
- **Usability:** Extension methods shall provide intuitive API for operation tracking.
- **Documentation:** XML documentation comments shall be provided for all public APIs.
- **Code Quality:** All code shall follow workspace coding conventions from `.github/copilot-instructions.md`.

## Assumptions and Dependencies

- .NET 9.0 SDK is installed and accessible
- PowerShell 7+ or Windows Terminal is used for ANSI color support
- NuGet packages available:
  - `Microsoft.Extensions.Logging` (included in .NET 9.0)
  - `Microsoft.Extensions.DependencyInjection`
  - `Microsoft.Extensions.Hosting`
  - `xUnit` (for testing)
  - `Moq` (for mocking)
  - `FluentAssertions` (for test assertions)
- Users have basic understanding of dependency injection and logging patterns
- File system write permissions are available for log file output

## Technical Architecture

### Project Structure

```
demos/day1/04-coding-agent/workspace/
├── LoggingMonitoringDemo/
│   ├── LoggingMonitoringDemo.csproj
│   ├── Models/
│   │   ├── LogEntryModel.cs
│   │   ├── PerformanceMetric.cs
│   │   └── ErrorEntry.cs
│   ├── Logging/
│   │   ├── Providers/
│   │   │   ├── ColoredConsoleLoggerProvider.cs
│   │   │   ├── ColoredConsoleLogger.cs
│   │   │   ├── JsonFileLoggerProvider.cs
│   │   │   ├── JsonFileLogger.cs
│   │   │   ├── MetricsLoggerProvider.cs
│   │   │   └── MetricsLogger.cs
│   │   ├── ErrorTracker.cs
│   │   ├── IErrorTracker.cs
│   │   ├── MetricsCollector.cs
│   │   ├── IMetricsCollector.cs
│   │   ├── PerformanceTracker.cs
│   │   └── LoggingExtensions.cs
│   └── Program.cs
└── LoggingMonitoringDemo.Tests/
    ├── LoggingMonitoringDemo.Tests.csproj
    ├── ErrorTrackerTests.cs
    ├── MetricsCollectorTests.cs
    ├── PerformanceTrackerTests.cs
    └── JsonFileLoggerProviderTests.cs
```

### Core Components

#### 1. Models (Record Types)

- **LogEntryModel:** Timestamp, LogLevel, Component, Message, Duration, ErrorDetails, Metadata
- **PerformanceMetric:** Component, Operation, DurationMs, MemoryUsedMb, Success, ErrorCount
- **ErrorEntry:** Timestamp, Type, Message, Component

#### 2. Custom Logger Providers

- **ColoredConsoleLoggerProvider:** Factory for colored console loggers
- **JsonFileLoggerProvider:** Factory for JSON file loggers with buffering
- **MetricsLoggerProvider:** Factory for metrics collection loggers

#### 3. Services

- **ErrorTracker (IErrorTracker):** Singleton service managing bounded error queue and type counts
- **MetricsCollector (IMetricsCollector):** Singleton service aggregating metrics by component
- **PerformanceTracker:** Static utility with `TrackOperation<T>` method

#### 4. Extension Methods

- **LoggingExtensions:** Provides `ILogger.TrackOperation()` extension methods

### Data Flow

1. Application code calls `logger.TrackOperation(component, operation, func)`
2. `PerformanceTracker` starts `Stopwatch` and executes the function
3. On success/failure, creates `PerformanceMetric` record
4. Logs success/error message with duration via `ILogger`
5. All registered providers receive log message:
   - Console provider writes colored output
   - File provider buffers and writes JSON
   - Metrics provider stores metric in collector
6. If log level is Error/Critical, `ErrorTracker` records error entry
7. Report generation calls `GetSummary()` on collectors for JSON output

## Success Criteria / KPIs

- All functional requirements (FR-1 through FR-18) implemented and verified
- Unit test coverage ≥80% for all components
- All tests pass in xUnit test runner
- Console output displays correct ANSI colors in Windows Terminal
- JSON file logs are valid NDJSON and parsable
- Performance tracking overhead measured at <1ms per operation
- Workload simulation produces expected output matching Python demo behavior
- Error summary accurately reflects recorded errors
- Metrics summary calculates correct averages and success rates
- Code follows all workspace conventions from `.github/copilot-instructions.md`

## Milestones & Timeline

| Milestone | Deliverables | Status |
|---|---|---|
| M1: Project Setup | Create .csproj files, install NuGet packages, configure project references | Pending |
| M2: Core Models | Implement record types (LogEntryModel, PerformanceMetric, ErrorEntry) | Pending |
| M3: Services | Implement ErrorTracker and MetricsCollector with interfaces | Pending |
| M4: Logger Providers | Implement all three custom logger providers and logger classes | Pending |
| M5: Performance Tracking | Implement PerformanceTracker and extension methods | Pending |
| M6: Workload Simulation | Implement Program.cs with demonstration and reporting | Pending |
| M7: Unit Tests | Create all test classes with comprehensive test coverage | Pending |
| M8: Documentation | Add XML comments, README, and usage examples | Pending |

## Usage Instructions (Demonstration Sequence)

### Prerequisites

1. Install .NET 9.0 SDK
2. Navigate to `demos/day1/04-coding-agent/workspace/LoggingMonitoringDemo/`
3. Restore NuGet packages: `dotnet restore`

### Build and Run

```powershell
# Build the solution
dotnet build

# Run the demo
dotnet run

# Run tests
cd ..\LoggingMonitoringDemo.Tests
dotnet test
```

### Expected Output

1. **Console Output:** Color-coded logs showing system initialization, workload simulation, and completion
2. **File Output:** `demo.log` file containing NDJSON formatted log entries
3. **Report Output:** JSON-formatted report showing:
   - Metrics summary (operations per component, average duration, success rate)
   - Error summary (total errors, error types, recent errors)

### Demonstration Flow

1. System initializes with all three logger providers registered
2. Services (ErrorTracker, MetricsCollector) registered as singletons
3. Workload simulation runs:
   - ProcessData operations with 1000 and 5000 item datasets
   - ValidateInput operations (successful and failing scenarios)
4. Performance tracked for all operations with automatic timing
5. Errors recorded for validation failures
6. All channels flushed
7. Comprehensive report generated and displayed

## Key Takeaways

- Demonstrates modern .NET 9.0 patterns including record types, primary constructors, and file-scoped namespaces
- Shows proper implementation of `ILoggerProvider` for custom logging channels
- Illustrates dependency injection and service registration patterns
- Provides clean API design with extension methods for developer experience
- Follows workspace conventions and best practices
- Includes comprehensive testing strategy with xUnit, Moq, and FluentAssertions
- Maintains code simplicity for educational purposes while demonstrating enterprise patterns

## Implementation Notes

### .NET 9.0 Features Leveraged

- **Record Types:** Immutable data models for log entries and metrics
- **Primary Constructors:** Simplified dependency injection in classes
- **File-Scoped Namespaces:** Reduced indentation and cleaner code
- **Collection Expressions:** Modern collection initialization syntax
- **Required Members:** Compile-time safety for critical properties

### Design Patterns

- **Strategy Pattern:** Multiple logger providers implementing common interface
- **Singleton Pattern:** ErrorTracker and MetricsCollector as application-wide services
- **Extension Method Pattern:** Clean, fluent API for operation tracking
- **Dependency Injection Pattern:** Constructor injection throughout

### Testing Strategy

- **Arrange-Act-Assert (AAA):** Standard test structure
- **Mocking:** Use Moq for `ILogger`, `IServiceProvider` dependencies
- **Fluent Assertions:** Readable, expressive test assertions
- **Test Organization:** Group tests by component with descriptive test names
- **Coverage:** Target all public methods and critical error paths

## Questions or Feedback from Attendees

- Should the system support configuration-based log level filtering?
- Is there interest in adding distributed tracing integration (System.Diagnostics.Activity)?
- Should we provide examples of custom metadata usage in log entries?
- Would async file I/O be beneficial for the JsonFileLoggerProvider?

## Questions for Attendees

- Are there additional performance metrics that would be valuable to track?
- Should the error tracking support error categorization or severity levels?
- Is there interest in adding log rotation or file size limits to the file provider?
- Would structured logging with semantic properties be a valuable addition?

## Call to Action

- Review the PRD and provide feedback on requirements and architecture
- Suggest additional features or modifications for educational value
- Identify any missing test scenarios or edge cases
- Propose improvements to API design or developer experience
