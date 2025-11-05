using calculator.tests.Data;

namespace calculator.tests;

/// <summary>
/// Calculator tests using SQLite database test data
/// </summary>
public class CalculatorDatabaseTests : IDisposable
{
    private readonly TestDataHelper _testDataHelper;

    public CalculatorDatabaseTests()
    {
        _testDataHelper = new TestDataHelper();
    }

    #region Database-Driven Addition Tests

    [Fact]
    public void Add_DatabaseTestData_AllAdditionTestsPass()
    {
        // Arrange
        var additionTests = _testDataHelper.GetTestCasesByOperation("Add");

        // Act & Assert
        foreach (var testCase in additionTests)
        {
            double result = CalculatorOperations.Add(testCase.FirstOperand, testCase.SecondOperand);

            Assert.Equal(testCase.ExpectedResult, result, precision: 10);
        }
    }

    [Theory]
    [MemberData(nameof(GetAdditionTestData))]
    public void Add_FromDatabase_ReturnsExpectedResult(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Add(num1, num2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    public static IEnumerable<object[]> GetAdditionTestData()
    {
        using var helper = new TestDataHelper();
        return helper.GetTestDataAsObjectArray("Addition");
    }

    #endregion

    #region Database-Driven Subtraction Tests

    [Fact]
    public void Subtract_DatabaseTestData_AllSubtractionTestsPass()
    {
        // Arrange
        var subtractionTests = _testDataHelper.GetTestCasesByOperation("Subtract");

        // Act & Assert
        foreach (var testCase in subtractionTests)
        {
            double result = CalculatorOperations.Subtract(testCase.FirstOperand, testCase.SecondOperand);

            Assert.Equal(testCase.ExpectedResult, result, precision: 10);
        }
    }

    [Theory]
    [MemberData(nameof(GetSubtractionTestData))]
    public void Subtract_FromDatabase_ReturnsExpectedResult(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Subtract(num1, num2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    public static IEnumerable<object[]> GetSubtractionTestData()
    {
        using var helper = new TestDataHelper();
        return helper.GetTestDataAsObjectArray("Subtraction");
    }

    #endregion

    #region Database-Driven Multiplication Tests

    [Fact]
    public void Multiply_DatabaseTestData_AllMultiplicationTestsPass()
    {
        // Arrange
        var multiplicationTests = _testDataHelper.GetTestCasesByOperation("Multiply");

        // Act & Assert
        foreach (var testCase in multiplicationTests)
        {
            double result = CalculatorOperations.Multiply(testCase.FirstOperand, testCase.SecondOperand);

            Assert.Equal(testCase.ExpectedResult, result, precision: 10);
        }
    }

    [Theory]
    [MemberData(nameof(GetMultiplicationTestData))]
    public void Multiply_FromDatabase_ReturnsExpectedResult(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Multiply(num1, num2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    public static IEnumerable<object[]> GetMultiplicationTestData()
    {
        using var helper = new TestDataHelper();
        return helper.GetTestDataAsObjectArray("Multiplication");
    }

    #endregion

    #region Database-Driven Division Tests

    [Fact]
    public void Divide_DatabaseTestData_AllDivisionTestsPass()
    {
        // Arrange
        var divisionTests = _testDataHelper.GetTestCasesByOperation("Divide");

        // Act & Assert
        foreach (var testCase in divisionTests)
        {
            double result = CalculatorOperations.Divide(testCase.FirstOperand, testCase.SecondOperand);

            Assert.Equal(testCase.ExpectedResult, result, precision: 10);
        }
    }

    [Theory]
    [MemberData(nameof(GetDivisionTestData))]
    public void Divide_FromDatabase_ReturnsExpectedResult(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Divide(num1, num2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    public static IEnumerable<object[]> GetDivisionTestData()
    {
        using var helper = new TestDataHelper();
        return helper.GetTestDataAsObjectArray("Division");
    }

    #endregion

    #region Category-Based Tests

    [Fact]
    public void AllDatabaseTests_ExecuteSuccessfully()
    {
        // Arrange
        var allTests = _testDataHelper.GetAllTestCases();
        int totalTests = 0;
        int passedTests = 0;

        // Act & Assert
        foreach (var testCase in allTests)
        {
            totalTests++;
            double result = 0;

            try
            {
                result = testCase.Operation switch
                {
                    "Add" => CalculatorOperations.Add(testCase.FirstOperand, testCase.SecondOperand),
                    "Subtract" => CalculatorOperations.Subtract(testCase.FirstOperand, testCase.SecondOperand),
                    "Multiply" => CalculatorOperations.Multiply(testCase.FirstOperand, testCase.SecondOperand),
                    "Divide" => CalculatorOperations.Divide(testCase.FirstOperand, testCase.SecondOperand),
                    _ => throw new InvalidOperationException($"Unknown operation: {testCase.Operation}")
                };

                if (Math.Abs(result - testCase.ExpectedResult) < 0.0000000001)
                {
                    passedTests++;
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test '{testCase.TestName}' failed: {ex.Message}");
            }
        }

        // Assert all tests passed
        Assert.Equal(totalTests, passedTests);
    }

    [Fact]
    public void Database_ContainsExpectedNumberOfTestCases()
    {
        // Arrange & Act
        var allTests = _testDataHelper.GetAllTestCases().ToList();

        // Assert
        Assert.True(allTests.Count >= 21, $"Expected at least 21 test cases, but found {allTests.Count}");
    }

    [Fact]
    public void Database_ContainsAllCategories()
    {
        // Arrange
        var expectedCategories = new[] { "Addition", "Subtraction", "Multiplication", "Division", "EdgeCases" };

        // Act
        var actualCategories = _testDataHelper.GetCategories().ToList();

        // Assert
        foreach (var expectedCategory in expectedCategories)
        {
            Assert.Contains(expectedCategory, actualCategories);
        }
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void EdgeCases_FromDatabase_ExecuteCorrectly()
    {
        // Arrange
        var edgeCaseTests = _testDataHelper.GetTestCasesByCategory("EdgeCases");

        // Act & Assert
        foreach (var testCase in edgeCaseTests)
        {
            double result = testCase.Operation switch
            {
                "Add" => CalculatorOperations.Add(testCase.FirstOperand, testCase.SecondOperand),
                "Subtract" => CalculatorOperations.Subtract(testCase.FirstOperand, testCase.SecondOperand),
                "Multiply" => CalculatorOperations.Multiply(testCase.FirstOperand, testCase.SecondOperand),
                "Divide" => CalculatorOperations.Divide(testCase.FirstOperand, testCase.SecondOperand),
                _ => throw new InvalidOperationException($"Unknown operation: {testCase.Operation}")
            };

            Assert.Equal(testCase.ExpectedResult, result, precision: 10);
        }
    }

    #endregion

    public void Dispose()
    {
        _testDataHelper?.Dispose();
    }
}
