# Day 2: .NET Development & MCP Integration

## Overview

Day 2 focuses on practical .NET development using GitHub Copilot with emphasis on:
- Agentic workflow patterns
- Code generation and optimization
- Refactoring and modernization
- Comprehensive testing with xUnit
- Debugging integration
- Model Context Protocol (MCP)

## Sessions

### 2.1: .NET Development with Agentic Mode
**Focus:** Building complete features with multi-turn agent workflows
- API design and CRUD operations
- Entity Framework integration
- Dependency injection patterns

### 2.2: Code Generation & Optimization
**Focus:** Generating common .NET patterns efficiently
- Repository pattern generation
- LINQ query optimization
- Async/await patterns
- Extension methods

### 2.3: Refactoring with GitHub Copilot
**Focus:** Modernizing legacy code and implementing patterns
- Dependency injection upgrades
- Design pattern implementation
- Performance improvements
- Code consolidation

### 2.4: Unit Testing & TDD
**Focus:** Test-driven development with xUnit
- Unit test generation
- Mock object creation
- Assertion patterns
- Test organization

### 2.5: Debugging & Documentation
**Focus:** IDE integration and documentation generation
- Breakpoint strategies
- Watch expressions
- Stack trace analysis
- Auto-generated comments and XML docs

### 2.6: MCP Overview
**Focus:** Model Context Protocol introduction
- MCP architecture
- Integration patterns
- Building custom MCP servers
- Use cases and benefits

## Demonstration Files

Each session has:
- `README.md` - Session overview and instructions
- Example code files with Copilot-assisted patterns
- Setup and execution scripts
- Output samples

## Running All Day 2 Demonstrations

```powershell
# Build all projects
Get-ChildItem . -Directory | ForEach-Object {
    dotnet build $_.FullName
}

# Run all tests
Get-ChildItem . -Recurse -Filter "*Tests.csproj" | ForEach-Object {
    dotnet test $_.FullName
}

# Run demonstrations
foreach ($dir in Get-ChildItem . -Directory) {
    if (Test-Path "$($dir.FullName)/bin/Debug/*/demo.exe") {
        & "$($dir.FullName)/bin/Debug/*/demo.exe"
    }
}
```

## Prerequisites

- .NET 6.0+ SDK
- Visual Studio 2022 or VS Code with C# extension
- xUnit testing framework
- GitHub Copilot extension
