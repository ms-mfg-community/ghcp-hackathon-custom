using calculator;

namespace calculator.web.Services;

/// <summary>
/// Calculator service implementation that wraps the static CalculatorOperations class.
/// This wrapper enables dependency injection and improved testability.
/// </summary>
public class CalculatorService : ICalculatorService
{
    public double Add(double a, double b) => CalculatorOperations.Add(a, b);
    public double Subtract(double a, double b) => CalculatorOperations.Subtract(a, b);
    public double Multiply(double a, double b) => CalculatorOperations.Multiply(a, b);
    public double Divide(double a, double b) => CalculatorOperations.Divide(a, b);
    public double Modulo(double a, double b) => CalculatorOperations.Modulo(a, b);
    public double Exponent(double a, double b) => CalculatorOperations.Exponent(a, b);
}
