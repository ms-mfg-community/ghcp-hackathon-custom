using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calculator.tests.Data;

/// <summary>
/// Represents a calculator test case stored in the SQLite database
/// </summary>
public class CalculatorTestCase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string TestName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public double FirstOperand { get; set; }

    public double SecondOperand { get; set; }

    [Required]
    [MaxLength(20)]
    public string Operation { get; set; } = string.Empty;

    public double ExpectedResult { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
