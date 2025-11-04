namespace calculator.blazor.Models;

/// <summary>
/// Represents the result of a calculation operation.
/// </summary>
public class CalculationResult
{
    public bool Success { get; set; }
    public double Value { get; set; }
    public string? ErrorMessage { get; set; }

    public CalculationResult(bool success, double value, string? errorMessage = null)
    {
        Success = success;
        Value = value;
        ErrorMessage = errorMessage;
    }
}
