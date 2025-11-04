"""
Day 1.1 Demo: GitHub Copilot Function Generation
Demonstrates how GitHub Copilot can generate functions from context and docstrings.
"""

# Example 1: Generate a utility function with docstring
def validate_email(email: str) -> bool:
    """
    Validate if the provided email follows a basic email format.
    
    Args:
        email: Email address to validate
        
    Returns:
        True if email is valid, False otherwise
        
    Note: This is a simplified validation. GitHub Copilot suggests using
    proper email validation libraries for production code.
    """
    import re
    pattern = r'^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$'
    return re.match(pattern, email) is not None

# Add the main function block to test the function
if __name__ == "__main__":
    test_emails = ["test@example.com", "invalid-email", "user@domain", "user.name@domain.com"]
    for email in test_emails:
        is_valid = validate_email(email)
        print(f"Email: {email}, Valid: {is_valid}")
# Example 2: Generate a function to calculate factorial
# def factorial(n: int) -> int:
#    """
#   Calculate the factorial of a non-negative integer n.
#   Args:
#          
