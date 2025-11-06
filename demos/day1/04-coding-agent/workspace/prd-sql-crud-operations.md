# Product Requirements Document (PRD): SQLite CRUD Operations Demo

## Document Information

- **Version:** 1.0
- **Author(s):** GitHub Copilot
- **Date:** November 5, 2025
- **Status:** Draft

## Executive Summary

This document defines the requirements for a demonstration project that showcases GitHub Copilot's ability to assist developers in implementing CRUD (Create, Read, Update, Delete) operations against the existing SQLite database in the calculator-xunit-testing project. The solution demonstrates best practices for database interactions, Entity Framework Core usage, error handling, and data validation using the existing `CalculatorTestCase` schema.

## Problem Statement

The calculator project already has a SQLite database (`calculator_tests.db`) with test data, but lacks comprehensive examples and utilities for performing CRUD operations beyond basic read operations. Developers need practical examples of how to manage test data programmatically, including adding new test cases, updating existing ones, and managing database state during development and testing.

## Goals and Objectives

- Demonstrate GitHub Copilot's capability to generate CRUD operations for existing SQLite database schema
- Showcase proper use of Entity Framework Core with SQLite
- Implement comprehensive CRUD operations on the `CalculatorTestCase` table
- Provide error handling and validation best practices
- Create utility tools for managing test data
- Include examples of bulk operations and data seeding
- Demonstrate testing strategies for database operations

## Scope

### In Scope

- CRUD operations for the existing `CalculatorTestCase` table
- Utility class for advanced data management beyond `TestDataHelper`
- Bulk insert/update/delete operations
- Data export and import capabilities (JSON, CSV)
- Database backup and restore utilities
- Test data generation helpers
- Unit tests for all CRUD operations
- CLI tool for database management
- Documentation and usage examples

### Out of Scope

- Modifying the existing `CalculatorTestCase` schema
- Adding new database tables
- Production database deployment
- Database migration to Azure SQL
- ORM alternatives (Dapper, ADO.NET)
- Performance optimization and indexing
- Multi-user concurrency handling
- Database replication or distributed scenarios

## User Stories / Use Cases

- As a developer, I want to add new test cases to the database programmatically
- As a QA engineer, I want to update test cases with new expected results
- As a developer, I want to deactivate obsolete test cases without deleting them
- As a team lead, I want to export test data for documentation purposes
- As a developer, I want to bulk import test cases from a CSV file
- As a tester, I want to reset the database to initial seed data
- As a developer, I want to query test cases with complex filters
- As a team member, I want to back up and restore the test database

## Functional Requirements

| Requirement ID | Description |
|---|---|
| FR-1 | The solution shall implement CREATE operations to add new CalculatorTestCase records |
| FR-2 | The solution shall implement READ operations with filtering, sorting, and pagination |
| FR-3 | The solution shall implement UPDATE operations for modifying existing test cases |
| FR-4 | The solution shall implement DELETE operations (soft delete by setting IsActive=false) |
| FR-5 | The solution shall validate data before insertion/update (required fields, valid operations) |
| FR-6 | The solution shall handle database exceptions with meaningful error messages |
| FR-7 | The solution shall support bulk operations (insert multiple, update multiple, delete multiple) |
| FR-8 | The solution shall provide data export functionality (JSON, CSV formats) |
| FR-9 | The solution shall provide data import functionality from JSON/CSV files |
| FR-10 | The solution shall include database backup and restore utilities |
| FR-11 | The solution shall generate random test data for testing purposes |
| FR-12 | The solution shall provide a CLI tool for database management tasks |
| FR-13 | All database operations shall use Entity Framework Core best practices |
| FR-14 | The solution shall include comprehensive unit tests with >85% coverage |

## Non-Functional Requirements

