// Day 2.1 Demo: Product.cs
// Entity model generated with GitHub Copilot Agentic Mode

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAgentic.Models
{
    /// <summary>
    /// Represents a product in the inventory system.
    /// Entity Framework will map this to the Products table.
    /// </summary>
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public string Category { get; set; }

        [StringLength(50)]
        public string Sku { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Calculate if product is in low stock
        /// </summary>
        public bool IsLowStock(int threshold = 10) => Stock < threshold;

        /// <summary>
        /// Apply discount to product price
        /// </summary>
        public decimal GetDiscountedPrice(decimal discountPercent)
        {
            return Price * (1 - (discountPercent / 100));
        }
    }

    /// <summary>
    /// Data transfer object for Product API responses
    /// Decouples API contracts from entity model
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string Sku { get; set; }
        public decimal Rating { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static ProductDto FromProduct(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Sku = product.Sku,
                Rating = product.Rating,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }
    }

    /// <summary>
    /// Create/Update request DTO for products
    /// </summary>
    public class CreateUpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public string Category { get; set; }

        [StringLength(50)]
        public string Sku { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
