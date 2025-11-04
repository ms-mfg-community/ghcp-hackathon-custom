using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Day 1.1 Demo: GitHub Copilot Class Generation
/// Demonstrates how GitHub Copilot can generate C# classes and methods.
/// </summary>

// Example 1: Product class with properties and methods
// Copilot suggestion: Generate common CRUD operations
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ProductService
{
    private List<Product> products = new List<Product>();

    public void CreateProduct(Product product)
    {
        products.Add(product);
    }

    public Product ReadProduct(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }

    public void UpdateProduct(Product product)
    {
        var existing = ReadProduct(product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Price = product.Price;
        }
    }

    public void DeleteProduct(int id)
    {
        var product = ReadProduct(id);
        if (product != null)
        {
            products.Remove(product);
        }
    }
}

// Example 2: ProductRepository for data access
// Copilot suggestion: Generate repository pattern
public class ProductRepository
{
    private List<Product> products = new List<Product>();

    public void Add(Product product)
    {
        products.Add(product);
    }

    public Product GetById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        return products;
    }

    public void Remove(int id)
    {
        var product = GetById(id);
        if (product != null)
        {
            products.Remove(product);
        }
    }
}