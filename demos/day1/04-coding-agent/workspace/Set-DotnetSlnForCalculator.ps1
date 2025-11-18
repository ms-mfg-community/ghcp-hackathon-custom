<#
.SYNOPSIS
    Sets up a .NET solution with a console calculator application and xUnit test project.

.DESCRIPTION
    This script creates a complete .NET solution structure for a calculator application
    with comprehensive xUnit testing. It performs the following actions:
    - Creates a solution folder named 'calculator-xunit-testing'
    - Initializes a new .NET solution
    - Adds a console application project named 'calculator'
    - Adds an xUnit test project named 'calculator.tests'
    - Configures project references between the test project and the main application
    - Renames default files to follow naming conventions (Program.cs -> Calculator.cs, UnitTest1.cs -> CalculatorTest.cs)

.PARAMETER None
    This script does not accept parameters.

.EXAMPLE
    .\Set-DotnetSlnForCalculator.ps1
    
    Creates the calculator solution structure in the current directory.

.NOTES
    File Name      : Set-DotnetSlnForCalculator.ps1
    Author         : GitHub Copilot
    Prerequisite   : .NET 8 SDK must be installed
    Version        : 1.0
    
    Requirements:
    - .NET 8 SDK or later
    - PowerShell 5.1 or later (PowerShell Core 7+ recommended)
    - Write permissions in the target directory
    
    Change Log:
    - Version 1.0: Initial creation

.LINK
    https://docs.microsoft.com/en-us/dotnet/core/tools/

#>

# Get the repository root path
$repoRoot = git rev-parse --show-toplevel

# Define the workspace path
$workspacePath = "demos\day1\04-coding-agent\workspace"

# Combine paths to get the target directory
$targetPath = Join-Path -Path $repoRoot -ChildPath $workspacePath

# Set the current location to the target path
Set-Location -Path $targetPath

Write-Host "Setting up .NET Calculator solution..." -ForegroundColor Cyan
Write-Host "Target directory: $targetPath" -ForegroundColor Gray
Write-Host "-----------------------------" -ForegroundColor Gray

# Create solution folder
$solutionFolder = "calculator-xunit-testing"
Write-Host "Creating solution folder: $solutionFolder" -ForegroundColor Yellow

if (Test-Path $solutionFolder) 
{
    Write-Host "Solution folder already exists. Removing..." -ForegroundColor Yellow
    Remove-Item -Path $solutionFolder -Recurse -Force
} #end if

New-Item -ItemType Directory -Path $solutionFolder | Out-Null
Set-Location -Path $solutionFolder

# Create new solution
Write-Host "Creating new solution..." -ForegroundColor Yellow
dotnet new sln --name calculator

# Create console application project
Write-Host "Creating console application project: calculator" -ForegroundColor Yellow
dotnet new console --framework net8.0 --name calculator --output calculator

# Rename Program.cs to Calculator.cs
$programPath = Join-Path -Path "calculator" -ChildPath "Program.cs"
$calculatorPath = Join-Path -Path "calculator" -ChildPath "Calculator.cs"

if (Test-Path $programPath) 
{
    Write-Host "Renaming Program.cs to Calculator.cs" -ForegroundColor Yellow
    Move-Item -Path $programPath -Destination $calculatorPath -Force
} #end if

# Create xUnit test project
Write-Host "Creating xUnit test project: calculator.tests" -ForegroundColor Yellow
dotnet new xunit --framework net8.0 --name calculator.tests --output calculator.tests

# Rename UnitTest1.cs to CalculatorTest.cs
$unitTestPath = Join-Path -Path "calculator.tests" -ChildPath "UnitTest1.cs"
$calculatorTestPath = Join-Path -Path "calculator.tests" -ChildPath "CalculatorTest.cs"

if (Test-Path $unitTestPath) 
{
    Write-Host "Renaming UnitTest1.cs to CalculatorTest.cs" -ForegroundColor Yellow
    Move-Item -Path $unitTestPath -Destination $calculatorTestPath -Force
} #end if

# Add projects to solution
Write-Host "Adding projects to solution..." -ForegroundColor Yellow
dotnet sln add calculator/calculator.csproj
dotnet sln add calculator.tests/calculator.tests.csproj

# Add project reference from test project to main project
Write-Host "Adding project reference from test project to main project..." -ForegroundColor Yellow
dotnet add calculator.tests/calculator.tests.csproj reference calculator/calculator.csproj

# Build the solution to verify setup
Write-Host "Building solution to verify setup..." -ForegroundColor Yellow
dotnet build

Write-Host "-----------------------------" -ForegroundColor Gray
Write-Host "Solution setup complete!" -ForegroundColor Green
Write-Host "Solution location: $(Get-Location)" -ForegroundColor Gray
Write-Host "" -ForegroundColor Gray
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Navigate to the calculator project: cd calculator" -ForegroundColor Gray
Write-Host "2. Implement calculator logic in Calculator.cs" -ForegroundColor Gray
Write-Host "3. Create tests in calculator.tests/CalculatorTest.cs" -ForegroundColor Gray
Write-Host "4. Run tests with: dotnet test" -ForegroundColor Gray

# end Set-DotnetSlnForCalculator
