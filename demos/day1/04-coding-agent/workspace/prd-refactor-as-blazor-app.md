# 1. Product Requirements Document (PRD): Blazor Server Calculator Refactoring

## 1.1 Document Information

- **Version:** 2.0
- **Author:** GitHub Copilot
- **Date:** November 5th, 2025
- **Status:** Draft - Enhanced with Blazor Best Practices

## 1.2 Executive Summary

This document describes the requirements for refactoring the existing .NET 8 console calculator application into a Blazor Server web application. The project will preserve all existing calculator functionality and test coverage while providing a modern, interactive web-based user interface that can be tested locally. The core `CalculatorOperations` class will remain unchanged to maintain compatibility with the existing xUnit test suite.

## 1.3 Problem Statement

The current console calculator application demonstrates excellent .NET practices and comprehensive testing but lacks the accessibility and user experience benefits of a web interface. Developers and users need:

- A web-based calculator that can be accessed through a browser
- An interactive UI that provides immediate visual feedback
- A solution that maintains the existing business logic and test coverage
- A practical example of migrating console applications to Blazor Server architecture
- The ability to test and run the application locally during development

## 1.4 Goals and Objectives

- Refactor the console calculator into a Blazor Server web application
- Preserve all existing calculator operations and business logic in the `CalculatorOperations` class
- Maintain 100% compatibility with the existing 50 xUnit tests
- Create an intuitive, responsive web UI for calculator operations
- Enable local testing and development with minimal setup
- Demonstrate Blazor Server best practices including component architecture and data binding
- Provide comprehensive documentation for setup, testing, and deployment

## 1.5 Scope

### 1.5.1 In Scope

- New Blazor Server project (`calculator.web`) added to existing solution
- CalculatorService wrapper class for improved testability with Playwright
- Calculator.razor component with calculator keypad UI layout
- Calculation history tracking functionality with scoped service pattern
- Keyboard shortcuts for operators and Enter key to calculate
- Input validation and error handling in the web interface
- Support for all six arithmetic operations (+, -, *, /, %, ^)
- Bootstrap-based styling for clean, professional calculator appearance
- Project reference to existing calculator library
- PowerShell setup script for automation
- Local testing documentation
- Blazor-specific error display mechanisms

### 1.5.2 Out of Scope

- Modifications to the existing console application
- Changes to the `CalculatorOperations` class or test suite
- Authentication or user management
- Calculation history persistence to database (in-memory only for this version)
- Deployment to production environments
- Mobile-specific UI optimizations beyond responsive design
- Advanced calculator features beyond the six existing operations
- API endpoints or REST services
- Export/import of calculation history

## 1.6 User Stories / Use Cases

- As a user, I want to access the calculator through a web browser so I can perform calculations without installing software
- As a user, I want to select operations from a dropdown menu so I can easily choose the correct operation
- As a user, I want immediate visual feedback when I enter invalid data so I can correct mistakes quickly
- As a user, I want to see calculation results displayed clearly on the same page so I don't lose context
- As a developer, I want to test the web application locally so I can verify changes before deployment
- As a developer, I want the business logic to remain separate from the UI so I can maintain and test it independently
- As a QA tester, I want all existing unit tests to continue passing so I can verify no regression has occurred

## 1.7 Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | The Blazor application shall provide input fields for first operand and second operand |
| FR-2 | The Blazor application shall provide a dropdown selector for choosing operations (+, -, *, /, %, ^) |
| FR-3 | The Blazor application shall include a "Calculate" button to trigger the operation |
| FR-4 | The Blazor application shall display calculation results on the same page |
| FR-5 | The Blazor application shall validate numeric input and display appropriate error messages |
| FR-6 | The Blazor application shall handle division by zero errors gracefully with user-friendly messages |
| FR-7 | The Blazor application shall handle modulo by zero errors gracefully with user-friendly messages |
| FR-8 | The Blazor application shall use the existing `CalculatorOperations` static class for all calculations |
| FR-9 | The Blazor application shall clear previous error messages when new calculations are performed |
| FR-10 | The Blazor application shall support decimal number inputs |
| FR-11 | The Blazor application shall be accessible via localhost for testing |
| FR-12 | All 50 existing xUnit tests shall continue to pass without modification |
| FR-13 | The solution shall include both console and web projects simultaneously |
| FR-14 | The Blazor application shall provide immediate visual feedback for user interactions |
| FR-15 | The web interface shall be responsive and functional on different browser window sizes |
| FR-16 | The application shall maintain in-memory calculation history scoped to user's circuit with timestamp, operands, operator, and result |
| FR-17 | The application shall display up to 50 most recent calculations with ability to clear history |
| FR-18 | The application shall provide structured error logging and circuit event monitoring via ILogger |

## 1.8 Non-Functional Requirements

- **Performance:** Calculations shall execute and display results within 100ms
- **Usability:** The web interface shall be intuitive requiring no training or documentation for basic use
- **Reliability:** The application shall handle all edge cases and invalid inputs without crashing
- **Testability:** All existing unit tests shall remain valid and passing
- **Maintainability:** Code shall follow Blazor and C# best practices with clear separation of concerns
- **Compatibility:** The application shall run on .NET 8 and be testable in modern browsers (Chrome, Edge, Firefox)
- **Development Experience:** Local testing shall require only `dotnet run` command with no additional configuration
- **Accessibility:** The UI shall follow basic web accessibility guidelines (semantic HTML, keyboard navigation)

## 1.9 Assumptions and Dependencies

- The existing calculator solution structure is intact and all tests are passing
- Development environment has .NET 8 SDK installed
- The `CalculatorOperations` class will not be modified
- Developers have access to a modern web browser for testing
- The Blazor Server template and dependencies are available through .NET SDK
- Local development will use HTTPS with dev certificates
- Bootstrap CSS framework (included by default in Blazor template) is acceptable for styling

## 1.10 Success Criteria / KPIs

- Blazor Server project successfully added to solution and builds without errors
- All six calculator operations function correctly in the web interface
- All 50 xUnit tests continue to pass
- Application runs locally using `dotnet run` and is accessible in browser
- Input validation prevents invalid operations and provides clear error messages
- Zero regression in existing calculator functionality
- Documentation clearly explains local testing procedure
- Setup script successfully automates project creation

## 1.11 Milestones & Timeline

1. **Project Setup (Phase 1)**
   - Create Blazor Server project in solution
   - Configure project references to calculator library
   - Verify solution builds successfully

