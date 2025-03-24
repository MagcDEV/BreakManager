using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class SaleItem
{
    public int SaleItemId { get; set; }
    
    public int SaleId { get; set; }
    
    public int ItemId { get; set; }
    
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal LineTotal { get; set; }
    
    public required Sale Sale { get; set; }
    public Item? Item { get; set; }
}
