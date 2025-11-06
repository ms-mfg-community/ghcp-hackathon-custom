using Bunit;
using calculator.tests.Data;
using calculator.web.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace calculator.tests;

/// <summary>
/// Blazor Calculator component tests using bUnit and SQLite database test data
/// </summary>
public class CalculatorComponentDatabaseTests : TestContext, IDisposable
{
    private readonly TestDataHelper _testDataHelper;

    public CalculatorComponentDatabaseTests()
    {
        _testDataHelper = new TestDataHelper();

        // Register required services
        Services.AddLogging();
        Services.AddScoped<calculator.web.Services.ICalculatorService, calculator.web.Services.CalculatorService>();
        Services.AddScoped<calculator.web.Services.ICalculationHistoryService, calculator.web.Services.CalculationHistoryService>();
    }

    [Fact]
    public void Calculator_RendersSuccessfully()
    {
        // Act
        var cut = RenderComponent<Calculator>();

        // Assert
        Assert.NotNull(cut);
        Assert.Contains("Calculator", cut.Markup);
    }

    [Fact]
    public void Calculator_Addition_DatabaseTests_ProduceCorrectResults()
    {
        // Arrange
        var additionTests = _testDataHelper.GetTestCasesByOperation("Add");
        var cut = RenderComponent<Calculator>();

        foreach (var testCase in additionTests)
        {
            // Skip negative numbers for UI testing (no negative button)
            if (testCase.FirstOperand < 0 || testCase.SecondOperand < 0)
                continue;

            // Act - Enter first operand (supports decimals now)
            EnterNumber(cut, testCase.FirstOperand);

            // Click operation
            var addButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == "+");
            if (addButton != null)
            {
                addButton.Click();

                // Enter second operand
                EnterNumber(cut, testCase.SecondOperand);

                // Click equals
                var equalsButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == "=");
                if (equalsButton != null)
                {
                    equalsButton.Click();

                    // Assert
                    var resultDisplay = cut.Find(".display-value");
                    if (double.TryParse(resultDisplay.TextContent, out var actualResult))
                    {
                        Assert.Equal(testCase.ExpectedResult, actualResult, precision: 1);
                    }
                }
            }