- **Performance:** CRUD operations should complete within 100ms for single records
- **Maintainability:** Code should follow existing project patterns and conventions
- **Testability:** All database operations must be unit testable with in-memory database
- **Usability:** CLI tool should have clear help text and examples
- **Reliability:** Database operations should be ACID-compliant
- **Compatibility:** Must work with existing `TestDataHelper` and `TestDataDbContext`

## Existing Database Schema

### CalculatorTestCase Table

The existing table structure (already implemented in `CalculatorTestCase.cs`):

```csharp
public class CalculatorTestCase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string TestName { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; }

    public double FirstOperand { get; set; }

    public double SecondOperand { get; set; }

    [Required]
    [MaxLength(20)]
    public string Operation { get; set; }

    public double ExpectedResult { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**Categories:** Addition, Subtraction, Multiplication, Division, EdgeCases

**Operations:** Add, Subtract, Multiply, Divide

**Current Record Count:** 26 seeded test cases

## Technical Requirements

### C# Implementation with Entity Framework Core

- Use existing `TestDataDbContext` as the base
- Create new `CrudOperationsManager` class for advanced operations
- Implement async/await for all database operations
- Use LINQ for query operations
- Include XML documentation comments for all public methods
- Use xUnit for testing with in-memory SQLite database
- Follow existing project structure in `calculator.tests/Data/` folder

### Required NuGet Packages (Already Installed)

- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Design`
- `xUnit`
- Additional: `CsvHelper` (for CSV export/import)
- Additional: `System.CommandLine` (for CLI tool)

## CRUD Operation Examples

### Create (Insert)

```csharp
/// <summary>
/// Add a new calculator test case to the database
/// </summary>
/// <param name="testCase">The test case to add</param>
/// <returns>The ID of the newly created test case</returns>
/// <exception cref="ArgumentNullException">Thrown when testCase is null</exception>
/// <exception cref="ValidationException">Thrown when validation fails</exception>
public async Task<int> AddTestCaseAsync(CalculatorTestCase testCase)
{
    // Validate required fields
    ValidateTestCase(testCase);
    
    // Add to database
    await _context.TestCases.AddAsync(testCase);
    await _context.SaveChangesAsync();
    
    return testCase.Id;
}

/// <summary>
/// Bulk insert multiple test cases
/// </summary>
/// <param name="testCases">Collection of test cases to add</param>
/// <returns>Number of test cases added</returns>
public async Task<int> BulkAddTestCasesAsync(IEnumerable<CalculatorTestCase> testCases)
{
    var validTestCases = testCases.Where(tc => IsValidTestCase(tc)).ToList();
    
    await _context.TestCases.AddRangeAsync(validTestCases);
    await _context.SaveChangesAsync();
    
    return validTestCases.Count;
}
```

### Read (Select)

```csharp
/// <summary>
/// Get a test case by ID
/// </summary>
/// <param name="id">The test case ID</param>
/// <returns>The test case or null if not found</returns>
public async Task<CalculatorTestCase?> GetTestCaseByIdAsync(int id)
{
    return await _context.TestCases
        .FirstOrDefaultAsync(tc => tc.Id == id);
}

/// <summary>
/// Get test cases with filtering, sorting, and pagination
/// </summary>
/// <param name="category">Optional category filter</param>
/// <param name="operation">Optional operation filter</param>
/// <param name="isActive">Optional active status filter</param>
/// <param name="pageNumber">Page number (1-based)</param>
/// <param name="pageSize">Number of records per page</param>
/// <param name="sortBy">Property name to sort by</param>
/// <param name="ascending">Sort order</param>
/// <returns>Paginated list of test cases</returns>
public async Task<PagedResult<CalculatorTestCase>> GetTestCasesAsync(
    string? category = null,
    string? operation = null,
    bool? isActive = null,
    int pageNumber = 1,
    int pageSize = 10,
    string sortBy = "Id",
    bool ascending = true)
{
    var query = _context.TestCases.AsQueryable();
    
    // Apply filters
    if (!string.IsNullOrEmpty(category))
        query = query.Where(tc => tc.Category == category);
    
    if (!string.IsNullOrEmpty(operation))
        query = query.Where(tc => tc.Operation == operation);
    
    if (isActive.HasValue)
        query = query.Where(tc => tc.IsActive == isActive.Value);
    
    // Get total count before pagination
    var totalCount = await query.CountAsync();
    
    // Apply sorting
    query = ApplySorting(query, sortBy, ascending);
    
    // Apply pagination
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    
    return new PagedResult<CalculatorTestCase>
    {
        Items = items,
        TotalCount = totalCount,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}

/// <summary>
/// Search test cases by description or test name
/// </summary>
/// <param name="searchTerm">Term to search for</param>
/// <returns>List of matching test cases</returns>
public async Task<List<CalculatorTestCase>> SearchTestCasesAsync(string searchTerm)
{
    return await _context.TestCases
        .Where(tc => tc.IsActive && 
               (tc.TestName.Contains(searchTerm) || 
                (tc.Description != null && tc.Description.Contains(searchTerm))))
        .ToListAsync();
}
```

