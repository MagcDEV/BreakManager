using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class Sale
{
    public int SaleId { get; set; }
    
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal SubTotal { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    public ICollection<AppliedOffer> AppliedOffers { get; set; } = new List<AppliedOffer>();
}
