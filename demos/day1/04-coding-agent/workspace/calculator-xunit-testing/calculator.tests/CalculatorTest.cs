namespace calculator.tests;

public class CalculatorOperationsTests
{
    #region Addition Tests
    
    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange
        double num1 = 5;
        double num2 = 3;
        double expected = 8;
        
        // Act
        double result = CalculatorOperations.Add(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(5, 3, 8)]
    [InlineData(10, 20, 30)]
    [InlineData(-5, -3, -8)]
    [InlineData(-5, 5, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(2.5, 3.5, 6.0)]
    public void Add_VariousInputs_ReturnsCorrectSum(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Add(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    #endregion
    
    #region Subtraction Tests
    
    [Fact]
    public void Subtract_TwoPositiveNumbers_ReturnsCorrectDifference()
    {
        // Arrange
        double num1 = 10;
        double num2 = 3;
        double expected = 7;
        
        // Act
        double result = CalculatorOperations.Subtract(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(10, 3, 7)]
    [InlineData(20, 10, 10)]
    [InlineData(5, 10, -5)]
    [InlineData(-5, -3, -2)]
    [InlineData(0, 5, -5)]
    [InlineData(5.5, 2.5, 3.0)]
    public void Subtract_VariousInputs_ReturnsCorrectDifference(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Subtract(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    #endregion
    
    #region Multiplication Tests
    
    [Fact]
    public void Multiply_TwoPositiveNumbers_ReturnsCorrectProduct()
    {
        // Arrange
        double num1 = 5;
        double num2 = 3;
        double expected = 15;
        
        // Act
        double result = CalculatorOperations.Multiply(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(5, 3, 15)]
    [InlineData(10, 2, 20)]
    [InlineData(-5, 3, -15)]
    [InlineData(-5, -3, 15)]
    [InlineData(0, 10, 0)]
    [InlineData(2.5, 4, 10.0)]
    public void Multiply_VariousInputs_ReturnsCorrectProduct(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Multiply(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    #endregion
    
    #region Division Tests
    
    [Fact]
    public void Divide_TwoPositiveNumbers_ReturnsCorrectQuotient()
    {
        // Arrange
        double num1 = 10;
        double num2 = 2;
        double expected = 5;
        
        // Act
        double result = CalculatorOperations.Divide(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(10, 2, 5)]
    [InlineData(20, 4, 5)]
    [InlineData(-10, 2, -5)]
    [InlineData(-10, -2, 5)]
    [InlineData(7, 2, 3.5)]
    [InlineData(0, 5, 0)]
    public void Divide_VariousInputs_ReturnsCorrectQuotient(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Divide(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double num1 = 10;
        double num2 = 0;
        
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => CalculatorOperations.Divide(num1, num2));
    }
    
    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 0)]
    [InlineData(-5, 0)]
    public void Divide_ByZero_ThrowsDivideByZeroExceptionForVariousNumerators(double num1, double num2)
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => CalculatorOperations.Divide(num1, num2));
    }
    
    #endregion
    
    #region Modulo Tests
    
    [Fact]
    public void Modulo_TwoPositiveNumbers_ReturnsCorrectRemainder()
    {
        // Arrange
        double num1 = 10;
        double num2 = 3;
        double expected = 1;
        
        // Act
        double result = CalculatorOperations.Modulo(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(10, 3, 1)]
    [InlineData(20, 6, 2)]
    [InlineData(15, 4, 3)]
    [InlineData(7, 7, 0)]
    [InlineData(5, 10, 5)]
    [InlineData(0, 5, 0)]
    public void Modulo_VariousInputs_ReturnsCorrectRemainder(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Modulo(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Modulo_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        double num1 = 10;
        double num2 = 0;
        
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => CalculatorOperations.Modulo(num1, num2));
    }
    
    [Theory]
    [InlineData(10, 0)]
    [InlineData(0, 0)]
    [InlineData(-5, 0)]
    public void Modulo_ByZero_ThrowsDivideByZeroExceptionForVariousNumbers(double num1, double num2)
    {
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => CalculatorOperations.Modulo(num1, num2));
    }
    
    #endregion
    
    #region Exponent Tests
    
    [Fact]
    public void Exponent_PositiveBaseAndExponent_ReturnsCorrectPower()
    {
        // Arrange
        double num1 = 2;
        double num2 = 3;
        double expected = 8;
        
        // Act
        double result = CalculatorOperations.Exponent(num1, num2);
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(2, 3, 8)]
    [InlineData(5, 2, 25)]
    [InlineData(10, 0, 1)]
    [InlineData(3, 4, 81)]
    [InlineData(2, -1, 0.5)]
    [InlineData(4, 0.5, 2)]
    public void Exponent_VariousInputs_ReturnsCorrectPower(double num1, double num2, double expected)
    {
        // Act
        double result = CalculatorOperations.Exponent(num1, num2);
        
        // Assert
        Assert.Equal(expected, result, precision: 10);
    }
    
    #endregion
}
