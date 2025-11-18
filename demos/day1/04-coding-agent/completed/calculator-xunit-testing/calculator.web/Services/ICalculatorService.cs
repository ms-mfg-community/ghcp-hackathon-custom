namespace calculator.web.Services;

/// <summary>
/// Interface for calculator operations service.
/// Wraps the static CalculatorOperations class for dependency injection.
/// </summary>
public interface ICalculatorService
{
    /// <summary>
    /// Performs addition of two numbers.
    /// </summary>
    double Add(double a, double b);

    /// <summary>
    /// Performs subtraction of two numbers.
    /// </summary>
    double Subtract(double a, double b);

    /// <summary>
    /// Performs multiplication of two numbers.
    /// </summary>
    double Multiply(double a, double b);

    /// <summary>
    /// Performs division of two numbers.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when divisor is zero.</exception>
    double Divide(double a, double b);

    /// <summary>
    /// Performs modulo operation on two numbers.
    /// </summary>
    /// <exception cref="DivideByZeroException">Thrown when divisor is zero.</exception>
    double Modulo(double a, double b);

    /// <summary>
    /// Performs exponentiation (a raised to the power of b).
    /// </summary>
    double Exponent(double a, double b);
}