### Update

```csharp
/// <summary>
/// Update an existing test case
/// </summary>
/// <param name="testCase">The test case with updated values</param>
/// <returns>True if updated, false if not found</returns>
/// <exception cref="ValidationException">Thrown when validation fails</exception>
public async Task<bool> UpdateTestCaseAsync(CalculatorTestCase testCase)
{
    var existing = await _context.TestCases.FindAsync(testCase.Id);
    
    if (existing == null)
        return false;
    
    // Validate updates
    ValidateTestCase(testCase);
    
    // Update properties
    existing.TestName = testCase.TestName;
    existing.Category = testCase.Category;
    existing.FirstOperand = testCase.FirstOperand;
    existing.SecondOperand = testCase.SecondOperand;
    existing.Operation = testCase.Operation;
    existing.ExpectedResult = testCase.ExpectedResult;
    existing.Description = testCase.Description;
    existing.IsActive = testCase.IsActive;
    
    await _context.SaveChangesAsync();
    
    return true;
}

/// <summary>
/// Update specific fields of a test case
/// </summary>
/// <param name="id">Test case ID</param>
/// <param name="updates">Dictionary of field names and new values</param>
/// <returns>True if updated, false if not found</returns>
public async Task<bool> PartialUpdateAsync(int id, Dictionary<string, object> updates)
{
    var testCase = await _context.TestCases.FindAsync(id);
    
    if (testCase == null)
        return false;
    
    foreach (var (key, value) in updates)
    {
        var property = typeof(CalculatorTestCase).GetProperty(key);
        if (property != null && property.CanWrite)
        {
            property.SetValue(testCase, value);
        }
    }
    
    await _context.SaveChangesAsync();
    
    return true;
}

/// <summary>
/// Bulk update test cases matching criteria
/// </summary>
/// <param name="filter">Filter expression</param>
/// <param name="updates">Action to apply to matching records</param>
/// <returns>Number of records updated</returns>
public async Task<int> BulkUpdateAsync(
    Expression<Func<CalculatorTestCase, bool>> filter,
    Action<CalculatorTestCase> updates)
{
    var testCases = await _context.TestCases.Where(filter).ToListAsync();
    
    foreach (var testCase in testCases)
    {
        updates(testCase);
    }
    
    await _context.SaveChangesAsync();
    
    return testCases.Count;
}
```

### Delete (Soft Delete)