2. **Component Development (Phase 2)**
   - Create Calculator.razor component with basic UI
   - Implement data binding for inputs and operator selection
   - Wire up Calculate button to call `CalculatorOperations` methods

3. **Error Handling & Validation (Phase 3)**
   - Add input validation for numeric fields
   - Implement error display for division/modulo by zero
   - Add user-friendly error messages for invalid inputs

4. **Styling & UX (Phase 4)**
   - Apply Bootstrap styling for professional appearance
   - Ensure responsive layout
   - Add visual feedback for user interactions

5. **Testing & Verification (Phase 5)**
   - Run all 50 xUnit tests to verify no regression
   - Perform manual testing of web interface
   - Test all six operations with various inputs
   - Verify local testing workflow

6. **Automation & Documentation (Phase 6)**
   - Create PowerShell setup script
   - Document local testing instructions
   - Update README with Blazor-specific information

## 1.12 Implementation Guidance

### 1.12.1 Project Structure

The refactored solution will have the following structure:

```text
calculator-xunit-testing/
├── calculator/                    # Existing console app (unchanged)
│   ├── Calculator.cs
│   └── calculator.csproj
├── calculator.tests/              # Existing xUnit tests (unchanged)
│   ├── CalculatorTest.cs
│   └── calculator.tests.csproj
├── calculator.web/                # New Blazor Server project
│   ├── Pages/
│   │   ├── Calculator.razor      # Main calculator component
│   │   ├── Calculator.razor.css  # Component-scoped CSS
│   │   ├── Index.razor           # Home page (optional redirect)
│   │   └── Error.razor           # Error page (template default)
│   ├── Services/
│   │   ├── ICalculatorService.cs # Calculator service interface
│   │   ├── CalculatorService.cs  # Calculator service implementation
│   │   ├── ICalculationHistoryService.cs # History service interface
│   │   ├── CalculationHistoryService.cs  # History service implementation
│   │   ├── CalculationEntry.cs   # History entry model
│   │   └── CalculatorCircuitHandler.cs # Custom circuit handler
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── wwwroot/
│   │   └── css/                  # Custom styles if needed
│   ├── App.razor
│   ├── Program.cs
│   └── calculator.web.csproj
├── calculator.web.tests/          # New bUnit test project
│   ├── CalculatorComponentTests.cs
│   └── calculator.web.tests.csproj
└── calculator.sln                 # Updated solution file
```

### 1.12.2 Blazor Project Creation

**PowerShell Script: `Set-BlazorCalculator.ps1`**

```powershell
# Navigate to solution directory
$repoRoot = git rev-parse --show-toplevel
$workspacePath = "demos\day1\04-coding-agent\workspace\calculator-xunit-testing"
Set-Location "$repoRoot\$workspacePath"

# Create Blazor Server project
dotnet new blazorserver -n calculator.web -o calculator.web

# Add project to solution
dotnet sln add calculator.web/calculator.web.csproj

# Add reference to calculator library
dotnet add calculator.web/calculator.web.csproj reference calculator/calculator.csproj

# Build solution to verify
dotnet build

Write-Host "Blazor Server project created successfully!" -ForegroundColor Green
Write-Host "To run the web application:" -ForegroundColor Cyan
Write-Host "  cd calculator.web" -ForegroundColor Yellow
Write-Host "  dotnet run" -ForegroundColor Yellow
```

### 1.12.3 Calculator.razor Component Implementation

**Component Location:** `calculator.web/Pages/Calculator.razor`

**Key Features:**

- `@page "/calculator"` directive for routing
- Two-way data binding with `@bind` for input fields
- Dropdown for operator selection
- Button click event handler for calculation
- Conditional rendering for results and errors
- Try-catch block for exception handling

**Component Structure:**

```razor
@page "/calculator"
@using calculator

<PageTitle>Calculator</PageTitle>

<h1>Blazor Calculator</h1>

<div class="calculator-container">
    <!-- First Operand Input -->
    <div class="mb-3">
        <label>First Number:</label>
        <input type="number" @bind="firstOperand" class="form-control" />
    </div>

    <!-- Second Operand Input -->
    <div class="mb-3">
        <label>Second Number:</label>
        <input type="number" @bind="secondOperand" class="form-control" />
    </div>

    <!-- Operator Selection -->
    <div class="mb-3">
        <label>Operation:</label>
        <select @bind="selectedOperator" class="form-select">
            <option value="+">Addition (+)</option>
            <option value="-">Subtraction (-)</option>
            <option value="*">Multiplication (*)</option>
            <option value="/">Division (/)</option>
            <option value="%">Modulo (%)</option>
            <option value="^">Exponent (^)</option>
        </select>
    </div>

    <!-- Calculate Button -->
    <button class="btn btn-primary" @onclick="Calculate">Calculate</button>

    <!-- Result Display -->
    @if (hasResult)
    {
        <div class="alert alert-success mt-3">
            <strong>Result:</strong> @result
        </div>
    }

    <!-- Error Display -->
    @if (!string.IsNullOrEmpty(errorMessage))
    {
        <div class="alert alert-danger mt-3">
            <strong>Error:</strong> @errorMessage
        </div>
    }
</div>

@code {
    private double firstOperand = 0;
    private double secondOperand = 0;
    private string selectedOperator = "+";
    private double result = 0;
    private bool hasResult = false;
    private string errorMessage = string.Empty;

    private void Calculate()
    {
        // Clear previous results and errors
        hasResult = false;
        errorMessage = string.Empty;

        try
        {
            result = selectedOperator switch
            {
                "+" => CalculatorOperations.Add(firstOperand, secondOperand),
                "-" => CalculatorOperations.Subtract(firstOperand, secondOperand),
                "*" => CalculatorOperations.Multiply(firstOperand, secondOperand),
                "/" => CalculatorOperations.Divide(firstOperand, secondOperand),
                "%" => CalculatorOperations.Modulo(firstOperand, secondOperand),
                "^" => CalculatorOperations.Exponent(firstOperand, secondOperand),
                _ => throw new InvalidOperationException("Invalid operator")
            };

            hasResult = true;
        }
        catch (DivideByZeroException ex)
        {
            errorMessage = ex.Message;
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
        }
    }
}
```

### 1.12.4 Navigation Configuration

**Update NavMenu.razor** to include calculator link:

```razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="calculator">
        <span class="oi oi-calculator" aria-hidden="true"></span> Calculator
    </NavLink>
</div>
```

**Optional: Update Index.razor** to redirect to calculator:

