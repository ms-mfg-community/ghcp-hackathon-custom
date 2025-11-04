// Calculator.cs - .NET 9.0 Console Calculator with xUnit Testing Support
// Demonstrates arithmetic operations, error handling, and testable method design

Console.Clear();
Console.WriteLine("======================================");
Console.WriteLine("   .NET Console Calculator");
Console.WriteLine("======================================");
Console.WriteLine();

bool continueCalculating = true;

while (continueCalculating)
{
    try
    {
        // Get first operand
        Console.Write("Enter first number: ");
        string? input1 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input1))
        {
            Console.WriteLine("Error: First number cannot be empty.");
            continue;
        }

        if (!double.TryParse(input1, out double num1))
        {
            Console.WriteLine("Error: Invalid first number.");
            continue;
        }

        // Get second operand
        Console.Write("Enter second number: ");
        string? input2 = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input2))
        {
            Console.WriteLine("Error: Second number cannot be empty.");
            continue;
        }

        if (!double.TryParse(input2, out double num2))
        {
            Console.WriteLine("Error: Invalid second number.");
            continue;
        }

        // Get operator
        Console.Write("Enter operator (+, -, *, /, %, ^): ");
        string? operatorInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(operatorInput))
        {
            Console.WriteLine("Error: Operator cannot be empty.");
            continue;
        }

        // Perform calculation
        double result;
        bool validOperation = true;

        switch (operatorInput)
        {
            case "+":
                result = Calculator.Add(num1, num2);
                break;
            case "-":
                result = Calculator.Subtract(num1, num2);
                break;
            case "*":
                result = Calculator.Multiply(num1, num2);
                break;
            case "/":
                if (num2 == 0)
                {
                    Console.WriteLine("Error: Cannot divide by zero.");
                    validOperation = false;
                    result = 0;
                }
                else
                {
                    result = Calculator.Divide(num1, num2);
                }
                break;
            case "%":
                if (num2 == 0)
                {
                    Console.WriteLine("Error: Cannot perform modulo with zero.");
                    validOperation = false;
                    result = 0;
                }
                else
                {
                    result = Calculator.Modulo(num1, num2);
                }
                break;
            case "^":
                result = Calculator.Exponent(num1, num2);
                break;
            default:
                Console.WriteLine($"Error: Unknown operator '{operatorInput}'.");
                validOperation = false;
                result = 0;
                break;
        }

        // Display result if operation was valid
        if (validOperation)
        {
            Console.WriteLine();
            Console.WriteLine($"Result: {num1} {operatorInput} {num2} = {result}");
        }

        // Ask if user wants to continue
        Console.WriteLine();
        Console.Write("Perform another calculation? (y/n): ");
        string? continueInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(continueInput) ||
            !continueInput.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            continueCalculating = false;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("   .NET Console Calculator");
            Console.WriteLine("======================================");
            Console.WriteLine();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        continueCalculating = false;
    }
}

Console.WriteLine();
Console.WriteLine("Thank you for using the calculator!");

/// <summary>
/// Public static class containing all arithmetic operation methods.
/// These methods are accessible to unit tests.
/// </summary>
public static class Calculator
{
    /// <summary>
    /// Adds two numbers together.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The sum of a and b.</returns>
    public static double Add(double a, double b)
    {
        return a + b;
    }

    /// <summary>
    /// Subtracts the second number from the first number.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The difference of a and b.</returns>
    public static double Subtract(double a, double b)
    {
        return a - b;
    }

    /// <summary>
    /// Multiplies two numbers together.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The product of a and b.</returns>
    public static double Multiply(double a, double b)
    {
        return a * b;
    }

    /// <summary>
    /// Divides the first number by the second number.
    /// </summary>
    /// <param name="a">The numerator.</param>
    /// <param name="b">The denominator.</param>
    /// <returns>The quotient of a divided by b.</returns>
    /// <exception cref="DivideByZeroException">Thrown when b is zero.</exception>
    public static double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return a / b;
    }

    /// <summary>
    /// Calculates the modulo (remainder) of the first number divided by the second number.
    /// </summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <returns>The remainder of a divided by b.</returns>
    /// <exception cref="DivideByZeroException">Thrown when b is zero.</exception>
    public static double Modulo(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot perform modulo with zero.");
        }
        return a % b;
    }

    /// <summary>
    /// Raises the first number to the power of the second number.
    /// </summary>
    /// <param name="baseNumber">The base number.</param>
    /// <param name="exponent">The exponent.</param>
    /// <returns>The result of baseNumber raised to the power of exponent.</returns>
    public static double Exponent(double baseNumber, double exponent)
    {
        return Math.Pow(baseNumber, exponent);
    }
}