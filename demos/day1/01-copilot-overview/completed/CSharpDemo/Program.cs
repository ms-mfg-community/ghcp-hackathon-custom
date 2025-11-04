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
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product()
    {
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Calculate total inventory value.
    /// Copilot recognizes the pattern and suggests the formula.
    /// </summary>
    public decimal GetInventoryValue()
    {
        return Price * Quantity;
    }

    /// <summary>
    /// Apply discount percentage to price.
    /// </summary>
    public void ApplyDiscount(decimal discountPercent)
    {
        Price = Price * (1 - discountPercent / 100);
    }
}

// Example 2: ProductRepository for data access
// Copilot suggestion: Generate repository pattern
public class ProductRepository
{
    private List<Product> _products = new();

    public void Add(Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        _products.Add(product);
    }

    public Product GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        return _products.ToList();
    }

    public void Update(Product product)
    {
        var existing = GetById(product.Id);
        if (existing != null)
        {
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Quantity = product.Quantity;
        }
    }

    public void Delete(int id)
    {
        var product = GetById(id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }
}

// Example 3: Order class demonstrating Copilot pattern recognition
public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Calculate order total - Copilot suggests LINQ pattern
    /// </summary>
    public decimal GetTotal()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }

    /// <summary>
    /// Apply tax calculation
    /// </summary>
    public decimal GetTotalWithTax(decimal taxRate)
    {
        return GetTotal() * (1 + taxRate);
    }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

// Example 4: Utility class with extension methods
// Copilot suggestion: Recognize common patterns and generate extensions
public static class ProductExtensions
{
    public static IEnumerable<Product> FilterByPriceRange(
        this IEnumerable<Product> products,
        decimal minPrice,
        decimal maxPrice)
    {
        return products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
    }

    public static IEnumerable<Product> SortByName(this IEnumerable<Product> products)
    {
        return products.OrderBy(p => p.Name);
    }

    public static bool IsLowStock(this Product product, int threshold = 10)
    {
        return product.Quantity < threshold;
    }
}

// Main demonstration
public class Program
{
    public static void Main()
    {
        Console.WriteLine("=".PadRight(60, '='));
        Console.WriteLine("GitHub Copilot Class Generation Demonstration");
        Console.WriteLine("=".PadRight(60, '='));

        // Create repository
        var repository = new ProductRepository();

        // Add products - Copilot assists with object initialization
        var products = new[]
        {
            new Product { Name = "Laptop", Price = 999.99m, Quantity = 5 },
            new Product { Name = "Mouse", Price = 29.99m, Quantity = 50 },
            new Product { Name = "Keyboard", Price = 79.99m, Quantity = 30 },
            new Product { Name = "Monitor", Price = 299.99m, Quantity = 2 }
        };

        foreach (var product in products)
        {
            repository.Add(product);
        }

        // Display products
        Console.WriteLine("\n1. All Products:");
        foreach (var product in repository.GetAll())
        {
            Console.WriteLine($"   {product.Name:20} Price: ${product.Price:10.2f} " +
                            $"Qty: {product.Quantity:5} Value: ${product.GetInventoryValue():12.2f}");
        }

        // Filter products by price range
        Console.WriteLine("\n2. Products Under $100:");
        var affordable = repository.GetAll()
            .FilterByPriceRange(0, 100)
            .SortByName();
        foreach (var product in affordable)
        {
            Console.WriteLine($"   {product.Name:20} ${product.Price:10.2f}");
        }

        // Check low stock items
        Console.WriteLine("\n3. Low Stock Items (< 10 units):");
        var lowStock = repository.GetAll().Where(p => p.IsLowStock());
        foreach (var product in lowStock)
        {
            Console.WriteLine($"   {product.Name:20} Quantity: {product.Quantity}");
        }

        // Apply discount - Copilot assists with business logic
        Console.WriteLine("\n4. Applying 20% discount to Laptop:");
        var laptop = repository.GetById(1);
        if (laptop != null)
        {
            Console.WriteLine($"   Before: ${laptop.Price:10.2f}");
            laptop.ApplyDiscount(20);
            Console.WriteLine($"   After:  ${laptop.Price:10.2f}");
        }

        // Create an order - Copilot recognizes related objects
        Console.WriteLine("\n5. Create Order:");
        var order = new Order
        {
            Id = 1001,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Processing,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, ProductName = "Laptop", Price = 999.99m, Quantity = 1 },
                new OrderItem { ProductId = 2, ProductName = "Mouse", Price = 29.99m, Quantity = 2 }
            }
        };

        Console.WriteLine($"   Order #{order.Id}");
        Console.WriteLine($"   Subtotal:        ${order.GetTotal():10.2f}");
        Console.WriteLine($"   With 10% Tax:    ${order.GetTotalWithTax(0.10m):10.2f}");

        Console.WriteLine("\n" + "=".PadRight(60, '='));
        Console.WriteLine("Demonstration Complete!");
        Console.WriteLine("=".PadRight(60, '='));
    }
}
