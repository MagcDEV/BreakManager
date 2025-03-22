using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class SaleItem
{
    public int SaleItemId { get; set; }
    
    [Required]
    public int SaleId { get; set; }
    
    [Required]
    public int ItemId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }

    public required Sale Sale { get; set; }
    public required Item Item { get; set; }
}
