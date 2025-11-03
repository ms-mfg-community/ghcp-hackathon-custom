"""
Day 1.3 Demo: Secure API Design with GitHub Copilot
Demonstrates security best practices and patterns for API development.
"""

import hashlib
import hmac
import secrets
from functools import wraps
from typing import Optional, Dict, Callable
from datetime import datetime, timedelta
import logging
import re

logger = logging.getLogger(__name__)


# Example 1: Secure credential handling pattern
class SecureCredentialManager:
    """
    Demonstrates how to handle credentials securely.
    Never embed credentials directly - use environment variables or secure vaults.
    """
    
    @staticmethod
    def get_api_key_from_env(key_name: str) -> Optional[str]:
        """
        Get API key from environment variable.
        Never accept credentials as function parameters in production.
        """
        import os
        api_key = os.getenv(key_name)
        if not api_key:
            raise ValueError(f"Environment variable {key_name} not found")
        return api_key
    
    @staticmethod
    def mask_sensitive_value(value: str, show_chars: int = 4) -> str:
        """Safely mask sensitive values for logging"""
        if len(value) <= show_chars:
            return "*" * len(value)
        return value[:show_chars] + "*" * (len(value) - show_chars)


# Example 2: Input validation pattern
class InputValidator:
    """
    Validates user input to prevent injection attacks and malicious data.
    GitHub Copilot can generate these validators from specifications.
    """
    
    @staticmethod
    def validate_email(email: str) -> bool:
        """Validate email format"""
        pattern = r'^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$'
        return re.match(pattern, email) is not None
    
    @staticmethod
    def validate_username(username: str) -> tuple[bool, str]:
        """Validate username format"""
        if len(username) < 3:
            return False, "Username must be at least 3 characters"
        if len(username) > 20:
            return False, "Username must be at most 20 characters"
        if not re.match(r'^[a-zA-Z0-9_]+$', username):
            return False, "Username can only contain letters, numbers, and underscores"
        return True, ""
    
    @staticmethod
    def sanitize_input(user_input: str) -> str:
        """Remove potentially dangerous characters"""
        dangerous_chars = ['<', '>', '"', "'", ';', '--', '/*', '*/']
        sanitized = user_input
        for char in dangerous_chars:
            sanitized = sanitized.replace(char, '')
        return sanitized.strip()


# Example 3: Rate limiting pattern (anti-DDoS)
class RateLimiter:
    """
    Implements rate limiting to prevent abuse and DDoS attacks.
    Copilot can generate these patterns when asked for "rate limiting".
    """
    
    def __init__(self, max_requests: int = 100, window_seconds: int = 60):
        self.max_requests = max_requests
        self.window_seconds = window_seconds
        self.requests = {}
    
    def is_allowed(self, client_id: str) -> bool:
        """Check if client is within rate limit"""
        now = datetime.now()
        
        if client_id not in self.requests:
            self.requests[client_id] = []
        
        # Remove old requests outside the window
        cutoff = now - timedelta(seconds=self.window_seconds)
        self.requests[client_id] = [
            req_time for req_time in self.requests[client_id]
            if req_time > cutoff
        ]
        
        if len(self.requests[client_id]) < self.max_requests:
            self.requests[client_id].append(now)
            return True
        
        return False


# Example 4: Secure password hashing (never store plain passwords!)
class PasswordManager:
    """
    Demonstrates secure password handling.
    NEVER generate passwords in Copilot prompts - use external tools.
    """
    
    @staticmethod
    def hash_password(password: str, salt: Optional[str] = None) -> tuple[str, str]:
        """
        Hash password with salt using SHA-256.
        In production, use bcrypt or argon2 instead.
        """
        if salt is None:
            salt = secrets.token_hex(16)
        
        # Hash password with salt
        pwd_hash = hashlib.pbkdf2_hmac(
            'sha256',
            password.encode('utf-8'),
            salt.encode('utf-8'),
            100000  # iterations
        ).hex()
        
        return pwd_hash, salt
    
    @staticmethod
    def verify_password(password: str, stored_hash: str, salt: str) -> bool:
        """Verify password against stored hash"""
        pwd_hash, _ = PasswordManager.hash_password(password, salt)
        return hmac.compare_digest(pwd_hash, stored_hash)


# Example 5: Secure decorator for authorization
def require_authentication(func: Callable) -> Callable:
    """
    Decorator to enforce authentication before allowing function execution.
    GitHub Copilot can generate decorators when given clear specifications.
    """
    @wraps(func)
    def wrapper(*args, **kwargs):
        # This would check actual authentication in production
        is_authenticated = kwargs.pop('authenticated', False)
        
        if not is_authenticated:
            raise PermissionError("Authentication required")
        
        return func(*args, **kwargs)
    
    return wrapper


def require_authorization(required_role: str):
    """Decorator to enforce role-based authorization"""
    def decorator(func: Callable) -> Callable:
        @wraps(func)
        def wrapper(*args, **kwargs):
            user_role = kwargs.pop('user_role', None)
            
            if user_role != required_role:
                raise PermissionError(f"Required role: {required_role}")
            
            return func(*args, **kwargs)
        
        return wrapper
    
    return decorator