```razor
@page "/"

<PageTitle>Home</PageTitle>

<h1>Welcome to Blazor Calculator</h1>

<p>
    <a href="/calculator">Go to Calculator</a>
</p>
```

### 1.12.5 Custom Styling (Optional)

**Create:** `calculator.web/wwwroot/css/calculator.css`

```css
.calculator-container {
    max-width: 500px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #f9f9f9;
}

.calculator-container input,
.calculator-container select {
    font-size: 1.1rem;
}

.calculator-container button {
    width: 100%;
    font-size: 1.2rem;
    padding: 10px;
}
```

**Reference in App.razor:**

```razor
<link href="css/calculator.css" rel="stylesheet" />
```

### 1.12.6 Local Testing Instructions

1. **Navigate to the Blazor project directory:**

   ```powershell
   cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing\calculator.web
   ```

2. **Run the application:**

   ```powershell
   dotnet run
   ```

3. **Access the application:**
   - Open browser to `https://localhost:5001` or `http://localhost:5000`
   - Or use the URL displayed in the console output

4. **Test calculator functionality:**
   - Navigate to `/calculator` route
   - Enter two numbers
   - Select an operation
   - Click "Calculate"
   - Verify result displays correctly

5. **Test error handling:**
   - Try division by zero
   - Try modulo by zero
   - Verify error messages display appropriately

### 1.12.7 Verification Steps

1. **Build verification:**

   ```powershell
   dotnet build
   ```

   - All projects should build without errors

2. **Test verification:**

   ```powershell
   cd calculator.tests
   dotnet test
   ```

   - All 50 tests should pass

3. **Web application verification:**
   - Run the Blazor app locally
   - Test all six operations
   - Verify input validation
   - Verify error handling

### 1.12.8 Dependency Injection & Service Layer

**Rationale:** Dependency injection improves testability, enables proper service lifetime management, and follows Blazor Server best practices for circuit-scoped state isolation.

#### 1.12.8.1 Service Interface Definition

**Create:** `calculator.web/Services/ICalculatorService.cs`

```csharp
namespace calculator.web.Services;

/// <summary>
/// Interface for calculator operations service.
/// Wraps the static CalculatorOperations class for dependency injection.
/// </summary>
public interface ICalculatorService
{
    /// <summary>
    /// Performs addition of two numbers.
    /// </summary>
    double Add(double a, double b);

    /// <summary>
    /// Performs subtraction of two numbers.
    /// </summary>
    double Subtract(double a, double b);

    /// <summary>
    /// Performs multiplication of two numbers.
    /// </summary>
    double Multiply(double a, double b);

    /// <summary>
    /// Performs division of two numbers.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when divisor is zero.</exception>
    double Divide(double a, double b);

    /// <summary>
    /// Performs modulo operation on two numbers.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when divisor is zero.</exception>
    double Modulo(double a, double b);

    /// <summary>
    /// Performs exponentiation (a raised to the power of b).
    /// </summary>
    double Exponent(double a, double b);
}
```

#### 1.12.8.2 Service Implementation

**Create:** `calculator.web/Services/CalculatorService.cs`

```csharp
using calculator;

namespace calculator.web.Services;

/// <summary>
/// Calculator service implementation that wraps the static CalculatorOperations class.
/// This wrapper enables dependency injection and improved testability.
/// </summary>
public class CalculatorService : ICalculatorService
{
    public double Add(double a, double b) => CalculatorOperations.Add(a, b);
    public double Subtract(double a, double b) => CalculatorOperations.Subtract(a, b);
    public double Multiply(double a, double b) => CalculatorOperations.Multiply(a, b);
    public double Divide(double a, double b) => CalculatorOperations.Divide(a, b);
    public double Modulo(double a, double b) => CalculatorOperations.Modulo(a, b);
    public double Exponent(double a, double b) => CalculatorOperations.Exponent(a, b);
}
```

#### 1.12.8.3 Service Registration

**Update:** `calculator.web/Program.cs`

```csharp
using calculator.web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    // Configure SignalR hub options for optimal performance
    options.DetailedErrors = builder.Environment.IsDevelopment();
});

// Register scoped services (scoped to the Blazor circuit/user session)
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<ICalculationHistoryService, CalculationHistoryService>();

// Register custom circuit handler for connection monitoring
builder.Services.AddScoped<CircuitHandler, CalculatorCircuitHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
```

#### 1.12.8.4 Component Injection

**Update Calculator.razor to use injected service:**

```razor
@page "/calculator"
@using calculator.web.Services
@inject ICalculatorService CalculatorService
@inject ICalculationHistoryService HistoryService
@inject ILogger<Calculator> Logger

<!-- Component markup remains the same -->

@code {
    private void Calculate()
    {
        hasResult = false;
        errorMessage = string.Empty;

        try
        {
            result = selectedOperator switch
            {
                "+" => CalculatorService.Add(firstOperand, secondOperand),
                "-" => CalculatorService.Subtract(firstOperand, secondOperand),
                "*" => CalculatorService.Multiply(firstOperand, secondOperand),
                "/" => CalculatorService.Divide(firstOperand, secondOperand),
                "%" => CalculatorService.Modulo(firstOperand, secondOperand),
                "^" => CalculatorService.Exponent(firstOperand, secondOperand),
                _ => throw new InvalidOperationException("Invalid operator")
            };

            hasResult = true;
            
            // Add to history
            HistoryService.AddEntry(firstOperand, secondOperand, selectedOperator, result);
            
            // Log successful calculation
            Logger.LogInformation(
                "Calculation performed: {First} {Operator} {Second} = {Result}",
                firstOperand, selectedOperator, secondOperand, result);
        }
        catch (DivideByZeroException ex)
        {
            errorMessage = ex.Message;
            Logger.LogWarning(ex, "Division/Modulo by zero attempted");
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred: {ex.Message}";
            Logger.LogError(ex, "Unexpected error during calculation");
        }
    }
}
```

#### 1.12.8.5 Service Lifetime Explanation

**Scoped Services in Blazor Server:**

- **Scoped Lifetime:** Services are created once per Blazor circuit (user session)
- **Circuit:** Represents a single user's connection to the server via SignalR
- **Isolation:** Each user gets their own instance, preventing data leakage between users
- **Lifecycle:** Services are disposed when the circuit ends (user navigates away or connection drops)

**Why Scoped for Calculator Services:**

1. `ICalculatorService`: Stateless, but scoped for consistency
2. `ICalculationHistoryService`: Maintains user-specific history in memory
3. Each user's calculation history remains isolated and private
4. Memory is automatically cleaned up when user disconnects

