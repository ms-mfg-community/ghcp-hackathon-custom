# Initialize and query the calculator test database using sqlite3

$dbPath = "calculator_tests.db"

Write-Host "`nInitializing database..." -ForegroundColor Cyan

# Create the database schema and seed data using sqlite3
$sql = @"
-- Drop table if exists
DROP TABLE IF EXISTS TestCases;

-- Create TestCases table
CREATE TABLE TestCases (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TestName TEXT NOT NULL,
    Category TEXT NOT NULL,
    FirstOperand REAL NOT NULL,
    SecondOperand REAL NOT NULL,
    Operation TEXT NOT NULL,
    ExpectedResult REAL NOT NULL,
    Description TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Insert seed data (26 test cases)
INSERT INTO TestCases (Id, TestName, Category, FirstOperand, SecondOperand, Operation, ExpectedResult, Description, IsActive) VALUES
(1, 'Add_TwoPositiveNumbers', 'Addition', 5, 3, 'Add', 8, 'Adding two positive integers', 1),
(2, 'Add_PositiveAndNegative', 'Addition', 10, -5, 'Add', 5, 'Adding positive and negative numbers', 1),
(3, 'Add_TwoNegativeNumbers', 'Addition', -7, -3, 'Add', -10, 'Adding two negative numbers', 1),
(4, 'Add_WithZero', 'Addition', 15, 0, 'Add', 15, 'Adding zero to a number', 1),
(5, 'Add_Decimals', 'Addition', 2.5, 3.7, 'Add', 6.2, 'Adding decimal numbers', 1),
(6, 'Subtract_LargerFromSmaller', 'Subtraction', 10, 3, 'Subtract', 7, 'Subtracting smaller from larger number', 1),
(7, 'Subtract_ResultNegative', 'Subtraction', 5, 10, 'Subtract', -5, 'Subtraction resulting in negative', 1),
(8, 'Subtract_WithZero', 'Subtraction', 20, 0, 'Subtract', 20, 'Subtracting zero', 1),
(9, 'Subtract_Decimals', 'Subtraction', 10.5, 3.2, 'Subtract', 7.3, 'Subtracting decimal numbers', 1),
(10, 'Multiply_TwoPositiveNumbers', 'Multiplication', 4, 5, 'Multiply', 20, 'Multiplying two positive numbers', 1),
(11, 'Multiply_ByZero', 'Multiplication', 100, 0, 'Multiply', 0, 'Multiplying by zero', 1),
(12, 'Multiply_NegativeNumbers', 'Multiplication', -3, -4, 'Multiply', 12, 'Multiplying two negative numbers', 1),
(13, 'Multiply_PositiveAndNegative', 'Multiplication', 6, -2, 'Multiply', -12, 'Multiplying positive and negative', 1),
(14, 'Multiply_Decimals', 'Multiplication', 2.5, 4, 'Multiply', 10, 'Multiplying with decimals', 1),
(15, 'Divide_EvenDivision', 'Division', 20, 4, 'Divide', 5, 'Even division', 1),
(16, 'Divide_WithRemainder', 'Division', 10, 3, 'Divide', 3.333333333333333, 'Division with remainder', 1),
(17, 'Divide_NegativeNumbers', 'Division', -15, -3, 'Divide', 5, 'Dividing two negative numbers', 1),
(18, 'Divide_PositiveByNegative', 'Division', 12, -4, 'Divide', -3, 'Dividing positive by negative', 1),
(19, 'Divide_Decimals', 'Division', 7.5, 2.5, 'Divide', 3, 'Dividing decimal numbers', 1),
(20, 'Add_LargeNumbers', 'EdgeCases', 999999, 1, 'Add', 1000000, 'Adding large numbers', 1),
(21, 'Multiply_VerySmallDecimals', 'EdgeCases', 0.001, 0.001, 'Multiply', 0.000001, 'Multiplying very small decimals', 1),
(22, 'Subtract_Decimals', 'Subtraction', 5.8, 2.3, 'Subtract', 3.5, 'Subtracting decimal numbers', 1),
(23, 'Multiply_Decimals', 'Multiplication', 3.5, 2.0, 'Multiply', 7.0, 'Multiplying decimal numbers', 1),
(24, 'Divide_Decimals', 'Division', 7.5, 2.5, 'Divide', 3.0, 'Dividing decimal numbers', 1),
(25, 'Add_MixedIntegerDecimal', 'Addition', 5.0, 3.7, 'Add', 8.7, 'Adding integer and decimal', 1),
(26, 'Multiply_DecimalByInteger', 'Multiplication', 4.5, 3.0, 'Multiply', 13.5, 'Multiplying decimal by integer', 1);
"@

# Write SQL to temp file and execute
$sqlFile = "temp_init.sql"
$sql | Out-File -FilePath $sqlFile -Encoding ASCII -NoNewline

Get-Content $sqlFile | sqlite3 $dbPath
Remove-Item $sqlFile

Write-Host "Database initialized successfully!`n" -ForegroundColor Green

# Now query all test cases with formatted output
Write-Host "=" -NoNewline; Write-Host ("=" * 99); Write-Host "ALL TEST CASES IN DATABASE" -ForegroundColor Cyan
Write-Host "=" -NoNewline; Write-Host ("=" * 99); Write-Host ""

sqlite3 $dbPath -header -column @"
SELECT 
    Id,
    substr(TestName, 1, 30) AS TestName,
    substr(Category, 1, 14) AS Category,
    Operation,
    printf('%.2f', FirstOperand) AS Op1,
    printf('%.2f', SecondOperand) AS Op2,
    printf('%.6g', ExpectedResult) AS Result,
    IsActive
FROM TestCases 
ORDER BY Category, Id;
"@

Write-Host "`n" -NoNewline
Write-Host "=" -NoNewline; Write-Host ("=" * 99)
Write-Host "Total: 26 test cases" -ForegroundColor Green
Write-Host "=" -NoNewline; Write-Host ("=" * 99)