```csharp
/// <summary>
/// Soft delete a test case by setting IsActive to false
/// </summary>
/// <param name="id">Test case ID</param>
/// <returns>True if deactivated, false if not found</returns>
public async Task<bool> DeactivateTestCaseAsync(int id)
{
    var testCase = await _context.TestCases.FindAsync(id);
    
    if (testCase == null)
        return false;
    
    testCase.IsActive = false;
    await _context.SaveChangesAsync();
    
    return true;
}

/// <summary>
/// Reactivate a previously deactivated test case
/// </summary>
/// <param name="id">Test case ID</param>
/// <returns>True if reactivated, false if not found</returns>
public async Task<bool> ReactivateTestCaseAsync(int id)
{
    var testCase = await _context.TestCases.FindAsync(id);
    
    if (testCase == null)
        return false;
    
    testCase.IsActive = true;
    await _context.SaveChangesAsync();
    
    return true;
}

/// <summary>
/// Permanently delete a test case from database (use with caution)
/// </summary>
/// <param name="id">Test case ID</param>
/// <returns>True if deleted, false if not found</returns>
public async Task<bool> HardDeleteTestCaseAsync(int id)
{
    var testCase = await _context.TestCases.FindAsync(id);
    
    if (testCase == null)
        return false;
    
    _context.TestCases.Remove(testCase);
    await _context.SaveChangesAsync();
    
    return true;
}

/// <summary>
/// Bulk deactivate test cases by category
/// </summary>
/// <param name="category">Category to deactivate</param>
/// <returns>Number of test cases deactivated</returns>
public async Task<int> BulkDeactivateByCategoryAsync(string category)
{
    return await BulkUpdateAsync(
        tc => tc.Category == category && tc.IsActive,
        tc => tc.IsActive = false
    );
}
```

## Data Import/Export Operations

### Export to JSON

```csharp
/// <summary>
/// Export test cases to JSON file
/// </summary>
/// <param name="filePath">Output file path</param>
/// <param name="includeInactive">Include inactive test cases</param>
/// <returns>Number of records exported</returns>
public async Task<int> ExportToJsonAsync(string filePath, bool includeInactive = false)
{
    var query = _context.TestCases.AsQueryable();
    
    if (!includeInactive)
        query = query.Where(tc => tc.IsActive);
    
    var testCases = await query.ToListAsync();
    
    var json = JsonSerializer.Serialize(testCases, new JsonSerializerOptions 
    { 
        WriteIndented = true 
    });
    
    await File.WriteAllTextAsync(filePath, json);
    
    return testCases.Count;
}
```

### Import from JSON

```csharp
/// <summary>
/// Import test cases from JSON file
/// </summary>
/// <param name="filePath">JSON file path</param>
/// <param name="replaceExisting">If true, clears existing data first</param>
/// <returns>Number of records imported</returns>
public async Task<int> ImportFromJsonAsync(string filePath, bool replaceExisting = false)
{
    var json = await File.ReadAllTextAsync(filePath);
    var testCases = JsonSerializer.Deserialize<List<CalculatorTestCase>>(json);
    
    if (testCases == null || !testCases.Any())
        return 0;
    
    if (replaceExisting)
    {
        _context.TestCases.RemoveRange(_context.TestCases);
        await _context.SaveChangesAsync();
    }
    
    // Reset IDs for new inserts
    foreach (var testCase in testCases)
    {
        testCase.Id = 0;
    }
    
    await _context.TestCases.AddRangeAsync(testCases);
    await _context.SaveChangesAsync();
    
    return testCases.Count;
}
```

### Export to CSV

```csharp
/// <summary>
/// Export test cases to CSV file
/// </summary>
/// <param name="filePath">Output CSV file path</param>
/// <param name="includeInactive">Include inactive test cases</param>
/// <returns>Number of records exported</returns>
public async Task<int> ExportToCsvAsync(string filePath, bool includeInactive = false)
{
    var query = _context.TestCases.AsQueryable();
    
    if (!includeInactive)
        query = query.Where(tc => tc.IsActive);
    
    var testCases = await query.ToListAsync();
    
    using var writer = new StreamWriter(filePath);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    
    await csv.WriteRecordsAsync(testCases);
    
    return testCases.Count;
}
```

## Testing Requirements

