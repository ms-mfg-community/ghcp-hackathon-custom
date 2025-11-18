// Calculator application with top-level statements
// Prompts user for two operands and an operator, then performs the calculation

string continueCalculating = "y";

while (continueCalculating?.ToLower() == "y")
{
    Console.Clear(); // Clear screen for better user experience
    
    // Prompt for first operand
    Console.Write("Enter the first number: ");
    string? input1 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input1))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        continue;
    }
    
    if (!double.TryParse(input1, out double operand1))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        continue;
    }

    // Prompt for second operand
    Console.Write("Enter the second number: ");
    string? input2 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input2))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        continue;
    }
    
    if (!double.TryParse(input2, out double operand2))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        continue;
    }

    // Prompt for operator
    Console.Write("Enter an operator (+, -, *, /, %, ^): ");
    string? operatorInput = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(operatorInput))
    {
        Console.WriteLine("Invalid operator. Please try again.");
        continue;
    }

    double result;

    // Perform calculation based on operator
    switch (operatorInput)
    {
        case "+":
            result = CalculatorOperations.Add(operand1, operand2);
            Console.WriteLine($"Result: {operand1} + {operand2} = {result}");
            break;
        case "-":
            result = CalculatorOperations.Subtract(operand1, operand2);
            Console.WriteLine($"Result: {operand1} - {operand2} = {result}");
            break;
        case "*":
            result = CalculatorOperations.Multiply(operand1, operand2);
            Console.WriteLine($"Result: {operand1} * {operand2} = {result}");
            break;
        case "/":
            if (operand2 == 0)
            {
                Console.WriteLine("Error: Division by zero is not allowed.");
            }
            else
            {
                result = CalculatorOperations.Divide(operand1, operand2);
                Console.WriteLine($"Result: {operand1} / {operand2} = {result}");
            }
            break;
        case "%":
            if (operand2 == 0)
            {
                Console.WriteLine("Error: Modulo by zero is not allowed.");
            }
            else
            {
                result = CalculatorOperations.Modulo(operand1, operand2);
                Console.WriteLine($"Result: {operand1} % {operand2} = {result}");
            }
            break;
        case "^":
            result = CalculatorOperations.Exponent(operand1, operand2);
            Console.WriteLine($"Result: {operand1} ^ {operand2} = {result}");
            break;
        default:
            Console.WriteLine("Invalid operator. Please use +, -, *, /, %, or ^.");
            break;
    }

    // Ask user if they want to perform another calculation
    Console.Write("\nWould you like to perform another calculation? (y/n): ");
    continueCalculating = Console.ReadLine() ?? "n";
    
    Console.WriteLine(); // Add blank line for readability
}

Console.WriteLine("Thank you for using the calculator. Goodbye!");

// Public class for arithmetic operations - testable from xUnit project
public static class CalculatorOperations
{
    /// <summary>
    /// Adds two numbers together
    /// </summary>
    /// <param name="a">First operand</param>
    /// <param name="b">Second operand</param>
    /// <returns>Sum of a and b</returns>
    public static double Add(double a, double b)
    {
        return a + b;
    }

    /// <summary>
    /// Subtracts the second number from the first
    /// </summary>
    /// <param name="a">First operand (minuend)</param>
    /// <param name="b">Second operand (subtrahend)</param>
    /// <returns>Difference of a and b</returns>
    public static double Subtract(double a, double b)
    {
        return a - b;
    }

    /// <summary>
    /// Multiplies two numbers together
    /// </summary>
    /// <param name="a">First operand</param>
    /// <param name="b">Second operand</param>
    /// <returns>Product of a and b</returns>
    public static double Multiply(double a, double b)
    {
        return a * b;
    }

    /// <summary>
    /// Divides the first number by the second
    /// </summary>
    /// <param name="a">Numerator</param>
    /// <param name="b">Denominator</param>
    /// <returns>Quotient of a divided by b</returns>
    /// <exception cref="DivideByZeroException">Thrown when b is zero</exception>
    public static double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero");
        }
        return a / b;
    }

    /// <summary>
    /// Calculates the modulo (remainder) of the first number divided by the second
    /// </summary>
    /// <param name="a">Dividend</param>
    /// <param name="b">Divisor</param>
    /// <returns>Remainder of a divided by b</returns>
    /// <exception cref="DivideByZeroException">Thrown when b is zero</exception>
    public static double Modulo(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Cannot perform modulo by zero");
        }
        return a % b;
    }

    /// <summary>
    /// Raises the first number to the power of the second
    /// </summary>
    /// <param name="a">Base</param>
    /// <param name="b">Exponent</param>
    /// <returns>a raised to the power of b</returns>
    public static double Exponent(double a, double b)
    {
        return Math.Pow(a, b);
    }
}
