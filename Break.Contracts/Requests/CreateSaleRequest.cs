using System.ComponentModel.DataAnnotations;

namespace Break.Contracts.Requests;

public class CreateSaleRequest
{
    [Required]
    public List<SaleItemRequest> Items { get; set; } = new();

    public string? CouponCode { get; set; }
}

public class SaleItemRequest
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}
