# 1. Product Requirements Document (PRD): .NET Calculator with xUnit Testing

## 1.1 Document Information

- **Version:** 1.0
- **Author:** GitHub Copilot
- **Date:** November 3rd, 2025
- **Status:** Draft

## 1.2 Executive Summary

This document describes the requirements for developing a basic .NET 8 console calculator application with comprehensive xUnit testing. The calculator will support standard arithmetic operations and be designed with a focus on testability, maintainability, and proper error handling.

## 1.3 Problem Statement

Developers need a practical example of a well-structured .NET application that demonstrates:

- Basic calculator functionality with support for essential arithmetic operations
- Proper application structure with testable components
- Implementation of unit testing with xUnit framework
- Effective error handling and input validation

The solution will serve as both a functional calculator and a reference implementation for .NET best practices.

## 1.4 Goals and Objectives

- Create a fully functional calculator application using .NET 8 and C#
- Implement core arithmetic operations (+, -, *, /) using case expressions
- Demonstrate proper C# coding practices including null handling, error management, and code organization
- Refactor code from using case expressions to methods for better testability
- Demonstrate incremental development by adding modulo and exponent operations
- Provide comprehensive xUnit tests with different test approaches (fact, theory)
- Enable a clean, interactive console experience with proper user feedback
- Showcase proper solution structure with separate application and test projects

## 1.5 Scope

### 1.5.1 In Scope

- PowerShell script to set up the solution and project structure
- .NET 8 console application using top-level statements
- Calculator core arithmetic operations (addition, subtraction, multiplication, division, modulo, exponent)
- User input handling with proper validation
- xUnit test project with comprehensive test coverage
- Method refactoring for testability
- Proper error handling and null safety
- Documentation for both application code and tests

### 1.5.2 Out of Scope

- GUI implementation
- Advanced mathematical functions (logarithms, trigonometry, etc.)
- Persistence of calculations
- Scientific calculator features
- Multi-language support
- Calculator history feature

## 1.6 User Stories / Use Cases

- As a user, I want to perform basic arithmetic calculations with two operands so that I can quickly compute results
- As a user, I want clear prompts and feedback during calculation so I can understand how to use the application
- As a user, I want to perform multiple calculations in sequence so I don't have to restart the application
- As a user, I want proper error handling so invalid inputs don't crash the application
- As a developer, I want well-structured and tested code so that I can understand best practices in .NET development
- As a QA tester, I want comprehensive unit tests so I can verify the calculator functions correctly

## 1.7 Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | The application shall prompt users for two numeric operands and one operator |
| FR-2 | The application shall support addition (+) operations between two numbers |
| FR-3 | The application shall support subtraction (-) operations between two numbers |
| FR-4 | The application shall support multiplication (*) operations between two numbers |
| FR-5 | The application shall support division (/) operations between two numbers |
| FR-6 | The application shall support modulo (%) operations between two numbers |
| FR-7 | The application shall support exponent (^) operations between two numbers |
| FR-8 | The application shall display the result of each calculation to the user |
| FR-9 | The application shall handle division by zero errors gracefully |
| FR-10 | The application shall validate user input and provide feedback for invalid entries |
| FR-11 | The application shall allow users to perform additional calculations without restarting |
| FR-12 | The application shall confirm with the user before exiting |
| FR-13 | The application shall clear the screen between calculations for a clean interface |
| FR-14 | Each arithmetic operation shall be implemented as a separate, testable method |
| FR-15 | The application shall properly handle null inputs to prevent runtime exceptions |

## 1.8 Non-Functional Requirements

- **Performance:** The calculator shall provide immediate feedback for all operations
- **Usability:** The console interface shall be intuitive with clear instructions for users
- **Reliability:** The application shall handle edge cases and invalid inputs without crashing
- **Testability:** All core functionality shall be covered by unit tests
- **Maintainability:** Code shall follow C# best practices and be well-documented
- **Compatibility:** The application shall run on any platform supporting .NET 8

## 1.9 Assumptions and Dependencies

- Development will use .NET 8 SDK
- xUnit will be used as the testing framework
- Developers have access to appropriate .NET development tools
- The application will use standard C# console I/O features
- No external libraries beyond .NET Standard and xUnit are required

## 1.10 Success Criteria / KPIs

- All specified calculator operations work correctly
- Unit tests cover all arithmetic operations with multiple test cases
- The console application runs without crashing when given valid or invalid input
- All build and test commands execute successfully
- Code meets appropriate null safety standards for C#
- Documentation is clear and comprehensive

## 1.11 Milestones & Timeline

1. Solution Setup
   - Create PowerShell script for initializing solution structure
   - Set up the .NET 8 console application and xUnit test project

2. Initial Implementation
   - Implement basic calculator operations with top-level statements
   - Build and test the initial version

3. Refactoring
   - Convert operations to testable methods
   - Add proper error handling and screen clearing
   - Make methods public for testing accessibility

4. Testing
   - Create comprehensive xUnit tests for all operations
   - Implement both fact and theory test approaches
   - Ensure all tests pass

5. Final Improvements
   - Add modulo and exponent operations
   - Document the code
   - Fix any remaining issues or warnings

6. Documentation
   - Update documentation
   - Create setup and cleanup scripts

## 1.12 Implementation Guidance

### 1.12.1 Solution Setup

1. Create a PowerShell script named `Set-DotnetSlnForCalculator.ps1` in "$(git rev-parse --show-toplevel    demos\day1\04-coding-agent\workspace" that:
   - Creates a solution folder named `calculator-xunit-testing`
   - Sets up a new solution
   - Adds a console application project named `calculator`
   - Adds an xUnit test project named `calculator.tests`
   - Configures appropriate project references
   - Changes default file names from `Program.cs` to `Calculator.cs` and from `UnitTest1.cs` to `CalculatorTest.cs`
   - Execute the powershell script to create the solution structure

### 1.12.2 Calculator Implementation

1. Implement the calculator in `Calculator.cs` using top-level statements:
   - Prompt for first operand, second operand, and operator
   - Perform the appropriate arithmetic operation
   - Display the result
   - Ask if the user wants to perform another calculation
   - Handle user response to continue or exit

2. Fix null reference warnings using appropriate null handling techniques

### 1.12.3 Refactoring Steps

1. Convert each arithmetic operation into its own method for testability
2. Make methods public to allow testing from the xUnit project
3. Add screen clearing for a better user experience
4. Implement modulo and exponent operations
5. Add proper null handling to prevent exceptions

### 1.12.4 Testing Strategy

1. Create xUnit tests for each calculator operation
2. Use Fact tests for simple assertions
3. Use Theory tests with InlineData for testing multiple cases
4. Include test cases for edge conditions (like division by zero)
5. Implement tests for normal, edge, and error cases

### 1.12.5 Final Steps

1. Translate calculator code to Python for cross-language comparison
2. Write a cleanup script to reset the exercise
3. Complete documentation

## 1.13 Testing Requirements

- All arithmetic operations must have unit tests
- Each operation should have at least three test cases using Theory and InlineData
- Tests should include edge cases and failure scenarios
- All tests must pass before completion

## 1.14 Cleanup Procedure

Create a PowerShell script named `Remove-DotnetSlnForCalculator.ps1` to reset the exercise by:

1. Getting the repository root path
2. Appending the workspace path
3. Setting the current path to the target
4. Removing the calculator solution directory

## 1.15 Additional Learning Outcomes

- Understanding .NET solution and project structure
- Working with C# top-level statements
- Implementing proper null handling in C# code
- Writing effective xUnit tests with different techniques
- Creating reusable setup and cleanup scripts
- Translating C# concepts to other languages (Python)