### Unit Test Coverage

All CRUD operations must have comprehensive unit tests:

```csharp
public class CrudOperationsManagerTests : IDisposable
{
    private readonly TestDataDbContext _context;
    private readonly CrudOperationsManager _manager;

    public CrudOperationsManagerTests()
    {
        // Use in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<TestDataDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new TestDataDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        
        _manager = new CrudOperationsManager(_context);
    }

    [Fact]
    public async Task AddTestCaseAsync_ValidTestCase_ReturnsId()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_Addition",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3,
            Description = "Simple addition test"
        };

        // Act
        var id = await _manager.AddTestCaseAsync(testCase);

        // Assert
        Assert.True(id > 0);
        var retrieved = await _manager.GetTestCaseByIdAsync(id);
        Assert.NotNull(retrieved);
        Assert.Equal("Test_Addition", retrieved.TestName);
    }

    [Fact]
    public async Task UpdateTestCaseAsync_ExistingTestCase_UpdatesSuccessfully()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Original_Name",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        // Act
        testCase.TestName = "Updated_Name";
        var result = await _manager.UpdateTestCaseAsync(testCase);

        // Assert
        Assert.True(result);
        var updated = await _manager.GetTestCaseByIdAsync(id);
        Assert.Equal("Updated_Name", updated!.TestName);
    }

    [Fact]
    public async Task DeactivateTestCaseAsync_ExistingTestCase_SetsIsActiveToFalse()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_To_Deactivate",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        // Act
        var result = await _manager.DeactivateTestCaseAsync(id);

        // Assert
        Assert.True(result);
        var deactivated = await _manager.GetTestCaseByIdAsync(id);
        Assert.False(deactivated!.IsActive);
    }

    // Additional tests for pagination, filtering, bulk operations, etc.

    public void Dispose()
    {
        _context.Database.CloseConnection();
        _context.Dispose();
    }
}
```

## CLI Tool Implementation

### Command Structure

```bash
# Add a new test case
calculator-data add --name "Test_Add" --category "Addition" --op1 5 --op2 3 --operation "Add" --result 8

# List test cases
calculator-data list --category "Addition" --page 1 --size 10

# Update a test case
calculator-data update --id 1 --name "Updated_Test_Name" --description "New description"

# Deactivate a test case
calculator-data deactivate --id 5

# Export data
calculator-data export --format json --output testcases.json

# Import data
calculator-data import --file testcases.json --replace

# Generate random test data
calculator-data generate --count 10 --category "EdgeCases"

# Reset database to seed data
calculator-data reset --confirm
```

## Validation Rules

### CalculatorTestCase Validation

```csharp
private void ValidateTestCase(CalculatorTestCase testCase)
{
    if (testCase == null)
        throw new ArgumentNullException(nameof(testCase));

    if (string.IsNullOrWhiteSpace(testCase.TestName))
        throw new ValidationException("TestName is required");

    if (testCase.TestName.Length > 100)
        throw new ValidationException("TestName cannot exceed 100 characters");

    if (string.IsNullOrWhiteSpace(testCase.Category))
        throw new ValidationException("Category is required");

    var validCategories = new[] { "Addition", "Subtraction", "Multiplication", "Division", "EdgeCases" };
    if (!validCategories.Contains(testCase.Category))
        throw new ValidationException($"Category must be one of: {string.Join(", ", validCategories)}");

    if (string.IsNullOrWhiteSpace(testCase.Operation))
        throw new ValidationException("Operation is required");

    var validOperations = new[] { "Add", "Subtract", "Multiply", "Divide" };
    if (!validOperations.Contains(testCase.Operation))
        throw new ValidationException($"Operation must be one of: {string.Join(", ", validOperations)}");

    if (testCase.Operation == "Divide" && testCase.SecondOperand == 0)
        throw new ValidationException("Cannot divide by zero");
}
```

## Assumptions and Dependencies

