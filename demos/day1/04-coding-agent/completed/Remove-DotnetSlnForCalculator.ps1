<#
.SYNOPSIS
    Cleanup script to reset the calculator xUnit testing exercise

.DESCRIPTION
    This script removes the calculator-xunit-testing solution directory
    to reset the exercise to its initial state. It navigates to the
    workspace directory and removes all generated files and folders.

.EXAMPLE
    .\Remove-DotnetSlnForCalculator.ps1
    
    Removes the calculator-xunit-testing directory and all its contents.

.NOTES
    File Name      : Remove-DotnetSlnForCalculator.ps1
    Author         : GitHub Copilot
    Prerequisite   : PowerShell 5.1 or later
    Version        : 1.0
    
    Change Log:
    - Version 1.0: Initial creation
#>

# Get the repository root path
$repoRoot = git rev-parse --show-toplevel
if (-not $repoRoot) {
    Write-Error "Failed to get repository root. Are you in a git repository?"
    exit 1
} # end if

# Append the workspace path
$workspacePath = Join-Path -Path $repoRoot -ChildPath "demos\day1\04-coding-agent\workspace"

# Set the solution directory path
$solutionDir = Join-Path -Path $workspacePath -ChildPath "calculator-xunit-testing"

# Check if the directory exists
if (Test-Path -Path $solutionDir) {
    Write-Host "Removing calculator solution directory..." -ForegroundColor Yellow
    Write-Host "Path: $solutionDir" -ForegroundColor Cyan
    
    try {
        # Remove the directory and all its contents
        Remove-Item -Path $solutionDir -Recurse -Force
        Write-Host "Successfully removed the calculator solution directory." -ForegroundColor Green
    } # end try
    catch {
        Write-Error "Failed to remove directory: $_"
        exit 1
    } # end catch
} # end if
else {
    Write-Host "Calculator solution directory not found at: $solutionDir" -ForegroundColor Yellow
    Write-Host "Nothing to clean up." -ForegroundColor Cyan
} # end else

Write-Host "`nCleanup complete!" -ForegroundColor Green
