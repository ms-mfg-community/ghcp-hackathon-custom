namespace calculator.web.Services;

/// <summary>
/// Service for managing calculation history.
/// </summary>
public interface ICalculationHistoryService
{
    /// <summary>
    /// Gets all calculation history entries.
    /// </summary>
    IReadOnlyList<CalculationEntry> GetHistory();

    /// <summary>
    /// Adds a new calculation to the history.
    /// </summary>
    void AddEntry(double firstOperand, double secondOperand, string operatorSymbol, double result);

    /// <summary>
    /// Clears all history entries.
    /// </summary>
    void ClearHistory();

    /// <summary>
    /// Gets the count of history entries.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Event raised when history changes.
    /// </summary>
    event EventHandler? HistoryChanged;
}
