// Basic .NET Calculator Application
// Implements arithmetic operations with user input handling

string? continueCalculation = "y";

while (continueCalculation?.ToLower() == "y")
{
    Console.Clear();
    
    // Prompt for first operand
    Console.Write("Enter the first number: ");
    string? input1 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input1))
    {
        Console.WriteLine("Error: First number cannot be empty.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        continue;
    }
    
    if (!double.TryParse(input1, out double num1))
    {
        Console.WriteLine("Error: Invalid number format for first operand.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        continue;
    }
    
    // Prompt for second operand
    Console.Write("Enter the second number: ");
    string? input2 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input2))
    {
        Console.WriteLine("Error: Second number cannot be empty.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        continue;
    }
    
    if (!double.TryParse(input2, out double num2))
    {
        Console.WriteLine("Error: Invalid number format for second operand.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        continue;
    }
    
    // Prompt for operator
    Console.Write("Enter an operator (+, -, *, /, %, ^): ");
    string? operatorInput = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(operatorInput))
    {
        Console.WriteLine("Error: Operator cannot be empty.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        continue;
    }
    
    // Perform the appropriate arithmetic operation using methods
    try
    {
        double result;
        
        switch (operatorInput)
        {
            case "+":
                result = CalculatorOperations.Add(num1, num2);
                Console.WriteLine($"Result: {num1} + {num2} = {result}");
                break;
            case "-":
                result = CalculatorOperations.Subtract(num1, num2);
                Console.WriteLine($"Result: {num1} - {num2} = {result}");
                break;
            case "*":
                result = CalculatorOperations.Multiply(num1, num2);
                Console.WriteLine($"Result: {num1} * {num2} = {result}");
                break;
            case "/":
                result = CalculatorOperations.Divide(num1, num2);
                Console.WriteLine($"Result: {num1} / {num2} = {result}");
                break;
            case "%":
                result = CalculatorOperations.Modulo(num1, num2);
                Console.WriteLine($"Result: {num1} % {num2} = {result}");
                break;
            case "^":
                result = CalculatorOperations.Exponent(num1, num2);
                Console.WriteLine($"Result: {num1} ^ {num2} = {result}");
                break;
            default:
                Console.WriteLine($"Error: Invalid operator '{operatorInput}'. Please use +, -, *, /, %, or ^.");
                break;
        }
    }
    catch (DivideByZeroException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: An unexpected error occurred: {ex.Message}");
    }
    
    // Ask if the user wants to perform another calculation
    Console.Write("\nWould you like to perform another calculation? (y/n): ");
    continueCalculation = Console.ReadLine();
}

Console.Clear();
Console.WriteLine("Thank you for using the calculator. Goodbye!");

// Public static class for testable arithmetic operations
public static class CalculatorOperations
{
    public static double Add(double num1, double num2)
    {
        return num1 + num2;
    }

    public static double Subtract(double num1, double num2)
    {
        return num1 - num2;
    }

    public static double Multiply(double num1, double num2)
    {
        return num1 * num2;
    }

    public static double Divide(double num1, double num2)
    {
        if (num2 == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
        return num1 / num2;
    }

    public static double Modulo(double num1, double num2)
    {
        if (num2 == 0)
        {
            throw new DivideByZeroException("Cannot perform modulo with zero.");
        }
        return num1 % num2;
    }

    public static double Exponent(double num1, double num2)
    {
        return Math.Pow(num1, num2);
    }
}


