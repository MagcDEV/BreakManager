namespace Break.Contracts.Responses;

public class ItemResponse
{
    public int ItemId { get; set; }
    public required string ProductCode { get; set; }
    public required string Barcode { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public required string ProductCategory { get; set; }
    public int ReorderQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int QuantityInStock { get; set; }
    public int MinimumStockLevel { get; set; }
    public int MaximumStockLevel { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime LastUpdated { get; set; }
}
