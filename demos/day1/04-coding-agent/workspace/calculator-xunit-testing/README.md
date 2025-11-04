# .NET Calculator Application

A basic console calculator application built with .NET 8 and C#, demonstrating arithmetic operations with proper error handling and user input validation.

## Features

- **Basic Arithmetic Operations**
  - Addition (+)
  - Subtraction (-)
  - Multiplication (*)
  - Division (/)

- **User-Friendly Interface**
  - Clear prompts for user input
  - Formatted result display
  - Multiple calculation support
  - Graceful exit

- **Robust Error Handling**
  - Division by zero protection
  - Input validation (empty/invalid inputs)
  - Null safety (no warnings)

## Quick Start

### Prerequisites

- .NET 8 SDK or higher
- PowerShell 7.0+ (for setup script)

### Installation

The solution was created using the automated setup script:

```powershell
# From the workspace directory
.\Set-DotnetSlnForCalculator.ps1
```

### Building the Application

```bash
# From the calculator-xunit-testing directory
dotnet build
```

### Running the Application

```bash
dotnet run --project calculator
```

### Running Tests

```bash
dotnet test
```

## Usage Example

```
Enter the first number: 10
Enter an operator (+, -, *, /): +
Enter the second number: 5
Result: 10 + 5 = 15

Would you like to perform another calculation? (y/n): y

Enter the first number: 20
Enter an operator (+, -, *, /): /
Enter the second number: 4
Result: 20 / 4 = 5

Would you like to perform another calculation? (y/n): n
Thank you for using the calculator. Goodbye!
```

## Solution Structure

```
calculator-xunit-testing/
├── calculator.sln                  # Solution file
├── calculator/                     # Main application project
│   ├── Calculator.cs              # Application entry point
│   └── calculator.csproj          # Project configuration
├── calculator.tests/              # xUnit test project
│   ├── CalculatorTest.cs         # Test cases
│   └── calculator.tests.csproj   # Test project configuration
└── README.md                      # This file
```

## Implementation Details

This calculator was implemented according to the PRD (Product Requirements Document) for a .NET Calculator with xUnit Testing, specifically implementing steps 1.12.1 and 1.12.2:

- **Step 1.12.1**: Automated solution setup using PowerShell
- **Step 1.12.2**: Calculator implementation with top-level statements

The application uses modern .NET practices including:
- Top-level statements for cleaner code
- Nullable reference types for better null safety
- TryParse for safe input conversion
- Switch expressions for operation selection

## Error Handling

The calculator handles various error scenarios:

| Scenario | Behavior |
|----------|----------|
| Division by zero | Displays error message, continues |
| Empty input | Displays error message, re-prompts |
| Invalid number format | Displays error message, re-prompts |
| Invalid operator | Displays error message, continues |
| Null input | Safely handled, no exceptions |

## Development

### Building from Source

```bash
# Clean previous builds
dotnet clean

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run in Debug mode
dotnet run --project calculator
```

### Testing

The solution includes an xUnit test project structure ready for comprehensive test coverage.

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with code coverage
dotnet test /p:CollectCoverage=true
```

## Requirements Met

### Functional Requirements
- ✅ FR-1: Prompts for two operands and operator
- ✅ FR-2: Addition operations
- ✅ FR-3: Subtraction operations
- ✅ FR-4: Multiplication operations
- ✅ FR-5: Division operations
- ✅ FR-8: Result display
- ✅ FR-9: Division by zero handling
- ✅ FR-10: Input validation
- ✅ FR-11: Multiple calculations
- ✅ FR-12: Exit confirmation
- ✅ FR-15: Null input handling

### Non-Functional Requirements
- ✅ Performance: Immediate feedback
- ✅ Usability: Clear, intuitive interface
- ✅ Reliability: Handles edge cases gracefully
- ✅ Maintainability: Clean, documented code
- ✅ Compatibility: .NET 8+ cross-platform

## Future Enhancements

The following features are defined in the PRD but not yet implemented:
- Refactoring operations into separate methods (Step 1.12.3)
- Comprehensive xUnit test coverage (Step 1.12.4)
- Modulo (%) operation
- Exponent (^) operation
- Screen clearing for better UX
- Calculation history

## License

This is a demonstration project for educational purposes.

## Author

Created as part of the GitHub Copilot Hackathon Custom project.

## Documentation

For detailed implementation verification, see [IMPLEMENTATION_VERIFICATION.md](./IMPLEMENTATION_VERIFICATION.md).
