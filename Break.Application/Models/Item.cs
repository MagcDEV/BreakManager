using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class Item
{
    public int ItemId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string ProductCode { get; set; }
    
    [MaxLength(100)]
    public required string Barcode { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string ProductName { get; set; }
    
    [MaxLength(2000)]
    public required string ProductDescription { get; set; }
    
    [MaxLength(100)]
    public required string ProductCategory { get; set; }
    
    public int ReorderQuantity { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public int QuantityInStock { get; set; }
    
    public int MinimumStockLevel { get; set; }
    public int MaximumStockLevel { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public ICollection<SaleItem>? SaleItems { get; set; }
    public ICollection<Offer>? Offers { get; set; }
}