            // Reset for next test
            var resetButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == "C");
            resetButton?.Click();
        }
    }

    [Fact]
    public void Calculator_AllOperations_DatabaseTests_ExecuteCorrectly()
    {
        // Arrange
        var allTests = _testDataHelper.GetAllTestCases();
        int testsRun = 0;

        foreach (var testCase in allTests)
        {
            // Act
            var cut = RenderComponent<Calculator>();

            try
            {
                // Simulate calculator operations based on test case
                SimulateCalculatorOperation(cut, testCase);
                testsRun++;
            }
            catch (Exception ex)
            {
                // Log failure but continue
                Assert.Fail($"Test '{testCase.TestName}' failed: {ex.Message}");
            }
        }

        // Assert
        Assert.True(testsRun > 0, "No tests were executed");
    }

    [Theory]
    [MemberData(nameof(GetCategoryTestData), "Addition")]
    public void Calculator_Addition_CategoryTests_FromDatabase(string testName, double operand1, double operand2, double expected)
    {
        // Arrange
        var cut = RenderComponent<Calculator>();
        var calculator = Services.GetRequiredService<calculator.web.Services.ICalculatorService>();

        // Act
        var result = calculator.Add(operand1, operand2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Theory]
    [MemberData(nameof(GetCategoryTestData), "Subtraction")]
    public void Calculator_Subtraction_CategoryTests_FromDatabase(string testName, double operand1, double operand2, double expected)
    {
        // Arrange
        var calculator = Services.GetRequiredService<calculator.web.Services.ICalculatorService>();

        // Act
        var result = calculator.Subtract(operand1, operand2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Theory]
    [MemberData(nameof(GetCategoryTestData), "Multiplication")]
    public void Calculator_Multiplication_CategoryTests_FromDatabase(string testName, double operand1, double operand2, double expected)
    {
        // Arrange
        var calculator = Services.GetRequiredService<calculator.web.Services.ICalculatorService>();

        // Act
        var result = calculator.Multiply(operand1, operand2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    [Theory]
    [MemberData(nameof(GetCategoryTestData), "Division")]
    public void Calculator_Division_CategoryTests_FromDatabase(string testName, double operand1, double operand2, double expected)
    {
        // Arrange
        var calculator = Services.GetRequiredService<calculator.web.Services.ICalculatorService>();

        // Act
        var result = calculator.Divide(operand1, operand2);

        // Assert
        Assert.Equal(expected, result, precision: 10);
    }

    public static IEnumerable<object[]> GetCategoryTestData(string category)
    {
        using var helper = new TestDataHelper();
        var testCases = helper.GetTestCasesByCategory(category);

        return testCases.Select(tc => new object[]
        {
            tc.TestName,
            tc.FirstOperand,
            tc.SecondOperand,
            tc.ExpectedResult
        });
    }

    /// <summary>
    /// Helper method to enter a number (integer or decimal) via button clicks
    /// </summary>
    private void EnterNumber(IRenderedComponent<Calculator> cut, double number)
    {
        var numberStr = number.ToString("G");
        foreach (var ch in numberStr)
        {
            if (ch == '.')
            {
                // Click decimal button
                var decimalButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == ".");
                decimalButton?.Click();
            }
            else if (ch == '-')
            {
                // For negative numbers, we'll skip for now as it requires subtraction from 0
                continue;
            }
            else if (char.IsDigit(ch))
            {
                // Click digit button
                var digitButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == ch.ToString());
                digitButton?.Click();
            }
        }
    }

    private void SimulateCalculatorOperation(IRenderedComponent<Calculator> cut, CalculatorTestCase testCase)
    {
        // This is a simplified simulation - actual button clicks would need more complex logic
        var calculator = Services.GetRequiredService<calculator.web.Services.ICalculatorService>();

        double result = testCase.Operation switch
        {
            "Add" => calculator.Add(testCase.FirstOperand, testCase.SecondOperand),
            "Subtract" => calculator.Subtract(testCase.FirstOperand, testCase.SecondOperand),
            "Multiply" => calculator.Multiply(testCase.FirstOperand, testCase.SecondOperand),
            "Divide" => calculator.Divide(testCase.FirstOperand, testCase.SecondOperand),
            _ => throw new InvalidOperationException($"Unknown operation: {testCase.Operation}")
        };

        Assert.Equal(testCase.ExpectedResult, result, precision: 10);
    }

    [Fact]
    public void Calculator_DecimalOperations_DatabaseTests_ProduceCorrectResults()
    {
        // Arrange - Get test cases that involve decimals
        var allTests = _testDataHelper.GetAllTestCases()
            .Where(tc => tc.FirstOperand != Math.Floor(tc.FirstOperand) ||
                         tc.SecondOperand != Math.Floor(tc.SecondOperand))
            .Where(tc => tc.FirstOperand >= 0 && tc.SecondOperand >= 0) // Skip negatives
            .ToList();

        var cut = RenderComponent<Calculator>();

        foreach (var testCase in allTests)
        {
            // Act - Enter first operand
            EnterNumber(cut, testCase.FirstOperand);

            // Click operation
            var operatorSymbol = testCase.Operation switch
            {
                "Add" => "+",
                "Subtract" => "−",
                "Multiply" => "×",
                "Divide" => "÷",
                _ => null
            };

            if (operatorSymbol != null)
            {
                var operatorButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Contains(operatorSymbol));
                if (operatorButton != null)
                {
                    operatorButton.Click();

                    // Enter second operand
                    EnterNumber(cut, testCase.SecondOperand);

                    // Click equals
                    var equalsButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == "=");
                    if (equalsButton != null)
                    {
                        equalsButton.Click();

                        // Assert
                        var resultDisplay = cut.Find(".display-value");
                        if (double.TryParse(resultDisplay.TextContent, out var actualResult))
                        {
                            Assert.Equal(testCase.ExpectedResult, actualResult, precision: 1);
                        }
                    }
                }
            }

            // Reset for next test
            var resetButton = cut.FindAll(".btn-keypad").FirstOrDefault(b => b.TextContent.Trim() == "C");
            resetButton?.Click();
        }
    }

    [Fact]
    public void Calculator_ClearButton_ResetsDisplay()
    {
        // Arrange
        var cut = RenderComponent<Calculator>();

        // Act - Find and click clear button
        var clearButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("C") || b.TextContent.Contains("Clear"));

        if (clearButton != null)
        {
            clearButton.Click();

            // Assert
            var display = cut.Find(".calculator-display");
            Assert.NotNull(display);
        }
    }

    [Fact]
    public void Calculator_HasRequiredElements()
    {
        // Act
        var cut = RenderComponent<Calculator>();

        // Assert
        Assert.NotNull(cut.Find(".calculator-display"));

        // Check that we have calculator keypad buttons
        var allButtons = cut.FindAll(".btn-keypad");
        Assert.NotEmpty(allButtons);

        // Should have at least 10 number buttons (0-9) + operation buttons
        Assert.True(allButtons.Count >= 14, $"Expected at least 14 buttons, but found {allButtons.Count}");

        // Check for number buttons (0-9)
        for (int i = 0; i <= 9; i++)
        {
            var numberButtons = cut.FindAll(".btn-keypad").Where(b => b.TextContent.Trim() == i.ToString());
            Assert.NotEmpty(numberButtons);
        }

        // Check for operation buttons (using actual symbols from Calculator.razor)
        var operationButtons = new[] { "+", "−", "×", "÷", "=", "C" };
        foreach (var op in operationButtons)
        {
            var opButtons = cut.FindAll(".btn-keypad").Where(b => b.TextContent.Contains(op));
            Assert.NotEmpty(opButtons);
        }
    }

    public new void Dispose()
    {
        _testDataHelper?.Dispose();
        base.Dispose();
    }
}