- SQLite database file `calculator_tests.db` exists in the test project
- Entity Framework Core 8.x is installed and configured
- Existing `TestDataDbContext` and `CalculatorTestCase` models remain unchanged
- .NET 8.0 runtime is available
- Developers have basic understanding of Entity Framework Core and LINQ

## Success Criteria / KPIs

- All CRUD operations implemented and working correctly
- Unit test coverage >85% for CRUD operations manager
- All tests pass successfully with in-memory database
- CLI tool functional with all major commands
- Export/import works with both JSON and CSV formats
- Database backup/restore utilities operational
- Documentation complete with usage examples
- No breaking changes to existing `TestDataHelper` functionality

## Milestones & Timeline

1. **Phase 1 - Core CRUD Operations (Day 1)**
   - Implement `CrudOperationsManager` class
   - Add Create, Read, Update, Delete methods
   - Write unit tests for basic operations

2. **Phase 2 - Advanced Operations (Day 2)**
   - Implement bulk operations
   - Add pagination and filtering
   - Implement search functionality
   - Write unit tests

3. **Phase 3 - Import/Export (Day 2)**
   - Implement JSON export/import
   - Implement CSV export/import
   - Add data generation utilities
   - Write unit tests

4. **Phase 4 - CLI Tool (Day 3)**
   - Implement CLI commands
   - Add help text and documentation
   - Test all CLI scenarios

5. **Phase 5 - Documentation & Demo (Day 3)**
   - Complete XML documentation
   - Create usage examples
   - Write demonstration scripts
   - Final testing and validation

## Usage Instructions (Demonstration Sequence)

### Prerequisites

1. Navigate to the calculator test project directory
2. Ensure the database file exists or will be created
3. Verify NuGet packages are restored

### Demonstration Steps

#### Step 1: Add New Test Cases

```csharp
// Create a new CRUD manager instance
using var context = new TestDataDbContext(options);
var manager = new CrudOperationsManager(context);

// Add a single test case
var newTest = new CalculatorTestCase
{
    TestName = "Add_VeryLargeNumbers",
    Category = "EdgeCases",
    FirstOperand = 999999999,
    SecondOperand = 1,
    Operation = "Add",
    ExpectedResult = 1000000000,
    Description = "Test addition with very large numbers"
};

var id = await manager.AddTestCaseAsync(newTest);
Console.WriteLine($"Created test case with ID: {id}");
```

#### Step 2: Query and Filter Test Cases

```csharp
// Get paginated test cases by category
var result = await manager.GetTestCasesAsync(
    category: "Addition",
    isActive: true,
    pageNumber: 1,
    pageSize: 5,
    sortBy: "CreatedAt",
    ascending: false
);

Console.WriteLine($"Found {result.TotalCount} addition test cases");
foreach (var testCase in result.Items)
{
    Console.WriteLine($"  - {testCase.TestName}: {testCase.FirstOperand} + {testCase.SecondOperand} = {testCase.ExpectedResult}");
}
```

#### Step 3: Update Test Cases

```csharp
// Update a test case
var testToUpdate = await manager.GetTestCaseByIdAsync(10);
if (testToUpdate != null)
{
    testToUpdate.Description = "Updated description with more details";
    var updated = await manager.UpdateTestCaseAsync(testToUpdate);
    Console.WriteLine($"Update successful: {updated}");
}

// Partial update
await manager.PartialUpdateAsync(15, new Dictionary<string, object>
{
    { "Description", "Partial update example" },
    { "IsActive", true }
});
```

#### Step 4: Bulk Operations

