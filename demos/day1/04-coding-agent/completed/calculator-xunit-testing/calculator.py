"""
Basic Python Calculator Application
Implements arithmetic operations with user input handling
This is a Python translation of the C# calculator for cross-language comparison
"""

import os
import math


class CalculatorOperations:
    """Public class for testable arithmetic operations"""
    
    @staticmethod
    def add(num1: float, num2: float) -> float:
        """Add two numbers"""
        return num1 + num2
    
    @staticmethod
    def subtract(num1: float, num2: float) -> float:
        """Subtract num2 from num1"""
        return num1 - num2
    
    @staticmethod
    def multiply(num1: float, num2: float) -> float:
        """Multiply two numbers"""
        return num1 * num2
    
    @staticmethod
    def divide(num1: float, num2: float) -> float:
        """Divide num1 by num2"""
        if num2 == 0:
            raise ZeroDivisionError("Cannot divide by zero.")
        return num1 / num2
    
    @staticmethod
    def modulo(num1: float, num2: float) -> float:
        """Calculate modulo of num1 and num2"""
        if num2 == 0:
            raise ZeroDivisionError("Cannot perform modulo with zero.")
        return num1 % num2
    
    @staticmethod
    def exponent(num1: float, num2: float) -> float:
        """Calculate num1 raised to the power of num2"""
        return math.pow(num1, num2)


def main():
    """Main calculator loop"""
    continue_calculation = "y"
    
    while continue_calculation.lower() == "y":
        os.system('cls' if os.name == 'nt' else 'clear')
        
        # Prompt for first operand
        input1 = input("Enter the first number: ")
        
        if not input1 or input1.strip() == "":
            print("Error: First number cannot be empty.")
            print("Press Enter to continue...")
            input()
            continue
        
        try:
            num1 = float(input1)
        except ValueError:
            print("Error: Invalid number format for first operand.")
            print("Press Enter to continue...")
            input()
            continue
        
        # Prompt for second operand
        input2 = input("Enter the second number: ")
        
        if not input2 or input2.strip() == "":
            print("Error: Second number cannot be empty.")
            print("Press Enter to continue...")
            input()
            continue
        
        try:
            num2 = float(input2)
        except ValueError:
            print("Error: Invalid number format for second operand.")
            print("Press Enter to continue...")
            input()
            continue
        
        # Prompt for operator
        operator_input = input("Enter an operator (+, -, *, /, %, ^): ")
        
        if not operator_input or operator_input.strip() == "":
            print("Error: Operator cannot be empty.")
            print("Press Enter to continue...")
            input()
            continue
        
        # Perform the appropriate arithmetic operation using methods
        try:
            result = None
            
            if operator_input == "+":
                result = CalculatorOperations.add(num1, num2)
                print(f"Result: {num1} + {num2} = {result}")
            elif operator_input == "-":
                result = CalculatorOperations.subtract(num1, num2)
                print(f"Result: {num1} - {num2} = {result}")
            elif operator_input == "*":
                result = CalculatorOperations.multiply(num1, num2)
                print(f"Result: {num1} * {num2} = {result}")
            elif operator_input == "/":
                result = CalculatorOperations.divide(num1, num2)
                print(f"Result: {num1} / {num2} = {result}")
            elif operator_input == "%":
                result = CalculatorOperations.modulo(num1, num2)
                print(f"Result: {num1} % {num2} = {result}")
            elif operator_input == "^":
                result = CalculatorOperations.exponent(num1, num2)
                print(f"Result: {num1} ^ {num2} = {result}")
            else:
                print(f"Error: Invalid operator '{operator_input}'. Please use +, -, *, /, %, or ^.")
        except ZeroDivisionError as ex:
            print(f"Error: {ex}")
        except Exception as ex:
            print(f"Error: An unexpected error occurred: {ex}")
        
        # Ask if the user wants to perform another calculation
        continue_calculation = input("\nWould you like to perform another calculation? (y/n): ")
    
    os.system('cls' if os.name == 'nt' else 'clear')
    print("Thank you for using the calculator. Goodbye!")


if __name__ == "__main__":
    main()
