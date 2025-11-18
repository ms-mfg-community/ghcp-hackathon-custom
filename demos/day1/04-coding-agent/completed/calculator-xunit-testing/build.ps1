<#
.SYNOPSIS
    Builds the Calculator solution with comprehensive logging and error handling.

.DESCRIPTION
    This script performs a complete build of the Calculator solution using dotnet CLI.
    It cleans previous build artifacts, restores NuGet packages, compiles the solution,
    and provides detailed feedback on the build process. The script is designed to be
    used in both development and CI/CD environments.

.EXAMPLE
    .\build.ps1
    
    Executes the build process for the Calculator solution in the current directory.

.EXAMPLE
    pwsh -File .\build.ps1
    
    Runs the build script using PowerShell Core for cross-platform compatibility.

.NOTES
    File Name      : build.ps1
    Prerequisite   : .NET SDK must be installed
    Version        : 1.0
    
    Requirements:
    - .NET SDK (version compatible with the solution)
    - PowerShell 5.1 or PowerShell Core 7.x+
    - Write permissions in the build directory
#>

# Display script start information with timestamp
Write-Host "Starting build process at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')..." -ForegroundColor Cyan

# Clean previous build artifacts to ensure a fresh build
# This removes bin/ and obj/ folders from all projects
Write-Host "Cleaning previous build artifacts..." -ForegroundColor Yellow
dotnet clean

# Restore NuGet package dependencies for the solution
# This downloads all required packages specified in project files
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# Build the entire solution in Release configuration
# Release configuration applies optimizations and excludes debug symbols
Write-Host "Building solution in Release configuration..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore

# Check if the build was successful by examining the exit code
if ($LASTEXITCODE -eq 0) 
{
    # Build succeeded - display success message
    Write-Host "Build completed successfully!" -ForegroundColor Green
} # end if
else 
{
    # Build failed - display error message and exit with error code
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
} # end else
