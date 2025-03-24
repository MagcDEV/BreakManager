using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class AppliedOffer
{
    public int AppliedOfferId { get; set; }
    public int SaleId { get; set; }
    public int OfferId { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }
    
    public required Sale Sale { get; set; }
    public required Offer Offer { get; set; }
}