### 1.12.9 Component Lifecycle Methods

**Purpose:** Understanding and utilizing Blazor component lifecycle methods enables proper initialization, resource management, and optimal rendering behavior.

#### 1.12.9.1 Lifecycle Execution Order

```
1. SetParametersAsync()
   ↓
2. OnInitialized() / OnInitializedAsync()
   ↓
3. OnParametersSet() / OnParametersSetAsync()
   ↓
4. StateHasChanged() [automatic]
   ↓
5. Render component
   ↓
6. OnAfterRender(bool firstRender) / OnAfterRenderAsync(bool firstRender)
```

#### 1.12.9.2 Common Lifecycle Patterns

**OnInitialized() - Component Initialization:**

```csharp
protected override void OnInitialized()
{
    // Initialize component state
    // Load initial data
    // Subscribe to events
    
    Logger.LogInformation("Calculator component initialized");
}
```

**OnInitializedAsync() - Async Initialization:**

```csharp
protected override async Task OnInitializedAsync()
{
    // Use for async operations during initialization
    // Example: Load data from API or database
    
    await Task.CompletedTask; // Placeholder for async work
}
```

**OnAfterRender() - Post-Render Operations:**

```csharp
protected override void OnAfterRender(bool firstRender)
{
    if (firstRender)
    {
        // Executes only once after first render
        // Use for JavaScript interop, focus management
        // Example: Focus first input field
    }
}
```

**IDisposable - Resource Cleanup:**

```csharp
public void Dispose()
{
    // Clean up resources
    // Unsubscribe from events
    // Dispose of disposable objects
    
    Logger.LogInformation("Calculator component disposed");
}
```

**Full Calculator Component with Lifecycle:**

```razor
@page "/calculator"
@implements IDisposable
@inject ILogger<Calculator> Logger

@code {
    protected override void OnInitialized()
    {
        Logger.LogInformation("Calculator initialized at {Time}", DateTime.UtcNow);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            Logger.LogInformation("Calculator first render complete");
        }
    }

    public void Dispose()
    {
        Logger.LogInformation("Calculator component disposing");
    }
}
```

#### 1.12.9.3 Manual State Refresh

**StateHasChanged() - Force UI Update:**

```csharp
private async Task LongRunningOperation()
{
    // Perform work
    await Task.Delay(1000);
    
    // Manually trigger UI update
    StateHasChanged();
}
```

**InvokeAsync() - Thread-Safe Updates:**

```csharp
private void OnExternalEvent(object sender, EventArgs e)
{
    // Ensure updates occur on the synchronization context
    InvokeAsync(() =>
    {
        // Update state
        StateHasChanged();
    });
}
```

### 1.12.10 Calculation History with StateContainer Pattern

**Purpose:** Implement in-memory calculation history using scoped services to track user calculations within their Blazor circuit session.

#### 1.12.10.1 History Entry Model

**Create:** `calculator.web/Services/CalculationEntry.cs`

```csharp
namespace calculator.web.Services;

/// <summary>
/// Represents a single calculation entry in the history.
/// </summary>
public class CalculationEntry
{
    /// <summary>
    /// Timestamp when the calculation was performed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// First operand of the calculation.
    /// </summary>
    public double FirstOperand { get; set; }

    /// <summary>
    /// Second operand of the calculation.
    /// </summary>
    public double SecondOperand { get; set; }

    /// <summary>
    /// Operator used (+, -, *, /, %, ^).
    /// </summary>
    public string Operator { get; set; } = string.Empty;

    /// <summary>
    /// Result of the calculation.
    /// </summary>
    public double Result { get; set; }

    /// <summary>
    /// Formatted display string for the calculation.
    /// </summary>
    public string Display => $"{FirstOperand} {Operator} {SecondOperand} = {Result}";
}
```

#### 1.12.10.2 History Service Interface

**Create:** `calculator.web/Services/ICalculationHistoryService.cs`

```csharp
namespace calculator.web.Services;

/// <summary>
/// Service for managing calculation history.
/// </summary>
public interface ICalculationHistoryService
{
    /// <summary>
    /// Gets all calculation history entries.
    /// </summary>
    IReadOnlyList<CalculationEntry> GetHistory();

    /// <summary>
    /// Adds a new calculation to the history.
    /// </summary>
    void AddEntry(double firstOperand, double secondOperand, string operatorSymbol, double result);

    /// <summary>
    /// Clears all history entries.
    /// </summary>
    void ClearHistory();

    /// <summary>
    /// Gets the count of history entries.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Event raised when history changes.
    /// </summary>
    event EventHandler? HistoryChanged;
}
```

#### 1.12.10.3 History Service Implementation

**Create:** `calculator.web/Services/CalculationHistoryService.cs`

```csharp
namespace calculator.web.Services;

/// <summary>
/// Implementation of calculation history service.
/// Maintains in-memory history scoped to the Blazor circuit.
/// </summary>
public class CalculationHistoryService : ICalculationHistoryService
{
    private readonly List<CalculationEntry> _history = new();
    private const int MaxHistoryEntries = 50;

    public event EventHandler? HistoryChanged;

    public int Count => _history.Count;

    public IReadOnlyList<CalculationEntry> GetHistory()
    {
        // Return most recent entries first
        return _history.AsReadOnly();
    }

    public void AddEntry(double firstOperand, double secondOperand, string operatorSymbol, double result)
    {
        var entry = new CalculationEntry
        {
            Timestamp = DateTime.UtcNow,
            FirstOperand = firstOperand,
            SecondOperand = secondOperand,
            Operator = operatorSymbol,
            Result = result
        };

        // Add to beginning of list (most recent first)
        _history.Insert(0, entry);

        // Enforce maximum history limit (FIFO removal)
        if (_history.Count > MaxHistoryEntries)
        {
            _history.RemoveAt(_history.Count - 1);
        }

        // Notify subscribers of change
        HistoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearHistory()
    {
        _history.Clear();
        HistoryChanged?.Invoke(this, EventArgs.Empty);
    }
}
```

#### 1.12.10.4 Calculator Component with History Display

**Update:** `calculator.web/Pages/Calculator.razor`

