# Set-DotnetSlnForCalculator.ps1
# Creates the calculator solution structure with console app and xUnit test project

# Get the repository root path
$repoRoot = git rev-parse --show-toplevel
if (-not $repoRoot) {
    Write-Error "Not in a git repository"
    exit 1
}

# Set the workspace path
$workspacePath = Join-Path $repoRoot "demos\day1\04-coding-agent\workspace"
$solutionFolder = Join-Path $workspacePath "calculator-xunit-testing"

# Create solution folder
Write-Host "Creating solution folder: $solutionFolder" -ForegroundColor Cyan
New-Item -ItemType Directory -Path $solutionFolder -Force | Out-Null

# Navigate to solution folder
Set-Location $solutionFolder

# Create new solution
Write-Host "Creating solution file..." -ForegroundColor Cyan
dotnet new sln -n calculator-xunit-testing

# Create console application
Write-Host "Creating console application project..." -ForegroundColor Cyan
dotnet new console -n calculator -f net9.0
Move-Item -Path "calculator\Program.cs" -Destination "calculator\Calculator.cs" -Force
dotnet sln add calculator\calculator.csproj

# Create xUnit test project
Write-Host "Creating xUnit test project..." -ForegroundColor Cyan
dotnet new xunit -n calculator.tests -f net9.0
Move-Item -Path "calculator.tests\UnitTest1.cs" -Destination "calculator.tests\CalculatorTests.cs" -Force
dotnet sln add calculator.tests\calculator.tests.csproj

# Add project reference from test project to calculator project
Write-Host "Adding project reference..." -ForegroundColor Cyan
dotnet add calculator.tests\calculator.tests.csproj reference calculator\calculator.csproj

# Build the solution to verify setup
Write-Host "Building solution..." -ForegroundColor Cyan
dotnet build

Write-Host "`n✅ Solution setup complete!" -ForegroundColor Green
Write-Host "Solution location: $solutionFolder" -ForegroundColor Yellow
Write-Host "`nNext steps:" -ForegroundColor Cyan
Write-Host "  1. Implement calculator logic in calculator\Calculator.cs"
Write-Host "  2. Add tests in calculator.tests\CalculatorTests.cs"
Write-Host "  3. Run tests with: dotnet test"
