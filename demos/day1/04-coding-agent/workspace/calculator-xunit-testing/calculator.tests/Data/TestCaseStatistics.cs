namespace calculator.tests.Data;

/// <summary>
/// Statistics about test cases in the database
/// </summary>
public class TestCaseStatistics
{
    /// <summary>
    /// Gets or sets the total number of test cases
    /// </summary>
    public int TotalTestCases { get; set; }

    /// <summary>
    /// Gets or sets the number of active test cases
    /// </summary>
    public int ActiveTestCases { get; set; }

    /// <summary>
    /// Gets or sets the number of inactive test cases
    /// </summary>
    public int InactiveTestCases { get; set; }

    /// <summary>
    /// Gets or sets the count of test cases by category
    /// </summary>
    public Dictionary<string, int> TestCasesByCategory { get; set; } = new();

    /// <summary>
    /// Gets or sets the count of test cases by operation
    /// </summary>
    public Dictionary<string, int> TestCasesByOperation { get; set; } = new();
}