```razor
@page "/calculator"
@using calculator.web.Services
@inject ICalculatorService CalculatorService
@inject ICalculationHistoryService HistoryService
@inject ILogger<Calculator> Logger
@implements IDisposable

<PageTitle>Calculator</PageTitle>

<h1>Blazor Calculator</h1>

<div class="row">
    <div class="col-md-6">
        <div class="calculator-container">
            <!-- Calculator inputs (existing markup) -->
            <!-- ... -->
        </div>
    </div>
    
    <div class="col-md-6">
        <div class="history-container">
            <h3>Calculation History</h3>
            
            @if (HistoryService.Count == 0)
            {
                <p class="text-muted">No calculations yet.</p>
            }
            else
            {
                <div class="mb-3">
                    <button class="btn btn-sm btn-outline-danger" @onclick="ClearHistory">
                        <span class="oi oi-trash"></span> Clear History
                    </button>
                    <span class="badge bg-secondary ms-2">@HistoryService.Count entries</span>
                </div>
                
                <div class="table-responsive" style="max-height: 400px; overflow-y: auto;">
                    <table class="table table-sm table-striped">
                        <thead>
                            <tr>
                                <th>Time</th>
                                <th>Calculation</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var entry in HistoryService.GetHistory())
                            {
                                <tr @key="entry.Timestamp">
                                    <td>@entry.Timestamp.ToLocalTime().ToString("HH:mm:ss")</td>
                                    <td><code>@entry.Display</code></td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>
            }
        </div>
    </div>
</div>

@code {
    protected override void OnInitialized()
    {
        // Subscribe to history changes
        HistoryService.HistoryChanged += OnHistoryChanged;
    }

    private void OnHistoryChanged(object? sender, EventArgs e)
    {
        // Refresh UI when history changes
        InvokeAsync(StateHasChanged);
    }

    private void ClearHistory()
    {
        HistoryService.ClearHistory();
        Logger.LogInformation("Calculation history cleared");
    }

    public void Dispose()
    {
        // Unsubscribe from events
        HistoryService.HistoryChanged -= OnHistoryChanged;
    }
}
```

#### 1.12.10.5 History Styling

**Create:** `calculator.web/Pages/Calculator.razor.css`

```css
.history-container {
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #f9f9f9;
}

.history-container h3 {
    margin-bottom: 15px;
    font-size: 1.25rem;
}

.table-responsive {
    border: 1px solid #dee2e6;
    border-radius: 4px;
}
```

## 1.13 UI Design Decisions

### 1.13.1 Layout Approach

**Selected Approach:** Form-based layout with dropdown operator selection

**Rationale:**

- Aligns with the console application's sequential input pattern
- Simpler to implement and maintain
- Easier to validate inputs
- More accessible for keyboard navigation
- Suitable for educational/demonstration purposes

**Alternative Considered:** Calculator button grid (iOS-style)

- More complex implementation
- Requires more extensive state management
- Better for rapid repeated calculations
- May be implemented in future iterations

### 1.13.2 Error Handling UI

**Selected Approach:** Inline alert messages below the calculate button

**Rationale:**

- Keeps errors in context with the inputs
- Uses Bootstrap alert components (already available)
- Clear visual distinction between results and errors
- No additional libraries required

**Alternatives Considered:**

- Toast notifications (requires additional library)
- Modal dialogs (more intrusive)
- Inline validation under each input (more complex state management)

### 1.13.3 Result Display

**Selected Approach:** Success alert box with result value

**Rationale:**

- Visually distinct from error messages (green vs. red)
- Persistent until next calculation
- Uses standard Bootstrap styling
- Clear and prominent display

## 1.14 Testing Requirements

### 1.14.1 Unit Test Preservation

- All 50 existing xUnit tests must pass without modification
- Test execution should be part of build verification
- No changes to `CalculatorOperations` class or test project

### 1.14.2 Manual Testing Checklist

- [ ] All six operations produce correct results
- [ ] Division by zero displays appropriate error
- [ ] Modulo by zero displays appropriate error
- [ ] Decimal numbers are handled correctly
- [ ] Negative numbers are handled correctly
- [ ] Large numbers are handled correctly
- [ ] Switching operations clears previous results
- [ ] Error messages clear when new calculation succeeds
- [ ] Application is accessible via keyboard navigation
- [ ] Application displays correctly in Chrome, Edge, and Firefox

### 1.14.3 Integration Testing

- [ ] Calculator.razor component correctly calls `CalculatorOperations` methods
- [ ] Project references are configured correctly
- [ ] Solution builds without warnings
- [ ] Application runs locally without errors

### 1.14.4 Blazor Component Testing with bUnit

**Purpose:** bUnit is a testing library for Blazor components that enables rendering components, simulating user interactions, and verifying output in isolation.

#### 1.14.4.1 Test Project Setup

**Create Test Project:**

```powershell
# Navigate to solution directory
cd demos\day1\04-coding-agent\workspace\calculator-xunit-testing

# Create xUnit test project
dotnet new xunit -n calculator.web.tests

# Add to solution
dotnet sln add calculator.web.tests/calculator.web.tests.csproj

# Add project reference to web project
dotnet add calculator.web.tests reference calculator.web/calculator.web.csproj

# Add bUnit packages
dotnet add calculator.web.tests package bunit
dotnet add calculator.web.tests package bunit.web
dotnet add calculator.web.tests package Moq
```

#### 1.14.4.2 Example Component Tests

**Create:** `calculator.web.tests/CalculatorComponentTests.cs`

