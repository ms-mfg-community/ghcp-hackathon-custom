<#
.SYNOPSIS
    Sets up a .NET solution structure for a basic calculator application with xUnit testing.

.DESCRIPTION
    This script creates a complete .NET solution with:
    - A console application project for the calculator
    - An xUnit test project for unit testing
    - Proper project references
    - Renamed files (Program.cs -> Calculator.cs, UnitTest1.cs -> CalculatorTest.cs)

.EXAMPLE
    .\Set-DotnetSlnForCalculator.ps1
    
    Creates the calculator solution structure in the workspace directory.

.NOTES
    File Name      : Set-DotnetSlnForCalculator.ps1
    Author         : GitHub Copilot
    Prerequisite   : .NET 8 SDK or higher
    Version        : 1.0
    
    Requirements:
    - .NET SDK 8.0 or higher
    - PowerShell 7.0 or higher
    
    Change Log:
    - Version 1.0: Initial creation

#>

# Get the repository root path
$repoRoot = git rev-parse --show-toplevel
if (-not $repoRoot) {
    Write-Error "Failed to get repository root. Are you in a git repository?"
    exit 1
} #end if

# Define the workspace path
$workspacePath = Join-Path -Path $repoRoot -ChildPath "demos\day1\04-coding-agent\workspace"

# Create the solution folder
$solutionFolder = Join-Path -Path $workspacePath -ChildPath "calculator-xunit-testing"

Write-Host "Creating solution folder: $solutionFolder" -ForegroundColor Cyan

# Create the directory if it doesn't exist
if (-not (Test-Path $solutionFolder)) {
    New-Item -Path $solutionFolder -ItemType Directory -Force | Out-Null
    Write-Host "  ✓ Solution folder created" -ForegroundColor Green
} else {
    Write-Host "  ✓ Solution folder already exists" -ForegroundColor Yellow
} #end if

# Change to the solution folder
Set-Location $solutionFolder

Write-Host "`nSetting up .NET solution..." -ForegroundColor Cyan

# Create a new solution
Write-Host "  Creating new solution..." -ForegroundColor White
dotnet new sln --name calculator
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Solution created" -ForegroundColor Green
} else {
    Write-Error "Failed to create solution"
    exit 1
} #end if

# Create console application project
Write-Host "`n  Creating console application project..." -ForegroundColor White
dotnet new console --name calculator --framework net8.0
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Console application project created" -ForegroundColor Green
} else {
    Write-Error "Failed to create console application project"
    exit 1
} #end if

# Create xUnit test project
Write-Host "`n  Creating xUnit test project..." -ForegroundColor White
dotnet new xunit --name calculator.tests --framework net8.0
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ xUnit test project created" -ForegroundColor Green
} else {
    Write-Error "Failed to create xUnit test project"
    exit 1
} #end if

# Add projects to solution
Write-Host "`n  Adding projects to solution..." -ForegroundColor White
dotnet sln add calculator/calculator.csproj
dotnet sln add calculator.tests/calculator.tests.csproj
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Projects added to solution" -ForegroundColor Green
} else {
    Write-Error "Failed to add projects to solution"
    exit 1
} #end if

# Add project reference from test project to calculator project
Write-Host "`n  Adding project reference..." -ForegroundColor White
dotnet add calculator.tests/calculator.tests.csproj reference calculator/calculator.csproj
if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✓ Project reference added" -ForegroundColor Green
} else {
    Write-Error "Failed to add project reference"
    exit 1
} #end if

# Rename Program.cs to Calculator.cs
Write-Host "`n  Renaming files..." -ForegroundColor White
$programFile = Join-Path -Path $solutionFolder -ChildPath "calculator\Program.cs"
$calculatorFile = Join-Path -Path $solutionFolder -ChildPath "calculator\Calculator.cs"

if (Test-Path $programFile) {
    Rename-Item -Path $programFile -NewName "Calculator.cs" -Force
    Write-Host "  ✓ Renamed Program.cs to Calculator.cs" -ForegroundColor Green
} else {
    Write-Host "  ⚠ Program.cs not found, skipping rename" -ForegroundColor Yellow
} #end if

# Rename UnitTest1.cs to CalculatorTest.cs
$unitTestFile = Join-Path -Path $solutionFolder -ChildPath "calculator.tests\UnitTest1.cs"
$calculatorTestFile = Join-Path -Path $solutionFolder -ChildPath "calculator.tests\CalculatorTest.cs"

if (Test-Path $unitTestFile) {
    Rename-Item -Path $unitTestFile -NewName "CalculatorTest.cs" -Force
    Write-Host "  ✓ Renamed UnitTest1.cs to CalculatorTest.cs" -ForegroundColor Green
} else {
    Write-Host "  ⚠ UnitTest1.cs not found, skipping rename" -ForegroundColor Yellow
} #end if

Write-Host "`n✓ Solution setup complete!" -ForegroundColor Green
Write-Host "`nSolution location: $solutionFolder" -ForegroundColor Cyan
Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "  1. Navigate to: $solutionFolder" -ForegroundColor White
Write-Host "  2. Implement calculator logic in: calculator\Calculator.cs" -ForegroundColor White
Write-Host "  3. Build the solution: dotnet build" -ForegroundColor White
Write-Host "  4. Run the application: dotnet run --project calculator" -ForegroundColor White
Write-Host "  5. Run tests: dotnet test" -ForegroundColor White
