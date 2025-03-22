using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Break.Application.Models;

public class Offer
{
    public int OfferId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string OfferName { get; set; }
    
    [MaxLength(2000)]
    public required string OfferDescription { get; set; }
    
    [Required]
    public DiscountType DiscountType { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountValue { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    [MaxLength(50)]
    public string? CouponCode { get; set; }

    public ICollection<OfferCondition>? OfferConditions { get; set; }
    public ICollection<Item>? Items { get; set; }
}
