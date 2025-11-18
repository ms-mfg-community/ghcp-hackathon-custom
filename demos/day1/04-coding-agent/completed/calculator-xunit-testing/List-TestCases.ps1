# List all test cases from the SQLite database

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$dbPath = Join-Path $scriptDir "calculator_tests.db"

# First, ensure database is created by loading the test helper assembly
Add-Type -Path "$scriptDir\calculator.tests\bin\Debug\net9.0\calculator.tests.dll"
Add-Type -Path "$scriptDir\calculator.tests\bin\Debug\net9.0\Microsoft.EntityFrameworkCore.dll"
Add-Type -Path "$scriptDir\calculator.tests\bin\Debug\net9.0\Microsoft.EntityFrameworkCore.Sqlite.dll"

# Create instance of TestDataHelper which will ensure database exists
$helper = New-Object calculator.tests.Data.TestDataHelper

# Get all test cases
$testCases = $helper.GetAllTestCases()

Write-Host "=" -NoNewline
Write-Host ("=" * 79)
Write-Host "ALL TEST CASES IN DATABASE" -ForegroundColor Cyan
Write-Host "=" -NoNewline
Write-Host ("=" * 79)
Write-Host ""

$count = 0
foreach ($tc in $testCases) {
    $count++
}

Write-Host "Total Test Cases: $count`n"

# Group by category
$grouped = $testCases | Group-Object -Property Category

foreach ($group in $grouped) {
    Write-Host "`n$($group.Name) ($($group.Count) tests)" -ForegroundColor Cyan
    Write-Host ("-" * 80)
    
    foreach ($tc in $group.Group) {
        Write-Host "`nID $($tc.Id): $($tc.TestName)" -ForegroundColor Yellow
        Write-Host "  Operation: $($tc.FirstOperand) $($tc.Operation) $($tc.SecondOperand) = $($tc.ExpectedResult)"
        if ($tc.Description) {
            Write-Host "  Description: $($tc.Description)"
        }
        Write-Host "  Created: $($tc.CreatedAt.ToString('yyyy-MM-dd HH:mm:ss'))"
        Write-Host "  Active: $($tc.IsActive)"
    }
}

Write-Host "`n$("=" * 80)"

# Cleanup
$helper.Dispose()
