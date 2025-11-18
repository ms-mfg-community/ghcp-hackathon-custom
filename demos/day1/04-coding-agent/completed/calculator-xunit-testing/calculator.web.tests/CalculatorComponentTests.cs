using Bunit;
using calculator.web.Pages;
using calculator.web.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace calculator.web.tests;

/// <summary>
/// Component tests for Calculator.razor using bUnit.
/// </summary>
public class CalculatorComponentTests : TestContext
{
    private readonly Mock<ICalculatorService> _mockCalculatorService;
    private readonly Mock<ICalculationHistoryService> _mockHistoryService;
    private readonly Mock<ILogger<Calculator>> _mockLogger;

    public CalculatorComponentTests()
    {
        _mockCalculatorService = new Mock<ICalculatorService>();
        _mockHistoryService = new Mock<ICalculationHistoryService>();
        _mockLogger = new Mock<ILogger<Calculator>>();

        // Register mocked services
        Services.AddSingleton(_mockCalculatorService.Object);
        Services.AddSingleton(_mockHistoryService.Object);
        Services.AddSingleton(_mockLogger.Object);
    }

    [Fact]
    public void Calculator_RendersCorrectly()
    {
        // Arrange
        _mockHistoryService.Setup(s => s.GetHistory()).Returns(new List<CalculationEntry>());
        _mockHistoryService.Setup(s => s.Count).Returns(0);

        // Act
        var cut = RenderComponent<Calculator>();

        // Assert
        Assert.NotNull(cut);
        Assert.Contains("Blazor Calculator", cut.Markup);

        var firstOperandInput = cut.Find("input[id='firstOperand']");
        Assert.NotNull(firstOperandInput);

        var secondOperandInput = cut.Find("input[id='secondOperand']");
        Assert.NotNull(secondOperandInput);

        var operatorSelect = cut.Find("select[id='operator']");
        Assert.NotNull(operatorSelect);

        var calculateButton = cut.Find("button.btn-primary");
        Assert.NotNull(calculateButton);
    }

    [Fact]
    public void Calculate_WithAddition_DisplaysCorrectResult()
    {
        // Arrange
        _mockHistoryService.Setup(s => s.GetHistory()).Returns(new List<CalculationEntry>());
        _mockHistoryService.Setup(s => s.Count).Returns(0);
        _mockCalculatorService.Setup(s => s.Add(5, 3)).Returns(8);

        var cut = RenderComponent<Calculator>();

        // Act
        var firstOperandInput = cut.Find("input[id='firstOperand']");
        firstOperandInput.Change(5);

        var secondOperandInput = cut.Find("input[id='secondOperand']");
        secondOperandInput.Change(3);

        var operatorSelect = cut.Find("select[id='operator']");
        operatorSelect.Change("+");

        var calculateButton = cut.Find("button.btn-primary");
        calculateButton.Click();

        // Assert
        _mockCalculatorService.Verify(s => s.Add(5, 3), Times.Once);
        _mockHistoryService.Verify(s => s.AddEntry(5, 3, "+", 8), Times.Once);

        var resultDiv = cut.Find(".alert-success");
        Assert.Contains("8", resultDiv.TextContent);
    }

    [Fact]
    public void Calculate_WithDivisionByZero_DisplaysError()
    {
        // Arrange
        _mockHistoryService.Setup(s => s.GetHistory()).Returns(new List<CalculationEntry>());
        _mockHistoryService.Setup(s => s.Count).Returns(0);
        _mockCalculatorService.Setup(s => s.Divide(10, 0)).Throws<DivideByZeroException>();

        var cut = RenderComponent<Calculator>();

        // Act
        var firstOperandInput = cut.Find("input[id='firstOperand']");
        firstOperandInput.Change(10);

        var secondOperandInput = cut.Find("input[id='secondOperand']");
        secondOperandInput.Change(0);

        var operatorSelect = cut.Find("select[id='operator']");
        operatorSelect.Change("/");

        var calculateButton = cut.Find("button.btn-primary");
        calculateButton.Click();

        // Assert
        _mockCalculatorService.Verify(s => s.Divide(10, 0), Times.Once);

        var errorDiv = cut.Find(".alert-danger");
        Assert.Contains("Cannot divide by zero", errorDiv.TextContent);
    }

