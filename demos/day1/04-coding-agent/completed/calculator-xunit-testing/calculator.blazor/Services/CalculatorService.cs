using calculator.blazor.Models;

namespace calculator.blazor.Services;

/// <summary>
/// Service wrapper for the static Calculator class.
/// Provides error handling and result formatting for Blazor components.
/// </summary>
public class CalculatorService
{
    /// <summary>
    /// Performs a calculation based on the operator.
    /// </summary>
    /// <param name="firstNumber">The first operand.</param>
    /// <param name="secondNumber">The second operand.</param>
    /// <param name="operatorSymbol">The operator symbol (+, -, *, /, %, ^).</param>
    /// <returns>A CalculationResult containing the result or error message.</returns>
    public CalculationResult Calculate(double firstNumber, double secondNumber, string operatorSymbol)
    {
        try
        {
            double result = operatorSymbol switch
            {
                "+" => Calculator.Add(firstNumber, secondNumber),
                "-" => Calculator.Subtract(firstNumber, secondNumber),
                "*" => Calculator.Multiply(firstNumber, secondNumber),
                "/" => Calculator.Divide(firstNumber, secondNumber),
                "%" => Calculator.Modulo(firstNumber, secondNumber),
                "^" => Calculator.Exponent(firstNumber, secondNumber),
                _ => throw new ArgumentException($"Unknown operator: {operatorSymbol}")
            };

            return new CalculationResult(true, result);
        }
        catch (DivideByZeroException ex)
        {
            return new CalculationResult(false, 0, ex.Message);
        }
        catch (Exception ex)
        {
            return new CalculationResult(false, 0, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the display name for an operator symbol.
    /// </summary>
    public string GetOperatorName(string operatorSymbol)
    {
        return operatorSymbol switch
        {
            "+" => "Add",
            "-" => "Subtract",
            "*" => "Multiply",
            "/" => "Divide",
            "%" => "Modulo",
            "^" => "Exponent",
            _ => operatorSymbol
        };
    }
}
