using Microsoft.EntityFrameworkCore;

namespace calculator.tests.Data;

/// <summary>
/// Helper class for managing test database and retrieving test data
/// </summary>
public class TestDataHelper : IDisposable
{
    private readonly TestDataDbContext _context;
    private bool _disposed = false;

    public TestDataHelper()
    {
        var options = new DbContextOptionsBuilder<TestDataDbContext>()
            .UseSqlite("Data Source=calculator_tests.db")
            .Options;

        _context = new TestDataDbContext(options);

        // Ensure database is created and migrated
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// Get all test cases
    /// </summary>
    public IEnumerable<CalculatorTestCase> GetAllTestCases()
    {
        return _context.TestCases.Where(tc => tc.IsActive).ToList();
    }

    /// <summary>
    /// Get test cases by category
    /// </summary>
    public IEnumerable<CalculatorTestCase> GetTestCasesByCategory(string category)
    {
        return _context.TestCases
            .Where(tc => tc.IsActive && tc.Category == category)
            .ToList();
    }

    /// <summary>
    /// Get test cases by operation
    /// </summary>
    public IEnumerable<CalculatorTestCase> GetTestCasesByOperation(string operation)
    {
        return _context.TestCases
            .Where(tc => tc.IsActive && tc.Operation == operation)
            .ToList();
    }

    /// <summary>
    /// Get a specific test case by name
    /// </summary>
    public CalculatorTestCase? GetTestCaseByName(string testName)
    {
        return _context.TestCases
            .FirstOrDefault(tc => tc.IsActive && tc.TestName == testName);
    }

    /// <summary>
    /// Add a new test case
    /// </summary>
    public void AddTestCase(CalculatorTestCase testCase)
    {
        _context.TestCases.Add(testCase);
        _context.SaveChanges();
    }

    /// <summary>
    /// Get test data as object array for xUnit TheoryData
    /// </summary>
    public IEnumerable<object[]> GetTestDataAsObjectArray(string? category = null)
    {
        var query = _context.TestCases.Where(tc => tc.IsActive);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(tc => tc.Category == category);
        }

        return query.Select(tc => new object[]
        {
            tc.FirstOperand,
            tc.SecondOperand,
            tc.ExpectedResult
        }).ToList();
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    public IEnumerable<string> GetCategories()
    {
        return _context.TestCases
            .Where(tc => tc.IsActive)
            .Select(tc => tc.Category)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Reset database to initial seed data
    /// </summary>
    public void ResetDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
}