```csharp
// Bulk deactivate all edge case tests
var count = await manager.BulkDeactivateByCategoryAsync("EdgeCases");
Console.WriteLine($"Deactivated {count} edge case tests");

// Bulk insert new test cases
var newTests = new List<CalculatorTestCase>
{
    new() { TestName = "Test1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 },
    new() { TestName = "Test2", Category = "Subtraction", FirstOperand = 5, SecondOperand = 3, Operation = "Subtract", ExpectedResult = 2 },
};

var addedCount = await manager.BulkAddTestCasesAsync(newTests);
Console.WriteLine($"Added {addedCount} test cases");
```

#### Step 5: Export and Import Data

```csharp
// Export to JSON
var exportCount = await manager.ExportToJsonAsync("testcases_backup.json");
Console.WriteLine($"Exported {exportCount} test cases to JSON");

// Export to CSV
var csvCount = await manager.ExportToCsvAsync("testcases.csv");
Console.WriteLine($"Exported {csvCount} test cases to CSV");

// Import from JSON
var importCount = await manager.ImportFromJsonAsync("testcases_backup.json");
Console.WriteLine($"Imported {importCount} test cases from JSON");
```

#### Step 6: Using CLI Tool

```powershell
# Build the CLI tool
dotnet build calculator.cli

# Add a test case
dotnet run --project calculator.cli -- add --name "CLI_Test" --category "Addition" --op1 10 --op2 20 --operation "Add" --result 30

# List all test cases
dotnet run --project calculator.cli -- list

# Export data
dotnet run --project calculator.cli -- export --format json --output backup.json

# Import data
dotnet run --project calculator.cli -- import --file backup.json
```

#### Step 7: Run Unit Tests

```powershell
# Run all CRUD tests
dotnet test --filter "FullyQualifiedName~CrudOperationsManagerTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage" --filter "FullyQualifiedName~CrudOperationsManagerTests"
```

## Key Takeaways

- GitHub Copilot accelerates CRUD implementation with existing schemas
- Entity Framework Core provides excellent abstraction for SQLite operations
- Async/await pattern ensures scalable database operations
- Soft deletes preserve data history and enable recovery
- Bulk operations significantly improve performance for large datasets
- Export/import capabilities enable data portability and backup
- CLI tools enhance developer productivity for database management
- Comprehensive unit testing with in-memory database ensures reliability

## Security Considerations

1. **Input Validation:**
   - Validate all user input before database operations
   - Use parameterized queries (Entity Framework handles this automatically)
   - Enforce maximum lengths and required fields

2. **Data Integrity:**
   - Use transactions for multi-step operations
   - Implement proper error handling and rollback
   - Validate business rules before committing changes

3. **Access Control:**
   - Implement appropriate access restrictions in production
   - Log all data modification operations
   - Consider audit trails for sensitive operations

## Performance Considerations

- Use `AsNoTracking()` for read-only queries to improve performance
- Implement pagination for large result sets
- Use bulk operations instead of individual inserts/updates
- Consider adding indexes for frequently queried columns
- Use compiled queries for repeated query patterns
- Profile slow operations and optimize LINQ queries

## Extension Opportunities

- Add database versioning and migrations
- Implement full-text search capabilities
- Add data validation rules engine
- Create web API for remote database access
- Implement real-time notifications for data changes
- Add database analytics and reporting
- Create graphical UI for database management

## Questions or Feedback from Attendees

- Should we add support for other database providers (PostgreSQL, SQL Server)?
- Are there specific bulk operation scenarios to prioritize?
- Should we include database migration examples?

## Questions for Attendees

- What additional CRUD scenarios would be helpful?
- Should we include examples of complex LINQ queries?
- Is there interest in a web-based database management UI?

## Call to Action

- Review the PRD and provide feedback
- Suggest additional CRUD operations or utilities
- Test the implementations and report issues
- Contribute examples and use cases

## References

- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [System.CommandLine Documentation](https://learn.microsoft.com/en-us/dotnet/standard/commandline/)
- [CsvHelper Documentation](https://joshclose.github.io/CsvHelper/)
- [xUnit Testing Patterns](https://xunit.net/docs/getting-started/netcore/cmdline)
