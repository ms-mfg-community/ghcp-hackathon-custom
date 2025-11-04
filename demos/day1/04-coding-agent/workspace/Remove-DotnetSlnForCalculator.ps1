<#
.SYNOPSIS
    Cleanup script for the calculator xUnit testing solution.

.DESCRIPTION
    Removes the calculator-xunit-testing solution directory and all its contents.
    This script is used to reset the workspace after completing the calculator exercise.

.EXAMPLE
    .\Remove-DotnetSlnForCalculator.ps1
    
    Removes the calculator-xunit-testing folder and all its contents.

.NOTES
    File Name      : Remove-DotnetSlnForCalculator.ps1
    Author         : GitHub Copilot
    Prerequisite   : PowerShell 5.1 or higher
    Version        : 1.0
    
    Requirements:
    - PowerShell 5.1 or higher
    - Write permissions in the workspace directory
    
    Change Log:
    - Version 1.0: Initial creation

.LINK
    https://github.com/ghcp-hackathon-custom
    
#>

# Get the script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# Define the solution directory path
$solutionDir = Join-Path $scriptPath "calculator-xunit-testing"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Calculator Solution Cleanup Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Check if the directory exists
if (Test-Path $solutionDir) {
    Write-Host "Found solution directory: $solutionDir" -ForegroundColor Yellow
    Write-Host ""
    
    # Confirm deletion
    $confirmation = Read-Host "Are you sure you want to delete this directory and all its contents? (yes/no)"
    
    if ($confirmation -eq 'yes') {
        try {
            Write-Host "Removing directory..." -ForegroundColor Yellow
            Remove-Item -Path $solutionDir -Recurse -Force -ErrorAction Stop
            Write-Host ""
            Write-Host "✓ Solution directory removed successfully!" -ForegroundColor Green
        }
        catch {
            Write-Host ""
            Write-Host "✗ Error removing directory: $_" -ForegroundColor Red
            Write-Host "You may need to close any open files or terminals in this directory." -ForegroundColor Yellow
            exit 1
        } # end catch
    }
    else {
        Write-Host ""
        Write-Host "Cleanup cancelled." -ForegroundColor Yellow
    } # end if
}
else {
    Write-Host "Directory not found: $solutionDir" -ForegroundColor Yellow
    Write-Host "Nothing to clean up." -ForegroundColor Yellow
} # end if

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Cleanup Complete" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
