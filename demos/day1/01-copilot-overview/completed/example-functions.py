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
    pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
    return re.match(pattern, email) is not None


# Example 2: Parse JSON data with error handling
def parse_json_safely(json_string: str) -> dict:
    """
    Safely parse a JSON string and return the parsed object.
    Returns an empty dict if parsing fails.
    """
    import json
    try:
        return json.loads(json_string)
    except json.JSONDecodeError as e:
        print(f"Error parsing JSON: {e}")
        return {}


# Example 3: Generate a list transformation function
def transform_user_data(users: list[dict]) -> list[dict]:
    """
    Transform raw user data by normalizing names and filtering active users.
    
    Copilot Suggestion: Consider the business logic requirements
    before accepting generated code suggestions.
    """
    normalized_users = []
    for user in users:
        if user.get('is_active', False):
            normalized_user = {
                'id': user.get('id'),
                'name': user.get('name', '').strip().title(),
                'email': user.get('email', '').lower(),
                'created_at': user.get('created_at')
            }
            normalized_users.append(normalized_user)
    return normalized_users


# Example 4: Generate a caching decorator
def cache_result(func):
    """
    Decorator that caches function results.
    Demonstrates how Copilot can generate decorator patterns.
    """
    cache = {}
    
    def wrapper(*args, **kwargs):
        key = (args, tuple(kwargs.items()))
        if key not in cache:
            cache[key] = func(*args, **kwargs)
        return cache[key]
    
    return wrapper


@cache_result
def fibonacci(n: int) -> int:
    """
    Calculate the nth Fibonacci number.
    Copilot assists in recognizing the caching pattern
    and optimizes performance.
    """
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)


# Example 5: String manipulation utilities
def reverse_string(s: str) -> str:
    """Reverse a string - Copilot knows multiple approaches."""
    return s[::-1]


def capitalize_words(text: str) -> str:
    """Capitalize first letter of each word."""
    return ' '.join(word.capitalize() for word in text.split())


def remove_duplicates(items: list) -> list:
    """Remove duplicates while preserving order - Copilot suggests efficient approaches."""
    seen = set()
    result = []
    for item in items:
        if item not in seen:
            seen.add(item)
            result.append(item)
    return result


# Demonstration and testing
if __name__ == "__main__":
    print("=" * 60)
    print("GitHub Copilot Function Generation Demonstration")
    print("=" * 60)
    
    # Test email validation
    print("\n1. Email Validation:")
    test_emails = ["user@example.com", "invalid.email", "test@domain.co.uk"]
    for email in test_emails:
        result = validate_email(email)
        print(f"   {email:25} -> {result}")
    
    # Test JSON parsing
    print("\n2. Safe JSON Parsing:")
    valid_json = '{"name": "John", "age": 30}'
    invalid_json = '{invalid json}'
    print(f"   Valid JSON: {parse_json_safely(valid_json)}")
    print(f"   Invalid JSON: {parse_json_safely(invalid_json)}")
    
    # Test user data transformation
    print("\n3. User Data Transformation:")
    users = [
        {'id': 1, 'name': '  alice smith  ', 'email': 'ALICE@EXAMPLE.COM', 'is_active': True},
        {'id': 2, 'name': '  bob jones  ', 'email': 'BOB@EXAMPLE.COM', 'is_active': False},
        {'id': 3, 'name': '  carol white  ', 'email': 'CAROL@EXAMPLE.COM', 'is_active': True},
    ]
    transformed = transform_user_data(users)
    for user in transformed:
        print(f"   {user}")
    
    # Test caching decorator
    print("\n4. Caching Decorator (Fibonacci):")
    for i in range(10):
        result = fibonacci(i)
        print(f"   fibonacci({i}) = {result}")
    
    # Test string utilities
    print("\n5. String Utilities:")
    text = "hello world"
    print(f"   Original: {text}")
    print(f"   Reversed: {reverse_string(text)}")
    print(f"   Capitalized: {capitalize_words(text)}")
    
    items = [1, 2, 2, 3, 3, 3, 4, 5, 5]
    print(f"   Duplicates removed: {remove_duplicates(items)}")
    
    print("\n" + "=" * 60)
    print("Demonstration Complete!")
    print("=" * 60)