```csharp
using Bunit;
using calculator.web.Pages;
using calculator.web.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace calculator.web.tests;

/// <summary>
/// Component tests for Calculator.razor using bUnit.
/// </summary>
public class CalculatorComponentTests : TestContext
{
    private readonly Mock<ICalculatorService> _mockCalculatorService;
    private readonly Mock<ICalculationHistoryService> _mockHistoryService;
    private readonly Mock<ILogger<Calculator>> _mockLogger;

    public CalculatorComponentTests()
    {
        _mockCalculatorService = new Mock<ICalculatorService>();
        _mockHistoryService = new Mock<ICalculationHistoryService>();
        _mockLogger = new Mock<ILogger<Calculator>>();

        // Register mock services
        Services.AddSingleton(_mockCalculatorService.Object);
        Services.AddSingleton(_mockHistoryService.Object);
        Services.AddSingleton(_mockLogger.Object);
    }

    [Fact]
    public void Calculator_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Calculator>();

        // Assert
        cut.MarkupMatches(@"
            <h1>Blazor Calculator</h1>
            <div class=""row"">
                <!-- Component content -->
            </div>
        ");
        
        // Verify calculator elements exist
        var firstInput = cut.Find("input[type='number']:first-of-type");
        var secondInput = cut.Find("input[type='number']:last-of-type");
        var operatorSelect = cut.Find("select");
        var calculateButton = cut.Find("button");
        
        Assert.NotNull(firstInput);
        Assert.NotNull(secondInput);
        Assert.NotNull(operatorSelect);
        Assert.NotNull(calculateButton);
    }

    [Fact]
    public void Calculate_WithAddition_DisplaysCorrectResult()
    {
        // Arrange
        _mockCalculatorService.Setup(s => s.Add(5, 3)).Returns(8);
        var cut = RenderComponent<Calculator>();

        // Act
        var firstInput = cut.Find("input[type='number']:first-of-type");
        var secondInput = cut.Find("input[type='number']:last-of-type");
        var operatorSelect = cut.Find("select");
        var calculateButton = cut.Find("button");

        firstInput.Change(5);
        secondInput.Change(3);
        operatorSelect.Change("+");
        calculateButton.Click();

        // Assert
        var resultDiv = cut.Find(".alert-success");
        Assert.Contains("8", resultDiv.TextContent);
        
        _mockCalculatorService.Verify(s => s.Add(5, 3), Times.Once);
        _mockHistoryService.Verify(s => s.AddEntry(5, 3, "+", 8), Times.Once);
    }

    [Fact]
    public void Calculate_WithDivisionByZero_DisplaysError()
    {
        // Arrange
        _mockCalculatorService
            .Setup(s => s.Divide(10, 0))
            .Throws(new DivideByZeroException("Cannot divide by zero"));
        var cut = RenderComponent<Calculator>();

        // Act
        var firstInput = cut.Find("input[type='number']:first-of-type");
        var secondInput = cut.Find("input[type='number']:last-of-type");
        var operatorSelect = cut.Find("select");
        var calculateButton = cut.Find("button");

        firstInput.Change(10);
        secondInput.Change(0);
        operatorSelect.Change("/");
        calculateButton.Click();

        // Assert
        var errorDiv = cut.Find(".alert-danger");
        Assert.Contains("Cannot divide by zero", errorDiv.TextContent);
        
        // Verify history was not updated
        _mockHistoryService.Verify(
            s => s.AddEntry(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<double>()),
            Times.Never);
    }

    [Fact]
    public void ClearHistory_InvokesHistoryService()
    {
        // Arrange
        var cut = RenderComponent<Calculator>();

        // Act
        var clearButton = cut.Find("button:contains('Clear History')");
        clearButton.Click();

        // Assert
        _mockHistoryService.Verify(s => s.ClearHistory(), Times.Once);
    }

    [Theory]
    [InlineData("+", 5, 3, 8)]
    [InlineData("-", 10, 4, 6)]
    [InlineData("*", 6, 7, 42)]
    [InlineData("/", 15, 3, 5)]
    public void Calculate_WithVariousOperations_CallsCorrectServiceMethod(
        string operatorSymbol, double first, double second, double expected)
    {
        // Arrange
        switch (operatorSymbol)
        {
            case "+":
                _mockCalculatorService.Setup(s => s.Add(first, second)).Returns(expected);
                break;
            case "-":
                _mockCalculatorService.Setup(s => s.Subtract(first, second)).Returns(expected);
                break;
            case "*":
                _mockCalculatorService.Setup(s => s.Multiply(first, second)).Returns(expected);
                break;
            case "/":
                _mockCalculatorService.Setup(s => s.Divide(first, second)).Returns(expected);
                break;
        }

        var cut = RenderComponent<Calculator>();

        // Act
        var firstInput = cut.Find("input[type='number']:first-of-type");
        var secondInput = cut.Find("input[type='number']:last-of-type");
        var operatorSelect = cut.Find("select");
        var calculateButton = cut.Find("button");

        firstInput.Change(first);
        secondInput.Change(second);
        operatorSelect.Change(operatorSymbol);
        calculateButton.Click();

        // Assert
        var resultDiv = cut.Find(".alert-success");
        Assert.Contains(expected.ToString(), resultDiv.TextContent);
    }
}
```

#### 1.14.4.3 Running bUnit Tests

```powershell
# Run all tests (both xUnit and bUnit)
dotnet test

# Run only bUnit tests
cd calculator.web.tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

#### 1.14.4.4 Test Coverage Goals

- **Target:** 80%+ code coverage of Calculator component
- **Focus Areas:**
  - All six arithmetic operations
  - Error handling (division by zero, modulo by zero)
  - Input validation
  - History integration
  - Event handling (button clicks, input changes)

#### 1.14.4.5 Best Practices for bUnit Testing

1. **Use Mocks for Services:** Always mock `ICalculatorService`, `ICalculationHistoryService`, and `ILogger`
2. **Test User Interactions:** Simulate clicks, input changes, and form submissions
3. **Verify Service Calls:** Use `Moq.Verify()` to ensure services are called correctly
4. **Test Error Paths:** Verify error messages display correctly
5. **Use Theory Tests:** Test multiple scenarios with `[Theory]` and `[InlineData]`
6. **Component Isolation:** Each test should render a fresh component instance

### 1.12.11 Error Handling & Resilience

**Purpose:** Implement robust error handling using Blazor's `ErrorBoundary` component, custom circuit handlers, and structured logging to provide graceful degradation and diagnostics.

#### 1.12.11.1 Error Boundary Implementation

**Update:** `calculator.web/App.razor`

```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <base href="~/" />
    <link rel="stylesheet" href="css/bootstrap/bootstrap.min.css" />
    <link href="css/site.css" rel="stylesheet" />
    <link href="calculator.web.styles.css" rel="stylesheet" />
</head>
<body>
    <ErrorBoundary>
        <ChildContent>
            <Router AppAssembly="@typeof(App).Assembly">
                <Found Context="routeData">
                    <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
                    <FocusOnNavigate RouteData="@routeData" Selector="h1" />
                </Found>
                <NotFound>
                    <PageTitle>Not found</PageTitle>
                    <LayoutView Layout="@typeof(MainLayout)">
                        <p role="alert">Sorry, there's nothing at this address.</p>
                    </LayoutView>
                </NotFound>
            </Router>
        </ChildContent>
        <ErrorContent Context="exception">
            <div class="alert alert-danger" role="alert">
                <h4 class="alert-heading">
                    <span class="oi oi-warning" aria-hidden="true"></span>
                    Something went wrong
                </h4>
                <p>
                    We're sorry, but an unexpected error has occurred in the calculator application.
                </p>
                <hr />
                <p class="mb-0">
                    <strong>Error Details:</strong><br />
                    <code>@exception.Message</code>
                </p>
                <div class="mt-3">
                    <button class="btn btn-primary" @onclick="ReportIssue">
                        <span class="oi oi-bug"></span> Report Issue
                    </button>
                    <button class="btn btn-secondary" @onclick="ReloadPage">
                        <span class="oi oi-reload"></span> Reload Page
                    </button>
                </div>
            </div>
        </ErrorContent>
    </ErrorBoundary>

    <script src="_framework/blazor.server.js"></script>