# Example 6: Secure API endpoint pattern
class UserAPI:
    """
    Demonstrates security patterns for API endpoints.
    Includes input validation, error handling, and authorization.
    """
    
    def __init__(self):
        self.rate_limiter = RateLimiter(max_requests=50, window_seconds=60)
        self.users = {}
    
    def create_user(self, client_id: str, username: str, email: str, password: str) -> Dict:
        """
        Create new user with security checks.
        Demonstrates multiple security layers.
        """
        
        # Check rate limit
        if not self.rate_limiter.is_allowed(client_id):
            raise PermissionError("Rate limit exceeded")
        
        # Validate input
        is_valid, error = InputValidator.validate_username(username)
        if not is_valid:
            raise ValueError(f"Invalid username: {error}")
        
        if not InputValidator.validate_email(email):
            raise ValueError("Invalid email format")
        
        if len(password) < 8:
            raise ValueError("Password must be at least 8 characters")
        
        # Sanitize input
        username = InputValidator.sanitize_input(username)
        email = InputValidator.sanitize_input(email)
        
        # Hash password (NEVER store plaintext)
        pwd_hash, salt = PasswordManager.hash_password(password)
        
        # Store user (in memory for demo, use database in production)
        user_id = len(self.users) + 1
        self.users[user_id] = {
            'id': user_id,
            'username': username,
            'email': email,
            'password_hash': pwd_hash,
            'salt': salt,
            'created_at': datetime.now().isoformat()
        }
        
        # Return non-sensitive data
        return {
            'id': user_id,
            'username': username,
            'email': email,
            'created_at': self.users[user_id]['created_at']
        }
    
    @require_authentication
    def get_user(self, client_id: str, user_id: int, **kwargs) -> Dict:
        """Retrieve user (requires authentication)"""
        
        if not self.rate_limiter.is_allowed(client_id):
            raise PermissionError("Rate limit exceeded")
        
        if user_id not in self.users:
            raise ValueError("User not found")
        
        user = self.users[user_id]
        return {
            'id': user['id'],
            'username': user['username'],
            'email': user['email'],
            'created_at': user['created_at']
        }


# Demonstration
if __name__ == "__main__":
    print("=" * 70)
    print("GitHub Copilot Security Best Practices Demonstration")
    print("=" * 70)
    
    # Example 1: Credential Management
    print("\n1. Secure Credential Handling:")
    print(f"   Masked API Key: {SecureCredentialManager.mask_sensitive_value('sk_test_1234567890abcdef')}")
    
    # Example 2: Input Validation
    print("\n2. Input Validation:")
    test_emails = ["user@example.com", "invalid.email", "test@domain.co.uk"]
    for email in test_emails:
        is_valid = InputValidator.validate_email(email)
        print(f"   {email:25} -> {'Valid' if is_valid else 'Invalid'}")
    
    print("\n   Username Validation:")
    test_usernames = ["user123", "ab", "user@admin", "validuser_123"]
    for username in test_usernames:
        is_valid, error = InputValidator.validate_username(username)
        status = 'Valid' if is_valid else f'Invalid: {error}'
        print(f"   {username:20} -> {status}")
    
    # Example 3: Rate Limiting
    print("\n3. Rate Limiting (5 requests per 60 seconds):")
    limiter = RateLimiter(max_requests=5, window_seconds=60)
    for i in range(7):
        allowed = limiter.is_allowed("client_123")
        print(f"   Request {i+1}: {'Allowed' if allowed else 'Denied (rate limit)'}")
    
    # Example 4: Password Security
    print("\n4. Password Hashing:")
    password = "MySecurePassword123!"
    pwd_hash, salt = PasswordManager.hash_password(password)
    print(f"   Password: {password}")
    print(f"   Salt: {salt}")
    print(f"   Hash: {pwd_hash[:32]}...")
    print(f"   Verification: {PasswordManager.verify_password(password, pwd_hash, salt)}")
    print(f"   Wrong password: {PasswordManager.verify_password('WrongPassword', pwd_hash, salt)}")
    
    # Example 5: API with Security
    print("\n5. Secure API Operations:")
    api = UserAPI()
    
    # Create user
    try:
        user = api.create_user(
            "client_456",
            "john_doe",
            "john@example.com",
            "SecurePassword123"
        )
        print(f"   Created user: {user}")
    except Exception as e:
        print(f"   Error: {e}")
    
    # Attempt to create invalid user
    try:
        invalid_user = api.create_user(
            "client_456",
            "ab",  # Too short
            "invalid_email",
            "short"
        )
    except ValueError as e:
        print(f"   ✓ Validation caught error: {e}")
    
    # Get user (with authentication)
    try:
        user = api.get_user("client_456", 1, authenticated=True)
        print(f"   Retrieved user: {user}")
    except Exception as e:
        print(f"   Error: {e}")
    
    print("\n" + "=" * 70)
    print("Demonstration Complete!")
    print("=" * 70)
