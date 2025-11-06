namespace calculator.tests.Data;

/// <summary>
/// Custom exception for validation errors
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ValidationException class
    /// </summary>
    /// <param name="message">The error message</param>
    public ValidationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with an inner exception
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
