<#
.SYNOPSIS
    Executes xUnit tests for the Calculator solution with code coverage analysis.

.DESCRIPTION
    This script runs all xUnit tests in the Calculator.Tests project and collects
    code coverage metrics. It uses the dotnet test command with integrated coverage
    collection, providing detailed test results and coverage statistics. The script
    is designed for both local development and CI/CD pipeline integration.

.EXAMPLE
    .\run-tests.ps1
    
    Runs all unit tests and collects coverage data for the Calculator solution.

.EXAMPLE
    pwsh -File .\run-tests.ps1
    
    Executes tests using PowerShell Core for cross-platform compatibility.

.NOTES
    File Name      : run-tests.ps1
    Prerequisite   : .NET SDK, xUnit, and coverlet.collector package
    Version        : 1.0
    
    Requirements:
    - .NET SDK (version compatible with the solution)
    - xUnit test framework installed in test project
    - coverlet.collector NuGet package for coverage
    - PowerShell 5.1 or PowerShell Core 7.x+
    
    Coverage Output:
    - Coverage results are collected by coverlet.collector
    - Results are typically saved in TestResults folder
    - Multiple output formats can be configured (json, lcov, cobertura)
#>

# Display test execution start information with timestamp
Write-Host "Running xUnit tests with code coverage analysis..." -ForegroundColor Cyan
Write-Host "Start time: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Gray

# Execute all tests in the solution with the following configurations:
# --configuration Release: Uses optimized Release build for testing
# --no-build: Skips rebuilding to use existing binaries (faster execution)
# --collect:"XPlat Code Coverage": Enables cross-platform code coverage collection
# --verbosity normal: Provides balanced output detail (minimal, quiet, normal, detailed, diagnostic)
Write-Host "`nExecuting test suite..." -ForegroundColor Yellow
dotnet test --configuration Release --no-build --collect:"XPlat Code Coverage" --verbosity normal

# Evaluate test execution results by checking the exit code
if ($LASTEXITCODE -eq 0) 
{
    # All tests passed successfully
    Write-Host "`nAll tests passed successfully!" -ForegroundColor Green
    Write-Host "Coverage data has been collected and saved to the TestResults folder." -ForegroundColor Cyan
    
    # Provide guidance on viewing coverage results
    Write-Host "`nTo view coverage results:" -ForegroundColor Yellow
    Write-Host "  1. Check the TestResults folder for coverage.cobertura.xml" -ForegroundColor Gray
    Write-Host "  2. Use coverage visualization tools (e.g., ReportGenerator, VS Code extensions)" -ForegroundColor Gray
} # end if
else 
{
    # One or more tests failed or there was an execution error
    Write-Host "`nTest execution failed!" -ForegroundColor Red
    Write-Host "Exit code: $LASTEXITCODE" -ForegroundColor Red
    Write-Host "Please review the test output above for failure details." -ForegroundColor Yellow
    exit $LASTEXITCODE
} # end else

# Display completion timestamp
Write-Host "`nTest execution completed at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Gray
