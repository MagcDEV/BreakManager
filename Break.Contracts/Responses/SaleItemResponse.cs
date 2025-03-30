namespace Break.Contracts.Responses;

public class SaleItemResponse
{
    public int SaleItemId { get; set; }
    public int ItemId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
