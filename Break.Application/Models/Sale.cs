using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class Sale
{
    public int SaleId { get; set; }
    
    [Required]
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }
    
    public int? CustomerId { get; set; }
    
    [MaxLength(50)]
    public required string OrderStatus { get; set; }

    public ICollection<SaleItem>? SaleItems { get; set; }
}
