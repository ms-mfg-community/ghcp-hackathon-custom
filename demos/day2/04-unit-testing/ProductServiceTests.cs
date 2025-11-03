// Day 2.4 Demo: ProductServiceTests.cs
// Unit tests generated with GitHub Copilot for xUnit

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DotNetAgentic.Models;
using DotNetAgentic.Services;

namespace DotNetAgentic.Tests
{
    /// <summary>
    /// Unit tests for ProductService
    /// Demonstrates TDD patterns with xUnit and Moq
    /// </summary>
    public class ProductServiceTests : IDisposable
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _service = new ProductService(_mockContext.Object);
        }

        public void Dispose()
        {
            _mockContext?.Reset();
        }

        #region GetAllProductsAsync Tests

        [Fact]
        public async Task GetAllProductsAsync_WithValidData_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.00m, Stock = 5 },
                new Product { Id = 2, Name = "Product2", Price = 20.00m, Stock = 10 }
            };
            var mockProducts = products.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllProductsAsync_WithDeletedProducts_ExcludesDeleted()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.00m },
                new Product { Id = 2, Name = "Product2", Price = 20.00m, DeletedAt = DateTime.UtcNow }
            };
            var mockProducts = products.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        #endregion

        #region GetProductByIdAsync Tests

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product1", Price = 10.00m };
            var mockProducts = new List<Product> { product }
                .AsQueryable()
                .BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act
            var result = await _service.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Product1", result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var mockProducts = new List<Product>()
                .AsQueryable()
                .BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetProductByIdAsync(999));
        }

        [Fact]
        public async Task GetProductByIdAsync_WithDeletedProduct_ThrowsException()
        {
            // Arrange
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Product1",
                DeletedAt = DateTime.UtcNow
            };
            var mockProducts = new List<Product> { product }
                .AsQueryable()
                .BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetProductByIdAsync(productId));
        }

        #endregion

        #region CreateProductAsync Tests

        [Fact]
        public async Task CreateProductAsync_WithValidData_CreatesProduct()
        {
            // Arrange
            var dto = new CreateUpdateProductDto
            {
                Name = "New Product",
                Price = 50.00m,
                Stock = 10,
                Category = "Electronics",
                Sku = "SKU001"
            };
            _mockContext.Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Act
            var result = await _service.CreateProductAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            Assert.Equal(50.00m, result.Price);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithEmptyName_ThrowsException()
        {
            // Arrange
            var dto = new CreateUpdateProductDto
            {
                Name = "",
                Price = 50.00m,
                Stock = 10,
                Category = "Electronics"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateProductAsync(dto));
        }

        [Fact]
        public async Task CreateProductAsync_WithNegativePrice_ThrowsException()
        {
            // Arrange
            var dto = new CreateUpdateProductDto
            {
                Name = "Product",
                Price = -10.00m,
                Stock = 10,
                Category = "Electronics"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => _service.CreateProductAsync(dto));
        }

        #endregion

        #region UpdateStockAsync Tests

        [Fact]
        public async Task UpdateStockAsync_WithPositiveQuantity_IncreasesStock()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Stock = 10 };
            var mockProducts = new List<Product> { product }
                .AsQueryable()
                .BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Act
            await _service.UpdateStockAsync(productId, 5);

            // Assert
            Assert.Equal(15, product.Stock);
        }

        [Fact]
        public async Task UpdateStockAsync_WithInsufficientStock_ThrowsException()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Stock = 5 };
            var mockProducts = new List<Product> { product }
                .AsQueryable()
                .BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.UpdateStockAsync(productId, -10));
        }

        #endregion

        #region SearchProductsAsync Tests

        [Fact]
        public async Task SearchProductsAsync_WithValidSearchTerm_ReturnsMatchingProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop Computer", Category = "Electronics" },
                new Product { Id = 2, Name = "Desktop PC", Category = "Electronics" },
                new Product { Id = 3, Name = "Office Chair", Category = "Furniture" }
            };
            var mockProducts = products.AsQueryable().BuildMockDbSet();
            _mockContext.Setup(c => c.Products).Returns(mockProducts.Object);

            // Act
            var result = await _service.SearchProductsAsync("Computer");

            // Assert
            Assert.Single(result);
            Assert.Equal("Laptop Computer", result.First().Name);
        }

        #endregion
    }

    /// <summary>
    /// Test helper for building mock DbSets
    /// </summary>
    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> BuildMockDbSet<T>(
            this IQueryable<T> queryable) where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new AsyncEnumerator<T>(queryable.GetEnumerator()));
            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => queryable.GetEnumerator());

            return mockDbSet;
        }
    }

    /// <summary>
    /// Async enumerator for DbSet mocking
    /// </summary>
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public async ValueTask<bool> MoveNextAsync()
        {
            return _inner.MoveNext();
        }

        public async ValueTask DisposeAsync()
        {
            _inner.Dispose();
        }
    }
}
