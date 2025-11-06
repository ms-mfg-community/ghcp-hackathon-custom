using Microsoft.EntityFrameworkCore;

namespace calculator.tests.Data;

/// <summary>
/// Database context for managing calculator test data
/// </summary>
public class TestDataDbContext : DbContext
{
    public DbSet<CalculatorTestCase> TestCases { get; set; } = null!;

    public TestDataDbContext(DbContextOptions<TestDataDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial test data
        modelBuilder.Entity<CalculatorTestCase>().HasData(
            // Addition tests
            new CalculatorTestCase
            {
                Id = 1,
                TestName = "Add_TwoPositiveNumbers",
                Category = "Addition",
                FirstOperand = 5,
                SecondOperand = 3,
                Operation = "Add",
                ExpectedResult = 8,
                Description = "Adding two positive integers"
            },
            new CalculatorTestCase
            {
                Id = 2,
                TestName = "Add_PositiveAndNegative",
                Category = "Addition",
                FirstOperand = 10,
                SecondOperand = -5,
                Operation = "Add",
                ExpectedResult = 5,
                Description = "Adding positive and negative numbers"
            },
            new CalculatorTestCase
            {
                Id = 3,
                TestName = "Add_TwoNegativeNumbers",
                Category = "Addition",
                FirstOperand = -7,
                SecondOperand = -3,
                Operation = "Add",
                ExpectedResult = -10,
                Description = "Adding two negative numbers"
            },
            new CalculatorTestCase
            {
                Id = 4,
                TestName = "Add_WithZero",
                Category = "Addition",
                FirstOperand = 15,
                SecondOperand = 0,
                Operation = "Add",
                ExpectedResult = 15,
                Description = "Adding zero to a number"
            },
            new CalculatorTestCase
            {
                Id = 5,
                TestName = "Add_Decimals",
                Category = "Addition",
                FirstOperand = 2.5,
                SecondOperand = 3.7,
                Operation = "Add",
                ExpectedResult = 6.2,
                Description = "Adding decimal numbers"
            },

            // Subtraction tests
            new CalculatorTestCase
            {
                Id = 6,
                TestName = "Subtract_LargerFromSmaller",
                Category = "Subtraction",
                FirstOperand = 10,
                SecondOperand = 3,
                Operation = "Subtract",
                ExpectedResult = 7,
                Description = "Subtracting smaller from larger number"
            },
            new CalculatorTestCase
            {
                Id = 7,
                TestName = "Subtract_ResultNegative",
                Category = "Subtraction",
                FirstOperand = 5,
                SecondOperand = 10,
                Operation = "Subtract",
                ExpectedResult = -5,
                Description = "Subtraction resulting in negative"
            },
            new CalculatorTestCase
            {
                Id = 8,
                TestName = "Subtract_WithZero",
                Category = "Subtraction",
                FirstOperand = 20,
                SecondOperand = 0,
                Operation = "Subtract",
                ExpectedResult = 20,
                Description = "Subtracting zero"
            },
            new CalculatorTestCase
            {
                Id = 9,
                TestName = "Subtract_Decimals",
                Category = "Subtraction",
                FirstOperand = 10.5,
                SecondOperand = 3.2,
                Operation = "Subtract",
                ExpectedResult = 7.3,
                Description = "Subtracting decimal numbers"
            },

            // Multiplication tests
            new CalculatorTestCase
            {
                Id = 10,
                TestName = "Multiply_TwoPositiveNumbers",
                Category = "Multiplication",
                FirstOperand = 4,
                SecondOperand = 5,
                Operation = "Multiply",
                ExpectedResult = 20,
                Description = "Multiplying two positive numbers"
            },
            new CalculatorTestCase
            {
                Id = 11,
                TestName = "Multiply_ByZero",
                Category = "Multiplication",
                FirstOperand = 100,
                SecondOperand = 0,
                Operation = "Multiply",
                ExpectedResult = 0,
                Description = "Multiplying by zero"
            },
            new CalculatorTestCase
            {
                Id = 12,
                TestName = "Multiply_NegativeNumbers",
                Category = "Multiplication",
                FirstOperand = -3,
                SecondOperand = -4,
                Operation = "Multiply",
                ExpectedResult = 12,
                Description = "Multiplying two negative numbers"
            },
            new CalculatorTestCase
            {
                Id = 13,
                TestName = "Multiply_PositiveAndNegative",
                Category = "Multiplication",
                FirstOperand = 6,
                SecondOperand = -2,
                Operation = "Multiply",
                ExpectedResult = -12,
                Description = "Multiplying positive and negative"
            },
            new CalculatorTestCase
            {
                Id = 14,
                TestName = "Multiply_Decimals",
                Category = "Multiplication",
                FirstOperand = 2.5,
                SecondOperand = 4,
                Operation = "Multiply",
                ExpectedResult = 10,
                Description = "Multiplying with decimals"
            },

            // Division tests
            new CalculatorTestCase
            {
                Id = 15,
                TestName = "Divide_EvenDivision",
                Category = "Division",
                FirstOperand = 20,
                SecondOperand = 4,
                Operation = "Divide",
                ExpectedResult = 5,
                Description = "Even division"
            },
            new CalculatorTestCase
            {
                Id = 16,
                TestName = "Divide_WithRemainder",
                Category = "Division",
                FirstOperand = 10,
                SecondOperand = 3,
                Operation = "Divide",
                ExpectedResult = 3.333333333333333,
                Description = "Division with remainder"
            },
            new CalculatorTestCase
            {
                Id = 17,
                TestName = "Divide_NegativeNumbers",
                Category = "Division",
                FirstOperand = -15,
                SecondOperand = -3,
                Operation = "Divide",
                ExpectedResult = 5,
                Description = "Dividing two negative numbers"
            },
            new CalculatorTestCase
            {
                Id = 18,
                TestName = "Divide_PositiveByNegative",
                Category = "Division",
                FirstOperand = 12,
                SecondOperand = -4,
                Operation = "Divide",
                ExpectedResult = -3,
                Description = "Dividing positive by negative"
            },
            new CalculatorTestCase
            {
                Id = 19,
                TestName = "Divide_Decimals",
                Category = "Division",
                FirstOperand = 7.5,
                SecondOperand = 2.5,
                Operation = "Divide",
                ExpectedResult = 3,
                Description = "Dividing decimal numbers"
            },

            // Edge cases
            new CalculatorTestCase
            {
                Id = 20,
                TestName = "Add_LargeNumbers",
                Category = "EdgeCases",
                FirstOperand = 999999,
                SecondOperand = 1,
                Operation = "Add",
                ExpectedResult = 1000000,
                Description = "Adding large numbers"
            },
            new CalculatorTestCase
            {
                Id = 21,
                TestName = "Multiply_VerySmallDecimals",
                Category = "EdgeCases",
                FirstOperand = 0.001,
                SecondOperand = 0.001,
                Operation = "Multiply",
                ExpectedResult = 0.000001,
                Description = "Multiplying very small decimals"
            },

            // Additional decimal test cases
            new CalculatorTestCase
            {
                Id = 22,
                TestName = "Subtract_Decimals",
                Category = "Subtraction",
                FirstOperand = 5.8,
                SecondOperand = 2.3,
                Operation = "Subtract",
                ExpectedResult = 3.5,
                Description = "Subtracting decimal numbers"
            },
            new CalculatorTestCase
            {
                Id = 23,
                TestName = "Multiply_Decimals",
                Category = "Multiplication",
                FirstOperand = 3.5,
                SecondOperand = 2.0,
                Operation = "Multiply",
                ExpectedResult = 7.0,
                Description = "Multiplying decimal numbers"
            },
            new CalculatorTestCase
            {
                Id = 24,
                TestName = "Divide_Decimals",
                Category = "Division",
                FirstOperand = 7.5,
                SecondOperand = 2.5,
                Operation = "Divide",
                ExpectedResult = 3.0,
                Description = "Dividing decimal numbers"
            },
            new CalculatorTestCase
            {
                Id = 25,
                TestName = "Add_MixedIntegerDecimal",
                Category = "Addition",
                FirstOperand = 5.0,
                SecondOperand = 3.7,
                Operation = "Add",
                ExpectedResult = 8.7,
                Description = "Adding integer and decimal"
            },
            new CalculatorTestCase
            {
                Id = 26,
                TestName = "Multiply_DecimalByInteger",
                Category = "Multiplication",
                FirstOperand = 4.5,
                SecondOperand = 3.0,
                Operation = "Multiply",
                ExpectedResult = 13.5,
                Description = "Multiplying decimal by integer"
            }
        );
    }
}
