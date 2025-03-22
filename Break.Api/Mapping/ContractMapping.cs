using Break.Application.Models;
using Break.Contracts.Requests;
using Break.Contracts.Responses;

namespace Break.Api.Mapping;

public static class ContractMapping
{
    public static Item MapToItem(this CreateItemRequest request)
    {
        return new Item
        {
            ProductCode = request.ProductCode,
            Barcode = request.Barcode,
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            ProductCategory = request.ProductCategory,
            ReorderQuantity = request.ReorderQuantity,
            UnitPrice = request.UnitPrice,
            QuantityInStock = request.QuantityInStock,
            MinimumStockLevel = request.MinimumStockLevel,
            MaximumStockLevel = request.MaximumStockLevel,
            DateAdded = request.DateAdded,
            LastUpdated = request.LastUpdated,
        };
    }

    public static ItemResponse MapToItemResponse(this Item item)
    {
        return new ItemResponse
        {
            ItemId = item.ItemId,
            ProductCode = item.ProductCode,
            Barcode = item.Barcode,
            ProductName = item.ProductName,
            ProductDescription = item.ProductDescription,
            ProductCategory = item.ProductCategory,
            ReorderQuantity = item.ReorderQuantity,
            UnitPrice = item.UnitPrice,
            QuantityInStock = item.QuantityInStock,
            MinimumStockLevel = item.MinimumStockLevel,
            MaximumStockLevel = item.MaximumStockLevel,
            DateAdded = item.DateAdded,
            LastUpdated = item.LastUpdated,
        };
    }
}
