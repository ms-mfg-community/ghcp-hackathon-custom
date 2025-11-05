namespace calculator.web.Services;

/// <summary>
/// Implementation of calculation history service.
/// Maintains in-memory history scoped to the Blazor circuit.
/// </summary>
public class CalculationHistoryService : ICalculationHistoryService
{
    private readonly List<CalculationEntry> _history = new();
    private const int MaxHistoryEntries = 50;

    public event EventHandler? HistoryChanged;

    public int Count => _history.Count;

    public IReadOnlyList<CalculationEntry> GetHistory()
    {
        // Return most recent entries first
        return _history.AsReadOnly();
    }

    public void AddEntry(double firstOperand, double secondOperand, string operatorSymbol, double result)
    {
        var entry = new CalculationEntry
        {
            Timestamp = DateTime.Now,
            FirstOperand = firstOperand,
            SecondOperand = secondOperand,
            Operator = operatorSymbol,
            Result = result
        };

        _history.Insert(0, entry); // Add to beginning for most recent first

        // Remove oldest entry if we exceed max
        if (_history.Count > MaxHistoryEntries)
        {
            _history.RemoveAt(_history.Count - 1);
        }

        HistoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearHistory()
    {
        _history.Clear();
        HistoryChanged?.Invoke(this, EventArgs.Empty);
    }
}
