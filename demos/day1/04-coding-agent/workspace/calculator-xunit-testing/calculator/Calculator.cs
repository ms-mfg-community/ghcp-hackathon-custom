// Basic .NET Calculator Application
// Implements arithmetic operations with user input handling

string? continueCalculation = "y";

while (continueCalculation?.ToLower() == "y")
{
    // Prompt for first operand
    Console.Write("Enter the first number: ");
    string? input1 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input1))
    {
        Console.WriteLine("Error: First number cannot be empty.");
        continue;
    }
    
    if (!double.TryParse(input1, out double num1))
    {
        Console.WriteLine("Error: Invalid number format for first operand.");
        continue;
    }
    
    // Prompt for operator
    Console.Write("Enter an operator (+, -, *, /): ");
    string? operatorInput = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(operatorInput))
    {
        Console.WriteLine("Error: Operator cannot be empty.");
        continue;
    }
    
    // Prompt for second operand
    Console.Write("Enter the second number: ");
    string? input2 = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input2))
    {
        Console.WriteLine("Error: Second number cannot be empty.");
        continue;
    }
    
    if (!double.TryParse(input2, out double num2))
    {
        Console.WriteLine("Error: Invalid number format for second operand.");
        continue;
    }
    
    // Perform the appropriate arithmetic operation
    double result;
    
    switch (operatorInput)
    {
        case "+":
            result = num1 + num2;
            Console.WriteLine($"Result: {num1} + {num2} = {result}");
            break;
        case "-":
            result = num1 - num2;
            Console.WriteLine($"Result: {num1} - {num2} = {result}");
            break;
        case "*":
            result = num1 * num2;
            Console.WriteLine($"Result: {num1} * {num2} = {result}");
            break;
        case "/":
            if (num2 == 0)
            {
                Console.WriteLine("Error: Cannot divide by zero.");
            }
            else
            {
                result = num1 / num2;
                Console.WriteLine($"Result: {num1} / {num2} = {result}");
            }
            break;
        default:
            Console.WriteLine($"Error: Invalid operator '{operatorInput}'. Please use +, -, *, or /.");
            break;
    }
    
    // Ask if the user wants to perform another calculation
    Console.Write("\nWould you like to perform another calculation? (y/n): ");
    continueCalculation = Console.ReadLine();
}

Console.WriteLine("Thank you for using the calculator. Goodbye!");