</body>
</html>

@code {
    private void ReportIssue()
    {
        // For local development, log to console
        // In production, this could POST to an API endpoint
        Console.WriteLine($"[ERROR REPORT] User reported issue at {DateTime.UtcNow}");
        
        // Future: POST error details to logging API
        // await Http.PostAsJsonAsync("/api/errors", errorDetails);
    }

    private void ReloadPage()
    {
        // Reload the current page
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
}
```

#### 1.12.11.2 Custom Circuit Handler

**Create:** `calculator.web/Services/CalculatorCircuitHandler.cs`

```csharp
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace calculator.web.Services;

/// <summary>
/// Custom circuit handler for monitoring Blazor Server connections.
/// </summary>
public class CalculatorCircuitHandler : CircuitHandler
{
    private readonly ILogger<CalculatorCircuitHandler> _logger;

    public CalculatorCircuitHandler(ILogger<CalculatorCircuitHandler> logger)
    {
        _logger = logger;
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Circuit opened: {CircuitId} at {Time}",
            circuit.Id,
            DateTime.UtcNow);
        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Circuit closed: {CircuitId} at {Time}",
            circuit.Id,
            DateTime.UtcNow);
        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "Connection lost: {CircuitId} at {Time}",
            circuit.Id,
            DateTime.UtcNow);
        return base.OnConnectionDownAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Connection restored: {CircuitId} at {Time}",
            circuit.Id,
            DateTime.UtcNow);
        return base.OnConnectionUpAsync(circuit, cancellationToken);
    }
}
```

#### 1.12.11.3 Structured Logging Configuration

**Update:** `calculator.web/Program.cs` (add logging configuration)

```csharp
// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}
```

#### 1.12.11.4 SignalR Reconnection UI

**Add to:** `calculator.web/Pages/_Host.cshtml` or `App.razor`

```html
<div id="blazor-error-ui">
    <div class="alert alert-warning" role="alert">
        <strong>Connection lost!</strong>
        Reconnecting to server...
        <button class="btn btn-sm btn-warning" onclick="window.location.reload()">
            Reload Now
        </button>
    </div>
</div>
```

**Add styling to:** `calculator.web/wwwroot/css/site.css`

```css
#blazor-error-ui {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 1000;
    padding: 1rem;
    background: linear-gradient(0deg, transparent, #ffc107);
    display: none;
}

#blazor-error-ui.show {
    display: block;
}
```

#### 1.12.11.5 Error Handling Best Practices

1. **Use ErrorBoundary for Component Errors:** Wrap critical components to prevent full app crashes
2. **Log All Errors:** Use `ILogger` to capture exceptions with context
3. **User-Friendly Messages:** Never expose stack traces or technical details to end users
4. **Graceful Degradation:** Provide fallback UI when features fail
5. **Connection Monitoring:** Track circuit state for diagnostics
6. **Retry Logic:** Implement automatic reconnection for transient failures

### 1.12.12 Performance Optimization

**Purpose:** Optimize Blazor Server performance through efficient rendering, SignalR configuration, CSS isolation, and component render control.

#### 1.12.12.1 SignalR Hub Configuration

**Update:** `calculator.web/Program.cs`

```csharp
builder.Services.AddServerSideBlazor(options =>
{
    // Detailed errors for development
    options.DetailedErrors = builder.Environment.IsDevelopment();
    
    // Disconnect timeout (default: 30 seconds)
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    
    // Maximum concurrent connections per server
    options.DisconnectedCircuitMaxRetained = 100;
    
    // JavaScript interop default timeout
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
});

// Configure SignalR hub options
builder.Services.Configure<HubOptions>(options =>
{
    // Maximum message size (32 KB default, increase if needed)
    options.MaximumReceiveMessageSize = 32 * 1024;
    
    // Client timeout interval (30 seconds)
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    
    // Keep-alive interval (15 seconds)
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});
```

#### 1.12.12.2 CSS Isolation for Component Styling

**Create:** `calculator.web/Pages/Calculator.razor.css`

```css
/* Component-scoped CSS - automatically scoped to Calculator component */

.calculator-container {
    max-width: 500px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #f9f9f9;
}

.calculator-container input,
.calculator-container select {
    font-size: 1.1rem;
}

.calculator-container button {
    width: 100%;
    font-size: 1.2rem;
    padding: 10px;
}

.history-container {
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #ffffff;
    max-height: 500px;
    overflow-y: auto;
}

/* Optimize scrolling performance */
.history-container {
    will-change: scroll-position;
    -webkit-overflow-scrolling: touch;
}

/* Table styling */
.table-responsive {
    border: 1px solid #dee2e6;
    border-radius: 4px;
}
```

#### 1.12.12.3 Efficient List Rendering with @key

**Use in:** `calculator.web/Pages/Calculator.razor`

```razor
<tbody>
    @foreach (var entry in HistoryService.GetHistory())
    {
        <!-- @key directive ensures efficient DOM updates -->
        <tr @key="entry.Timestamp">
            <td>@entry.Timestamp.ToLocalTime().ToString("HH:mm:ss")</td>
            <td><code>@entry.Display</code></td>
        </tr>
    }
</tbody>
```

**Key Benefits:**
- Prevents unnecessary re-rendering of list items
- Blazor can track which items changed
- Improves performance for dynamic lists

#### 1.12.12.4 Component Render Optimization (Optional)

**Advanced:** Override `ShouldRender()` if history grows very large

```csharp
@code {
    private bool _shouldRender = true;

    protected override bool ShouldRender()
    {
        // Only re-render when explicitly needed
        if (!_shouldRender)
            return false;

        _shouldRender = false;
        return true;
    }

    private void Calculate()
    {
        // ... calculation logic ...
        
        // Explicitly request re-render
        _shouldRender = true;
    }
}
```

**Warning:** Only use `ShouldRender()` optimization when profiling shows performance issues.

#### 1.12.12.5 Virtualization for Large Lists (Future Enhancement)

**If history exceeds 100+ entries, consider virtualization:**

```razor
<Virtualize Items="@HistoryService.GetHistory()" Context="entry">
    <tr @key="entry.Timestamp">
        <td>@entry.Timestamp.ToLocalTime().ToString("HH:mm:ss")</td>
        <td><code>@entry.Display</code></td>
    </tr>
