namespace calculator.web.Services;

/// <summary>
/// Represents a single calculation entry in the history.
/// </summary>
public class CalculationEntry
{
    /// <summary>
    /// Timestamp when the calculation was performed.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// First operand of the calculation.
    /// </summary>
    public double FirstOperand { get; set; }

    /// <summary>
    /// Second operand of the calculation.
    /// </summary>
    public double SecondOperand { get; set; }

    /// <summary>
    /// Operator used (+, -, *, /, %, ^).
    /// </summary>
    public string Operator { get; set; } = string.Empty;

    /// <summary>
    /// Result of the calculation.
    /// </summary>
    public double Result { get; set; }

    /// <summary>
    /// Formatted display string for the calculation.
    /// </summary>
    public string Display => $"{FirstOperand} {Operator} {SecondOperand} = {Result}";
}
