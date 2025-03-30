namespace Break.Contracts.Responses;

public class SaleResponse
{
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Total { get; set; }
    public List<SaleItemResponse> SaleItems { get; set; } = new();
}
