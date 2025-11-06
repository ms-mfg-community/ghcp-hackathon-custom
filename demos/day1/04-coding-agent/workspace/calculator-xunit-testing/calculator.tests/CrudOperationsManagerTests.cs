using calculator.tests.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace calculator.tests;

/// <summary>
/// Unit tests for CrudOperationsManager
/// </summary>
[Collection("CrudOperationsTests")]
public class CrudOperationsManagerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly TestDataDbContext _context;
    private readonly CrudOperationsManager _manager;

    public CrudOperationsManagerTests()
    {
        // Create and open a connection for in-memory SQLite database
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Create DbContext options using the open connection
        var options = new DbContextOptionsBuilder<TestDataDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TestDataDbContext(options);
        _context.Database.EnsureCreated();

        // Clear the seeded data to start with a clean database for each test
        _context.TestCases.RemoveRange(_context.TestCases);
        _context.SaveChanges();

        _manager = new CrudOperationsManager(_context);
    }

    #region Create Tests

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
    public async Task AddTestCaseAsync_NullTestCase_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _manager.AddTestCaseAsync(null!));
    }

    [Fact]
    public async Task AddTestCaseAsync_InvalidCategory_ThrowsValidationException()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_Invalid",
            Category = "InvalidCategory",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _manager.AddTestCaseAsync(testCase));
    }

    [Fact]
    public async Task BulkAddTestCasesAsync_ValidTestCases_ReturnsCount()
    {
        // Arrange
        var testCases = new List<CalculatorTestCase>
        {
            new() { TestName = "Test1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 },
            new() { TestName = "Test2", Category = "Subtraction", FirstOperand = 5, SecondOperand = 3, Operation = "Subtract", ExpectedResult = 2 },
            new() { TestName = "Test3", Category = "Multiplication", FirstOperand = 2, SecondOperand = 3, Operation = "Multiply", ExpectedResult = 6 }
        };

        // Act
        var count = await _manager.BulkAddTestCasesAsync(testCases);

        // Assert
        Assert.Equal(3, count);
    }

    #endregion

    #region Read Tests

    [Fact]
    public async Task GetTestCaseByIdAsync_ExistingId_ReturnsTestCase()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_Multiply",
            Category = "Multiplication",
            FirstOperand = 5,
            SecondOperand = 4,
            Operation = "Multiply",
            ExpectedResult = 20
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        // Act
        var result = await _manager.GetTestCaseByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test_Multiply", result.TestName);
        Assert.Equal(20, result.ExpectedResult);
    }

    [Fact]
    public async Task GetTestCaseByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _manager.GetTestCaseByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTestCasesAsync_WithCategoryFilter_ReturnsFilteredResults()
    {
        // Arrange
        await _manager.BulkAddTestCasesAsync(new[]
        {
            new CalculatorTestCase { TestName = "Add1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 },
            new CalculatorTestCase { TestName = "Sub1", Category = "Subtraction", FirstOperand = 5, SecondOperand = 3, Operation = "Subtract", ExpectedResult = 2 },
            new CalculatorTestCase { TestName = "Add2", Category = "Addition", FirstOperand = 2, SecondOperand = 2, Operation = "Add", ExpectedResult = 4 }
        });

        // Act
        var result = await _manager.GetTestCasesAsync(category: "Addition");

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.All(result.Items, tc => Assert.Equal("Addition", tc.Category));
    }

    [Fact]
    public async Task GetTestCasesAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var testCases = Enumerable.Range(1, 15)
            .Select(i => new CalculatorTestCase
            {
                TestName = $"Test{i}",
                Category = "Addition",
                FirstOperand = i,
                SecondOperand = 1,
                Operation = "Add",
                ExpectedResult = i + 1
            });
        await _manager.BulkAddTestCasesAsync(testCases);

        // Act
        var page1 = await _manager.GetTestCasesAsync(pageNumber: 1, pageSize: 5);
        var page2 = await _manager.GetTestCasesAsync(pageNumber: 2, pageSize: 5);

        // Assert
        Assert.Equal(15, page1.TotalCount);
        Assert.Equal(5, page1.Items.Count);
        Assert.Equal(5, page2.Items.Count);
        Assert.True(page1.HasNextPage);
        Assert.False(page1.HasPreviousPage);
        Assert.True(page2.HasPreviousPage);
    }

    [Fact]
    public async Task SearchTestCasesAsync_MatchingTerm_ReturnsResults()
    {
        // Arrange
        await _manager.AddTestCaseAsync(new CalculatorTestCase
        {
            TestName = "Special_Addition_Test",
            Category = "Addition",
            FirstOperand = 10,
            SecondOperand = 20,
            Operation = "Add",
            ExpectedResult = 30,
            Description = "A special test case"
        });

        // Act
        var results = await _manager.SearchTestCasesAsync("Special");

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, tc => tc.TestName.Contains("Special"));
    }

    #endregion

    #region Update Tests

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
        testCase.Description = "Updated description";
        var result = await _manager.UpdateTestCaseAsync(testCase);

        // Assert
        Assert.True(result);
        var updated = await _manager.GetTestCaseByIdAsync(id);
        Assert.Equal("Updated_Name", updated!.TestName);
        Assert.Equal("Updated description", updated.Description);
    }

    [Fact]
    public async Task UpdateTestCaseAsync_NonExistingTestCase_ReturnsFalse()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            Id = 9999,
            TestName = "NonExisting",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3
        };

        // Act
        var result = await _manager.UpdateTestCaseAsync(testCase);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task PartialUpdateAsync_UpdatesSpecifiedFields()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Original",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3,
            Description = "Original description"
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        var updates = new Dictionary<string, object>
        {
            { "Description", "Partially updated" },
            { "IsActive", false }
        };

        // Act
        var result = await _manager.PartialUpdateAsync(id, updates);

        // Assert
        Assert.True(result);
        var updated = await _manager.GetTestCaseByIdAsync(id);
        Assert.Equal("Partially updated", updated!.Description);
        Assert.False(updated.IsActive);
        Assert.Equal("Original", updated.TestName); // Should remain unchanged
    }

    [Fact]
    public async Task BulkUpdateAsync_UpdatesMatchingRecords()
    {
        // Arrange
        await _manager.BulkAddTestCasesAsync(new[]
        {
            new CalculatorTestCase { TestName = "Add1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 },
            new CalculatorTestCase { TestName = "Add2", Category = "Addition", FirstOperand = 2, SecondOperand = 2, Operation = "Add", ExpectedResult = 4 },
            new CalculatorTestCase { TestName = "Sub1", Category = "Subtraction", FirstOperand = 5, SecondOperand = 3, Operation = "Subtract", ExpectedResult = 2 }
        });

        // Act
        var count = await _manager.BulkUpdateAsync(
            tc => tc.Category == "Addition",
            tc => tc.Description = "Bulk updated");

        // Assert
        Assert.Equal(2, count);
        var additionTests = await _manager.GetTestCasesAsync(category: "Addition");
        Assert.All(additionTests.Items, tc => Assert.Equal("Bulk updated", tc.Description));
    }

    #endregion

    #region Delete Tests

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

    [Fact]
    public async Task ReactivateTestCaseAsync_DeactivatedTestCase_SetsIsActiveToTrue()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_To_Reactivate",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3,
            IsActive = false
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        // Act
        var result = await _manager.ReactivateTestCaseAsync(id);

        // Assert
        Assert.True(result);
        var reactivated = await _manager.GetTestCaseByIdAsync(id);
        Assert.True(reactivated!.IsActive);
    }

    [Fact]
    public async Task HardDeleteTestCaseAsync_ExistingTestCase_RemovesFromDatabase()
    {
        // Arrange
        var testCase = new CalculatorTestCase
        {
            TestName = "Test_To_Delete",
            Category = "Addition",
            FirstOperand = 1,
            SecondOperand = 2,
            Operation = "Add",
            ExpectedResult = 3
        };
        var id = await _manager.AddTestCaseAsync(testCase);

        // Act
        var result = await _manager.HardDeleteTestCaseAsync(id);

        // Assert
        Assert.True(result);
        var deleted = await _manager.GetTestCaseByIdAsync(id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task BulkDeactivateByCategoryAsync_DeactivatesMatchingRecords()
    {
        // Arrange
        await _manager.BulkAddTestCasesAsync(new[]
        {
            new CalculatorTestCase { TestName = "Edge1", Category = "EdgeCases", FirstOperand = 999999, SecondOperand = 1, Operation = "Add", ExpectedResult = 1000000 },
            new CalculatorTestCase { TestName = "Edge2", Category = "EdgeCases", FirstOperand = 0.001, SecondOperand = 0.001, Operation = "Multiply", ExpectedResult = 0.000001 },
            new CalculatorTestCase { TestName = "Add1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 }
        });

        // Act
        var count = await _manager.BulkDeactivateByCategoryAsync("EdgeCases");

        // Assert
        Assert.Equal(2, count);
        var edgeCases = await _manager.GetTestCasesAsync(category: "EdgeCases", isActive: false);
        Assert.Equal(2, edgeCases.TotalCount);
    }

    #endregion

    #region Utility Tests

    [Fact]
    public async Task GetStatisticsAsync_ReturnsCorrectCounts()
    {
        // Arrange
        await _manager.BulkAddTestCasesAsync(new[]
        {
            new CalculatorTestCase { TestName = "Add1", Category = "Addition", FirstOperand = 1, SecondOperand = 1, Operation = "Add", ExpectedResult = 2 },
            new CalculatorTestCase { TestName = "Add2", Category = "Addition", FirstOperand = 2, SecondOperand = 2, Operation = "Add", ExpectedResult = 4, IsActive = false },
            new CalculatorTestCase { TestName = "Sub1", Category = "Subtraction", FirstOperand = 5, SecondOperand = 3, Operation = "Subtract", ExpectedResult = 2 }
        });

        // Act
        var stats = await _manager.GetStatisticsAsync();

        // Assert
        Assert.Equal(3, stats.TotalTestCases);
        Assert.Equal(2, stats.ActiveTestCases);
        Assert.Equal(1, stats.InactiveTestCases);
        Assert.Equal(2, stats.TestCasesByCategory["Addition"]);
        Assert.Equal(1, stats.TestCasesByCategory["Subtraction"]);
    }

    [Fact]
    public async Task GenerateRandomTestCasesAsync_CreatesSpecifiedCount()
    {
        // Act
        var count = await _manager.GenerateRandomTestCasesAsync(10, "EdgeCases");

        // Assert
        Assert.Equal(10, count);
        var generated = await _manager.GetTestCasesAsync(category: "EdgeCases");
        Assert.Equal(10, generated.TotalCount);
    }

    [Fact]
    public async Task ExportToJsonAsync_CreatesFileWithData()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        await _manager.AddTestCaseAsync(new CalculatorTestCase
        {
            TestName = "Export_Test",
            Category = "Addition",
            FirstOperand = 5,
            SecondOperand = 3,
            Operation = "Add",
            ExpectedResult = 8
        });

        try
        {
            // Act
            var count = await _manager.ExportToJsonAsync(tempFile);

            // Assert
            Assert.True(count > 0);
            Assert.True(File.Exists(tempFile));
            var fileContent = await File.ReadAllTextAsync(tempFile);
            Assert.Contains("Export_Test", fileContent);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task ImportFromJsonAsync_LoadsDataFromFile()
    {
        // Arrange
        var tempFile = Path.GetTempFileName();
        var testCases = new[]
        {
            new CalculatorTestCase
            {
                Id = 0,
                TestName = "Import_Test_1",
                Category = "Addition",
                FirstOperand = 10,
                SecondOperand = 20,
                Operation = "Add",
                ExpectedResult = 30
            }
        };

        var json = System.Text.Json.JsonSerializer.Serialize(testCases);
        await File.WriteAllTextAsync(tempFile, json);

        try
        {
            // Act
            var count = await _manager.ImportFromJsonAsync(tempFile);

            // Assert
            Assert.Equal(1, count);
            var imported = await _manager.SearchTestCasesAsync("Import_Test");
            Assert.NotEmpty(imported);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    #endregion

    public void Dispose()
    {
        _manager?.Dispose();
        _context?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
