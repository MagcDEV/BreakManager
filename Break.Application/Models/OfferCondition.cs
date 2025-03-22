using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class OfferCondition
{
    public int OfferConditionId { get; set; }
    
    [Required]
    public int OfferId { get; set; }
    
    [Required]
    public ConditionType ConditionType { get; set; }
    
    public int? ItemId { get; set; }
    public int? MinimumQuantity { get; set; }
    public int? MaximumQuantity { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal? MinimumAmount { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal? MaximumAmount { get; set; }

    public required Offer Offer { get; set; }
    public Item? Item { get; set; }
}
