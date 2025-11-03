# Day 2.4: Unit Testing & TDD

## Objective

Master test-driven development with xUnit and GitHub Copilot:
- Test case generation
- Mock object creation
- Assertion patterns
- Test organization
- Coverage improvement

## TDD Workflow with Copilot

1. **Write Test First** - Describe what you're testing
2. **Ask Copilot** - "Generate xUnit tests for ProductService"
3. **Review Generated Tests** - Verify coverage and logic
4. **Implement Feature** - Run tests to guide implementation
5. **Refactor** - Improve code while tests stay green

## Test Generation Capabilities

Copilot can generate:
- ✅ Happy path tests
- ✅ Edge case tests
- ✅ Error scenario tests
- ✅ Mock setups with Moq
- ✅ Test data builders
- ✅ Assertion helpers
- ✅ Test cleanup (Dispose)

## Example Test Structure

```csharp
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetProductById_WithValidId_ReturnsProduct()
    {
        // Arrange
        var productId = 1;
        var expectedProduct = new Product { Id = productId, Name = "Test Product" };
        _mockRepository.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _service.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ThrowsException()
    {
        // Arrange
        var productId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(productId))
            .ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetProductByIdAsync(productId));
    }
}
```

## Running Tests

```powershell
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ProductServiceTests"

# Run with coverage
dotnet test /p:CollectCoverage=true
```
