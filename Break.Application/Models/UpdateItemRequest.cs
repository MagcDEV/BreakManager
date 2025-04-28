namespace Break.Application.Models;

public class UpdateItemRequest
{
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
}
