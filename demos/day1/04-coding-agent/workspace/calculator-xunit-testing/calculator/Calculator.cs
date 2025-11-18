// Calculator application with top-level statements
// Prompts user for two operands and an operator, then performs the calculation

string continueCalculating = "y";

while (continueCalculating?.ToLower() == "y")
{
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
    Console.Write("Enter an operator (+, -, *, /): ");
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
            result = operand1 + operand2;
            Console.WriteLine($"Result: {operand1} + {operand2} = {result}");
            break;
        case "-":
            result = operand1 - operand2;
            Console.WriteLine($"Result: {operand1} - {operand2} = {result}");
            break;
        case "*":
            result = operand1 * operand2;
            Console.WriteLine($"Result: {operand1} * {operand2} = {result}");
            break;
        case "/":
            if (operand2 == 0)
            {
                Console.WriteLine("Error: Division by zero is not allowed.");
            }
            else
            {
                result = operand1 / operand2;
                Console.WriteLine($"Result: {operand1} / {operand2} = {result}");
            }
            break;
        default:
            Console.WriteLine("Invalid operator. Please use +, -, *, or /.");
            break;
    }

    // Ask user if they want to perform another calculation
    Console.Write("\nWould you like to perform another calculation? (y/n): ");
    continueCalculating = Console.ReadLine() ?? "n";
    
    Console.WriteLine(); // Add blank line for readability
}

Console.WriteLine("Thank you for using the calculator. Goodbye!");