</Virtualize>
```

#### 1.12.12.6 Performance Monitoring

**Add performance logging:**

```csharp
private void Calculate()
{
    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
    
    try
    {
        // ... calculation logic ...
    }
    finally
    {
        stopwatch.Stop();
        Logger.LogInformation(
            "Calculation completed in {ElapsedMs}ms",
            stopwatch.ElapsedMilliseconds);
    }
}
```

#### 1.12.12.7 Performance Best Practices Summary

1. **Use @key for lists** - Enables efficient DOM diffing
2. **Configure SignalR appropriately** - Balance between responsiveness and resource usage
3. **CSS isolation** - Reduces global CSS overhead
4. **Minimize State Changes** - Batch updates when possible
5. **Lazy Load Components** - Use `@attribute [Lazy]` for non-critical components
6. **Monitor Performance** - Log operation timings in development
7. **Optimize Images** - Use appropriate formats and sizes in wwwroot
8. **Enable Response Compression** - Add compression middleware in production

## 1.15 Documentation Requirements

### 1.15.1 README Updates

Create a new section in the calculator README documenting:

- Blazor Server project structure
- How to run the web application locally
- Differences between console and web versions
- Troubleshooting common issues

### 1.15.2 Code Documentation

- Add XML comments to Calculator.razor component
- Document the purpose of each @code block member
- Include usage examples in component documentation

### 1.15.3 Setup Script Documentation

- Include header comments in PowerShell script
- Document prerequisites
- Document expected outcomes
- Include troubleshooting section

## 1.16 Future Enhancement Opportunities

While out of scope for this iteration, the following enhancements could be considered for future versions:

- Calculation history feature with persistence
- Advanced calculator operations (trigonometry, logarithms)
- Keyboard shortcuts for operations
- Calculator button grid layout option
- Theme customization (dark mode)
- Export calculation history
- Scientific notation support
- Unit conversion capabilities
- Mobile-optimized responsive design

## 1.17 Risk Assessment

| Risk | Impact | Mitigation |
|---|---|---|
| Breaking existing tests | High | Do not modify `CalculatorOperations` class; verify tests after each change |
| Port conflicts during local testing | Medium | Document default ports; provide instructions for changing ports |
| Browser compatibility issues | Low | Use standard Bootstrap components; test in major browsers |
| HTTPS certificate issues | Medium | Document dev certificate trust process; provide troubleshooting steps |
| Confusion between console and web apps | Low | Clear documentation; separate project folders |

## 1.18 Acceptance Criteria

The Blazor Server refactoring will be considered complete when:

1. Blazor Server project successfully added to solution
2. Calculator.razor component implements all six operations
3. All 50 xUnit tests continue to pass
4. Application runs locally using `dotnet run`
5. All calculator operations produce correct results in web UI
6. Division by zero and modulo by zero errors display appropriately
7. Input validation prevents invalid numeric entries
8. Setup script successfully automates project creation
9. Documentation includes local testing instructions
10. Manual testing checklist is completed and verified
11. **Calculation history displays correctly and clears on command**
12. **bUnit component tests pass with 80%+ code coverage of Calculator component**
13. **Error boundary displays fallback UI when component errors occur**
14. **Dependency injection services are properly registered and injected**
15. **Component lifecycle methods are implemented where appropriate**

## 1.19 Dependencies & References

- **.NET 8 SDK:** Required for building and running the application
- **Blazor Server Template:** Included with .NET 8 SDK
- **Bootstrap 5:** Included by default in Blazor Server template
- **Existing Calculator Project:** `CalculatorOperations` class must remain accessible
- **xUnit Framework:** Already configured in test project
- **bUnit:** Testing library for Blazor components (v1.x or later)
- **bUnit.web:** Web-specific extensions for bUnit
- **Moq:** Mocking framework for service dependencies in tests
- **Microsoft.Extensions.Logging:** Structured logging for diagnostics and monitoring
- **Microsoft.AspNetCore.Components.Server:** Blazor Server runtime and circuit management

## 1.20 Appendix: Command Reference

### Common Commands

```powershell
# Build entire solution
dotnet build

# Run all tests (xUnit + bUnit)
dotnet test

# Run only unit tests
cd calculator.tests
dotnet test

# Run only component tests
cd calculator.web.tests
dotnet test

# Run web application
cd calculator.web
dotnet run

# Run console application
cd calculator
dotnet run

# Create Blazor project (automated in script)
dotnet new blazorserver -n calculator.web

# Add project to solution
dotnet sln add calculator.web/calculator.web.csproj

# Add project reference
dotnet add calculator.web reference calculator/calculator.csproj

# Add bUnit test project
dotnet new xunit -n calculator.web.tests
dotnet sln add calculator.web.tests/calculator.web.tests.csproj
dotnet add calculator.web.tests reference calculator.web/calculator.web.csproj
dotnet add calculator.web.tests package bunit
dotnet add calculator.web.tests package bunit.web
dotnet add calculator.web.tests package Moq
```

### Troubleshooting Commands

```powershell
# Clear build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore

# List project references
dotnet list reference

# Trust HTTPS development certificate
dotnet dev-certs https --trust
```

## 1.21 Glossary

- **Blazor Server:** ASP.NET Core framework for building interactive web UIs using C# instead of JavaScript
- **Component:** Reusable UI element in Blazor, similar to a partial view or user control
- **Data Binding:** Automatic synchronization between UI elements and code variables using `@bind`
- **Razor:** Syntax for embedding C# code in HTML markup
- **Two-way Binding:** Changes in UI update code variables and vice versa
- **Directive:** Special Razor syntax starting with `@` that controls component behavior
- **Route:** URL pattern that maps to a Blazor component (`@page` directive)
- **Circuit:** Blazor Server's representation of a single user's session/connection via SignalR
- **SignalR:** Real-time communication library that Blazor Server uses for client-server communication
- **Scoped Service:** Service instance created once per circuit and shared across components in that circuit
- **bUnit:** Testing library specifically designed for Blazor component testing
- **ErrorBoundary:** Blazor component that catches unhandled exceptions and displays fallback UI
- **CSS Isolation:** Feature that scopes CSS rules to a specific component (ComponentName.razor.css)
- **@key Directive:** Blazor directive that helps optimize list rendering by tracking item identity
- **StateHasChanged:** Method to manually trigger component re-rendering
- **CircuitHandler:** Base class for monitoring and handling Blazor circuit lifecycle events
