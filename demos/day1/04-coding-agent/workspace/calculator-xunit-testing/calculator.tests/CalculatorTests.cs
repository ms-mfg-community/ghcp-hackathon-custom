using System.Globalization;

namespace calculator.tests;

/// <summary>
/// Comprehensive xUnit tests for the Calculator application.
/// Demonstrates CSV-based data-driven testing using MemberData.
/// </summary>
public class CalculatorTests
{
    #region CSV Data Readers

    /// <summary>
    /// Reads CSV test data and returns it as IEnumerable for xUnit Theory tests.
    /// </summary>
    private static IEnumerable<object[]> ReadCsvTestData(string fileName)
    {
        var testDataPath = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);

        if (!File.Exists(testDataPath))
        {
            throw new FileNotFoundException($"Test data file not found: {testDataPath}");
        }

        var lines = File.ReadAllLines(testDataPath);

        // Skip header row
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');

            var firstNumber = double.Parse(values[0], CultureInfo.InvariantCulture);
            var secondNumber = double.Parse(values[1], CultureInfo.InvariantCulture);
            var expectedValue = double.Parse(values[3], CultureInfo.InvariantCulture);

            yield return new object[] { firstNumber, secondNumber, expectedValue };
        }
    }

    public static IEnumerable<object[]> AdditionTestData => ReadCsvTestData("AdditionTests.csv");
    public static IEnumerable<object[]> SubtractionTestData => ReadCsvTestData("SubtractionTests.csv");
    public static IEnumerable<object[]> MultiplicationTestData => ReadCsvTestData("MultiplicationTests.csv");
    public static IEnumerable<object[]> DivisionTestData => ReadCsvTestData("DivisionTests.csv");
    public static IEnumerable<object[]> ModuloTestData => ReadCsvTestData("ModuloTests.csv");
    public static IEnumerable<object[]> ExponentTestData => ReadCsvTestData("ExponentTests.csv");

    #endregion

    #region Addition Tests

    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double a = 5;
        double b = 3;
        double expected = 8;

        // Act
        double result = Calculator.Add(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(AdditionTestData))]
    public void Add_CsvDataSource_ReturnsCorrectSum(double a, double b, double expected)
    {
        // Act
        double result = Calculator.Add(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    #endregion

    #region Subtraction Tests

    [Fact]
    public void Subtract_TwoPositiveNumbers_ReturnsCorrectDifference()
    {
        // Arrange
        double a = 10;
        double b = 4;
        double expected = 6;

        // Act
        double result = Calculator.Subtract(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(SubtractionTestData))]
    public void Subtract_CsvDataSource_ReturnsCorrectDifference(double a, double b, double expected)
    {
        // Act
        double result = Calculator.Subtract(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    #endregion

    #region Multiplication Tests

    [Fact]
    public void Multiply_TwoPositiveNumbers_ReturnsCorrectProduct()
    {
        // Arrange
        double a = 6;
        double b = 7;
        double expected = 42;

        // Act
        double result = Calculator.Multiply(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(MultiplicationTestData))]
    public void Multiply_CsvDataSource_ReturnsCorrectProduct(double a, double b, double expected)
    {
        // Act
        double result = Calculator.Multiply(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    #endregion

    #region Division Tests

    [Fact]
    public void Divide_TwoPositiveNumbers_ReturnsCorrectQuotient()
    {
        // Arrange
        double a = 20;
        double b = 4;
        double expected = 5;

        // Act
        double result = Calculator.Divide(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(DivisionTestData))]
    public void Divide_CsvDataSource_ReturnsCorrectQuotient(double a, double b, double expected)
    {
        // Act
        double result = Calculator.Divide(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double a = 10;
        double b = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => Calculator.Divide(a, b));
    }

    #endregion

    #region Modulo Tests

    [Fact]
    public void Modulo_TwoPositiveNumbers_ReturnsCorrectRemainder()
    {
        // Arrange
        double a = 10;
        double b = 3;
        double expected = 1;

        // Act
        double result = Calculator.Modulo(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(ModuloTestData))]
    public void Modulo_CsvDataSource_ReturnsCorrectRemainder(double a, double b, double expected)
    {
        // Act
        double result = Calculator.Modulo(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Fact]
    public void Modulo_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double a = 10;
        double b = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => Calculator.Modulo(a, b));
    }

    #endregion

    #region Exponent Tests

    [Fact]
    public void Exponent_PositiveBase_ReturnsCorrectPower()
    {
        // Arrange
        double baseNumber = 2;
        double exponent = 3;
        double expected = 8;

        // Act
        double result = Calculator.Exponent(baseNumber, exponent);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(ExponentTestData))]
    public void Exponent_CsvDataSource_ReturnsCorrectPower(double baseNumber, double exponent, double expected)
    {
        // Act
        double result = Calculator.Exponent(baseNumber, exponent);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Fact]
    public void Exponent_ZeroToZeroPower_ReturnsOne()
    {
        // Arrange
        double baseNumber = 0;
        double exponent = 0;
        double expected = 1; // Math.Pow(0, 0) returns 1

        // Act
        double result = Calculator.Exponent(baseNumber, exponent);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void Add_VeryLargeNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double a = double.MaxValue / 2;
        double b = double.MaxValue / 2;
        double expected = double.MaxValue;

        // Act
        double result = Calculator.Add(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 1);
    }

    [Fact]
    public void Multiply_ByZero_ReturnsZero()
    {
        // Arrange
        double a = 12345.67;
        double b = 0;
        double expected = 0;

        // Act
        double result = Calculator.Multiply(a, b);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Subtract_SameNumber_ReturnsZero()
    {
        // Arrange
        double a = 42.42;
        double b = 42.42;
        double expected = 0;

        // Act
        double result = Calculator.Subtract(a, b);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    #endregion
}
