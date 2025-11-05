# Blazor Server Calculator

A modern web-based calculator application built with Blazor Server and .NET 9, refactored from a console application while preserving all business logic and test coverage.

## Features

- **Six Arithmetic Operations:** Addition, Subtraction, Multiplication, Division, Modulo, and Exponentiation
- **Real-time Calculation History:** Tracks up to 50 recent calculations in-memory
- **Error Handling:** Graceful handling of division by zero and invalid inputs with ErrorBoundary
- **Responsive UI:** Clean Bootstrap 5-based interface
- **Component Testing:** Comprehensive bUnit tests for UI components
- **Dependency Injection:** Service-based architecture with scoped services
- **SignalR Integration:** Real-time communication between client and server
- **Circuit Monitoring:** Custom circuit handler for connection diagnostics
- **Structured Logging:** ILogger integration for diagnostics

## Project Structure

```
calculator-xunit-testing/
├── calculator/                    # Original console app (unchanged)
│   ├── Calculator.cs
│   └── calculator.csproj
├── calculator.tests/              # Original xUnit tests (50 tests)
│   ├── CalculatorTest.cs
│   └── calculator.tests.csproj
├── calculator.web/                # Blazor Server project
│   ├── Pages/
│   │   ├── Calculator.razor      # Main calculator component
│   │   ├── Calculator.razor.css  # Component-scoped CSS
│   │   ├── Index.razor
│   │   ├── Counter.razor
│   │   └── FetchData.razor
│   ├── Services/
│   │   ├── ICalculatorService.cs
│   │   ├── CalculatorService.cs
│   │   ├── ICalculationHistoryService.cs
│   │   ├── CalculationHistoryService.cs
│   │   ├── CalculationEntry.cs
│   │   └── CalculatorCircuitHandler.cs
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── App.razor                 # ErrorBoundary configuration
│   ├── Program.cs                # Service registration & SignalR config
│   └── calculator.web.csproj
├── calculator.web.tests/          # bUnit test project (11 tests)
│   ├── CalculatorComponentTests.cs
│   └── calculator.web.tests.csproj
└── calculator.sln
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Modern web browser (Chrome, Edge, Firefox)
- HTTPS development certificate (trusted)

### Trust the Development Certificate

If you haven't already, trust the ASP.NET Core HTTPS development certificate:

```powershell
dotnet dev-certs https --trust
```

### Running the Application

1. **Navigate to the web project directory:**

   ```powershell
   cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing\calculator.web
   ```

2. **Run the application:**

   ```powershell
   dotnet run
   ```

3. **Access the application:**

   Open your browser to:
   - **HTTPS:** https://localhost:7092
   - **HTTP:** http://localhost:5100
   
   (Port numbers may vary - check console output)

4. **Navigate to the calculator:**

   Click "Calculator" in the navigation menu or go directly to `/calculator`

### Running Tests

#### Run All Tests (61 total)

```powershell
cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing
dotnet test
```

#### Run Original xUnit Tests Only (50 tests)

```powershell
cd calculator.tests
dotnet test
```

#### Run bUnit Component Tests Only (11 tests)

```powershell
cd calculator.web.tests
dotnet test
```

#### Run Tests with Coverage

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Architecture

### Service Layer

The application uses dependency injection to wrap the static `CalculatorOperations` class:

- **ICalculatorService / CalculatorService:** Wraps calculator operations for DI
- **ICalculationHistoryService / CalculationHistoryService:** Manages in-memory calculation history
- **CalculationEntry:** Model for history entries
- **CalculatorCircuitHandler:** Monitors Blazor circuit lifecycle

### Service Lifetimes

All services are registered as **scoped** (circuit lifetime):
- Each user session gets isolated service instances
- History is maintained per-user
- Services are disposed when the circuit ends

### SignalR Configuration

Optimized for performance:
- **MaximumReceiveMessageSize:** 32 KB
- **ClientTimeoutInterval:** 30 seconds
- **KeepAliveInterval:** 15 seconds
- **DisconnectedCircuitRetentionPeriod:** 3 minutes

### Error Handling

- **ErrorBoundary:** Catches unhandled component exceptions
- **Try-Catch:** Handles division by zero and calculation errors
- **Structured Logging:** All errors logged with context
- **User-Friendly Messages:** No technical details exposed to users

## Usage

### Performing Calculations

1. Enter the first number
2. Enter the second number
3. Select an operation from the dropdown
4. Click "Calculate"
5. View the result or error message
6. See the calculation added to history

### Managing History

- **View History:** Displays up to 50 recent calculations
- **Clear History:** Click "Clear History" button to reset
- **Automatic Limit:** Oldest entries removed when exceeding 50

### Supported Operations

| Operation | Symbol | Example |
|-----------|--------|---------|
| Addition | + | 5 + 3 = 8 |
| Subtraction | - | 10 - 4 = 6 |
| Multiplication | * | 6 * 7 = 42 |
| Division | / | 15 / 3 = 5 |
| Modulo | % | 10 % 3 = 1 |
| Exponentiation | ^ | 2 ^ 3 = 8 |

## Development

### Building the Solution

```powershell
dotnet build
```

### Clean Build Artifacts

```powershell
dotnet clean
```

### Restore NuGet Packages

```powershell
dotnet restore
```

### Run in Watch Mode (Auto-reload)

```powershell
cd calculator.web
dotnet watch run
```

## Testing

### Test Coverage Goals

- **Business Logic:** 100% (maintained from original project)
- **UI Components:** 80%+ (bUnit tests)
- **Service Layer:** 100% (covered by integration tests)

### Test Categories

1. **Unit Tests (calculator.tests):**
   - All six arithmetic operations
   - Edge cases (division by zero, large numbers)
   - Theory-based tests with multiple scenarios

2. **Component Tests (calculator.web.tests):**
   - Component rendering
   - User interactions (button clicks, input changes)
   - Service integration with mocks
   - Error handling UI
   - History display and management

## Troubleshooting

### Port Already in Use

If the default ports are in use, edit `Properties/launchSettings.json` to change:

```json
{
  "applicationUrl": "https://localhost:XXXX;http://localhost:YYYY"
}
```

### Certificate Not Trusted

Run the following command and restart your browser:

```powershell
dotnet dev-certs https --trust
```

### Build Errors

1. Clean the solution: `dotnet clean`
2. Restore packages: `dotnet restore`
3. Rebuild: `dotnet build`

### Tests Failing

Ensure all projects build successfully first:

```powershell
dotnet build
```

Check for compilation errors before running tests.

## Technology Stack

- **.NET 9:** Target framework
- **Blazor Server:** UI framework
- **SignalR:** Real-time communication
- **Bootstrap 5:** CSS framework
- **xUnit:** Unit testing framework
- **bUnit 1.40.0:** Blazor component testing
- **Moq 4.20.72:** Mocking framework
- **C# 12:** Programming language

## Dependencies

### Production Dependencies
- Microsoft.AspNetCore.Components.Web (9.0.5)
- Microsoft.Extensions.Logging (9.0.5)
- Microsoft.AspNetCore.SignalR (via framework)

### Development Dependencies
- bunit (1.40.0)
- bunit.web (1.40.0)
- Moq (4.20.72)
- xunit (2.5.3+)

## Best Practices Implemented

✅ **Dependency Injection** - All services properly registered and injected  
✅ **Component Lifecycle** - OnInitialized, OnAfterRender, IDisposable  
✅ **State Management** - Scoped services with event-driven updates  
✅ **Error Boundaries** - Graceful error handling with fallback UI  
✅ **CSS Isolation** - Component-scoped styling  
✅ **Performance Optimization** - @key directives, SignalR configuration  
✅ **Structured Logging** - ILogger integration with log levels  
✅ **Comprehensive Testing** - Unit tests + component tests  
✅ **Clean Architecture** - Separation of concerns (UI, services, models)  
✅ **Responsive Design** - Bootstrap grid system  

## Acceptance Criteria Met

✅ Blazor Server project added to solution  
✅ All six calculator operations functional  
✅ All 50 xUnit tests passing  
✅ Application runs locally via `dotnet run`  
✅ Input validation and error handling  
✅ Calculation history tracking  
✅ bUnit component tests with 80%+ coverage  
✅ Error boundary displays fallback UI  
✅ Dependency injection services registered  
✅ Component lifecycle methods implemented  

## Future Enhancements

- Keyboard shortcuts for operations
- Calculation history export (CSV, JSON)
- Scientific calculator mode
- Theme customization (dark mode)
- Advanced operations (trigonometry, logarithms)
- Persistent history with database
- Mobile-optimized responsive design
- Accessibility improvements (ARIA labels)

## License

This project follows the same license as the parent repository.

## Contributing

1. Ensure all tests pass before submitting changes
2. Follow C# coding conventions
3. Add bUnit tests for new UI components
4. Update documentation for new features

## Support

For issues or questions:
1. Check the Troubleshooting section above
2. Review test output for specific errors
3. Check console logs for detailed diagnostics
4. Refer to the PRD for implementation details

---

**Built with ❤️ using Blazor Server and .NET 9**