    [Fact]
    public void ClearHistory_InvokesHistoryService()
    {
        // Arrange
        _mockHistoryService.Setup(s => s.GetHistory()).Returns(new List<CalculationEntry>
        {
            new CalculationEntry { Timestamp = DateTime.Now, FirstOperand = 5, SecondOperand = 3, Operator = "+", Result = 8 }
        });
        _mockHistoryService.Setup(s => s.Count).Returns(1);

        var cut = RenderComponent<Calculator>();

        // Act
        var clearButton = cut.Find("button.btn-outline-secondary");
        clearButton.Click();

        // Assert
        _mockHistoryService.Verify(s => s.ClearHistory(), Times.Once);
    }

    [Theory]
    [InlineData("+", 5, 3, 8)]
    [InlineData("-", 10, 4, 6)]
    [InlineData("*", 6, 7, 42)]
    [InlineData("/", 15, 3, 5)]
    [InlineData("%", 10, 3, 1)]
    [InlineData("^", 2, 3, 8)]
    public void Calculate_WithVariousOperations_CallsCorrectServiceMethod(
        string operatorSymbol, double first, double second, double expected)
    {
        // Arrange
        _mockHistoryService.Setup(s => s.GetHistory()).Returns(new List<CalculationEntry>());
        _mockHistoryService.Setup(s => s.Count).Returns(0);

        switch (operatorSymbol)
        {
            case "+":
                _mockCalculatorService.Setup(s => s.Add(first, second)).Returns(expected);
                break;
            case "-":
                _mockCalculatorService.Setup(s => s.Subtract(first, second)).Returns(expected);
                break;
            case "*":
                _mockCalculatorService.Setup(s => s.Multiply(first, second)).Returns(expected);
                break;
            case "/":
                _mockCalculatorService.Setup(s => s.Divide(first, second)).Returns(expected);
                break;
            case "%":
                _mockCalculatorService.Setup(s => s.Modulo(first, second)).Returns(expected);
                break;
            case "^":
                _mockCalculatorService.Setup(s => s.Exponent(first, second)).Returns(expected);
                break;
        }

        var cut = RenderComponent<Calculator>();

        // Act
        var firstOperandInput = cut.Find("input[id='firstOperand']");
        firstOperandInput.Change(first);

        var secondOperandInput = cut.Find("input[id='secondOperand']");
        secondOperandInput.Change(second);

        var operatorSelect = cut.Find("select[id='operator']");
        operatorSelect.Change(operatorSymbol);

        var calculateButton = cut.Find("button.btn-primary");
        calculateButton.Click();

        // Assert
        var resultDiv = cut.Find(".alert-success");
        Assert.Contains(expected.ToString(), resultDiv.TextContent);
    }

    [Fact]
    public void HistoryDisplay_ShowsCalculationHistory()
    {
        // Arrange
        var historyEntries = new List<CalculationEntry>
        {
            new CalculationEntry { Timestamp = DateTime.Now, FirstOperand = 5, SecondOperand = 3, Operator = "+", Result = 8 },
            new CalculationEntry { Timestamp = DateTime.Now, FirstOperand = 10, SecondOperand = 2, Operator = "/", Result = 5 }
        };

        _mockHistoryService.Setup(s => s.GetHistory()).Returns(historyEntries);
        _mockHistoryService.Setup(s => s.Count).Returns(2);

        // Act
        var cut = RenderComponent<Calculator>();

        // Assert
        Assert.Contains("Calculation History", cut.Markup);
        Assert.Contains("5 + 3 = 8", cut.Markup);
        Assert.Contains("10 / 2 = 5", cut.Markup);
        Assert.Contains("(2 entries)", cut.Markup);
    }
}
