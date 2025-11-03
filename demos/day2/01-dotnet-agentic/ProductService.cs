// Day 2.1 Demo: ProductService.cs
// Service layer generated with GitHub Copilot Agentic Mode

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetAgentic.Models;

namespace DotNetAgentic.Services
{
    /// <summary>
    /// Service interface for product operations
    /// Defines contract for business logic
    /// </summary>
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateUpdateProductDto dto);
        Task<ProductDto> UpdateProductAsync(int id, CreateUpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
        Task<bool> UpdateStockAsync(int productId, int quantity);
    }

    /// <summary>
    /// Product service implementation
    /// Contains business logic for product operations
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = _context.Products
                .Where(p => !p.DeletedAt.HasValue)
                .Select(p => ProductDto.FromProduct(p))
                .ToList();

            return await Task.FromResult(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id && !p.DeletedAt.HasValue);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            return await Task.FromResult(ProductDto.FromProduct(product));
        }

        public async Task<ProductDto> CreateProductAsync(CreateUpdateProductDto dto)
        {
            ValidateProductDto(dto);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Category = dto.Category,
                Sku = dto.Sku,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ProductDto.FromProduct(product);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, CreateUpdateProductDto dto)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id && !p.DeletedAt.HasValue);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            ValidateProductDto(dto);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Category = dto.Category;
            product.Sku = dto.Sku;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return ProductDto.FromProduct(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id && !p.DeletedAt.HasValue);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            product.DeletedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            var term = searchTerm.ToLower();
            var products = _context.Products
                .Where(p => !p.DeletedAt.HasValue &&
                            (p.Name.ToLower().Contains(term) ||
                             p.Description.ToLower().Contains(term) ||
                             p.Category.ToLower().Contains(term)))
                .Select(p => ProductDto.FromProduct(p))
                .ToList();

            return await Task.FromResult(products);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = _context.Products
                .Where(p => !p.DeletedAt.HasValue && p.Category == category)
                .Select(p => ProductDto.FromProduct(p))
                .ToList();

            return await Task.FromResult(products);
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId && !p.DeletedAt.HasValue);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found");

            product.Stock += quantity;

            if (product.Stock < 0)
                throw new InvalidOperationException("Insufficient stock");

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        private void ValidateProductDto(CreateUpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name is required");

            if (dto.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (string.IsNullOrWhiteSpace(dto.Category))
                throw new ArgumentException("Category is required");
        }
    }
}
