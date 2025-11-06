using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace calculator.tests.Data;

/// <summary>
/// Manager class for advanced CRUD operations on CalculatorTestCase entities
/// </summary>
public class CrudOperationsManager : IDisposable
{
    private readonly TestDataDbContext _context;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the CrudOperationsManager class
    /// </summary>
    /// <param name="context">The database context</param>
    public CrudOperationsManager(TestDataDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Create Operations

    /// <summary>
    /// Add a new calculator test case to the database
    /// </summary>
    /// <param name="testCase">The test case to add</param>
    /// <returns>The ID of the newly created test case</returns>
    /// <exception cref="ArgumentNullException">Thrown when testCase is null</exception>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
    public async Task<int> AddTestCaseAsync(CalculatorTestCase testCase)
    {
        ValidateTestCase(testCase);

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

    /// <summary>
    /// Bulk insert with transaction support
    /// </summary>
    /// <param name="testCases">Collection of test cases to add</param>
    /// <returns>Number of test cases added</returns>
    public async Task<int> BulkAddTestCasesWithTransactionAsync(IEnumerable<CalculatorTestCase> testCases)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var validTestCases = testCases.Where(tc => IsValidTestCase(tc)).ToList();

            await _context.TestCases.AddRangeAsync(validTestCases);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return validTestCases.Count;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    #endregion

    #region Read Operations

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

    /// <summary>
    /// Get all active test cases
    /// </summary>
    /// <returns>List of active test cases</returns>
    public async Task<List<CalculatorTestCase>> GetAllActiveTestCasesAsync()
    {
        return await _context.TestCases
            .Where(tc => tc.IsActive)
            .ToListAsync();
    }

    #endregion

    #region Update Operations

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

    #endregion

    #region Delete Operations

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

    #endregion

    #region Import/Export Operations

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

    #endregion

    #region Utility Methods

    /// <summary>
    /// Get statistics about test cases
    /// </summary>
    /// <returns>Statistics object containing counts and breakdowns</returns>
    public async Task<TestCaseStatistics> GetStatisticsAsync()
    {
        return new TestCaseStatistics
        {
            TotalTestCases = await _context.TestCases.CountAsync(),
            ActiveTestCases = await _context.TestCases.CountAsync(tc => tc.IsActive),
            InactiveTestCases = await _context.TestCases.CountAsync(tc => !tc.IsActive),
            TestCasesByCategory = await _context.TestCases
                .GroupBy(tc => tc.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Category, x => x.Count),
            TestCasesByOperation = await _context.TestCases
                .GroupBy(tc => tc.Operation)
                .Select(g => new { Operation = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Operation, x => x.Count)
        };
    }

    /// <summary>
    /// Generate random test cases for testing
    /// </summary>
    /// <param name="count">Number of test cases to generate</param>
    /// <param name="category">Category for generated test cases</param>
    /// <returns>Number of test cases generated</returns>
    public async Task<int> GenerateRandomTestCasesAsync(int count, string category = "EdgeCases")
    {
        var random = new Random();
        var operations = new[] { "Add", "Subtract", "Multiply", "Divide" };
        var testCases = new List<CalculatorTestCase>();

        for (int i = 0; i < count; i++)
        {
            var op1 = Math.Round(random.NextDouble() * 1000, 2);
            var op2 = Math.Round(random.NextDouble() * 1000, 2);
            var operation = operations[random.Next(operations.Length)];

            var expected = operation switch
            {
                "Add" => op1 + op2,
                "Subtract" => op1 - op2,
                "Multiply" => op1 * op2,
                "Divide" => op2 != 0 ? op1 / op2 : 0,
                _ => 0
            };

            testCases.Add(new CalculatorTestCase
            {
                TestName = $"Generated_{operation}_{i + 1}",
                Category = category,
                FirstOperand = op1,
                SecondOperand = op2,
                Operation = operation,
                ExpectedResult = expected,
                Description = $"Randomly generated test case for {operation}"
            });
        }

        return await BulkAddTestCasesAsync(testCases);
    }

    /// <summary>
    /// Reset database to initial seed data from DbContext
    /// </summary>
    public async Task ResetToSeedDataAsync()
    {
        // Delete all existing data
        _context.TestCases.RemoveRange(_context.TestCases);
        await _context.SaveChangesAsync();

        // Recreate database with seed data
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    #endregion

    #region Validation Helpers

    /// <summary>
    /// Validate test case and throw exception if invalid
    /// </summary>
    /// <param name="testCase">Test case to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when testCase is null</exception>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
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

    /// <summary>
    /// Validate test case without throwing exceptions
    /// </summary>
    /// <param name="testCase">Test case to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    private bool IsValidTestCase(CalculatorTestCase testCase)
    {
        try
        {
            ValidateTestCase(testCase);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Apply dynamic sorting to a queryable
    /// </summary>
    /// <param name="query">The queryable to sort</param>
    /// <param name="sortBy">Property name to sort by</param>
    /// <param name="ascending">Sort order</param>
    /// <returns>Sorted queryable</returns>
    private IQueryable<CalculatorTestCase> ApplySorting(
        IQueryable<CalculatorTestCase> query,
        string sortBy,
        bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "id" => ascending ? query.OrderBy(tc => tc.Id) : query.OrderByDescending(tc => tc.Id),
            "testname" => ascending ? query.OrderBy(tc => tc.TestName) : query.OrderByDescending(tc => tc.TestName),
            "category" => ascending ? query.OrderBy(tc => tc.Category) : query.OrderByDescending(tc => tc.Category),
            "operation" => ascending ? query.OrderBy(tc => tc.Operation) : query.OrderByDescending(tc => tc.Operation),
            "createdat" => ascending ? query.OrderBy(tc => tc.CreatedAt) : query.OrderByDescending(tc => tc.CreatedAt),
            _ => ascending ? query.OrderBy(tc => tc.Id) : query.OrderByDescending(tc => tc.Id)
        };
    }

    #endregion

    #region IDisposable

    /// <summary>
    /// Dispose of resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose of resources
    /// </summary>
    /// <param name="disposing">Whether to dispose managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
