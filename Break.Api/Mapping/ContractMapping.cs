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

    public static IEnumerable<(int ItemId, int Quantity)> MapToSaleItems(
        this CreateSaleRequest request
    )
    {
        return request.Items.Select(item => (item.ItemId, item.Quantity));
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

    public static Item MapToItem(this UpdateItemRequest request)
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

    public static User MapToUser(this RegisterUserRequest request)
    {
        return new User
        {
            Username = request.Username,
            Email = request.Email,
            Roles = request.Roles
        };
    }

    public static IEnumerable<ItemResponse> MapToItemResponse(this IEnumerable<Item> items)
    {
        return items.Select(item => item.MapToItemResponse());
    }

    public static SaleResponse MapToSaleResponse(this Sale sale)
    {
        return new SaleResponse
        {
            SaleId = sale.SaleId,
            SaleDate = sale.SaleDate,
            SubTotal = sale.SubTotal,
            DiscountAmount = sale.DiscountAmount,
            Total = sale.Total,
            SaleItems = sale.SaleItems.Select(si => si.MapToSaleItemResponse()).ToList()
        };
    }

    public static SaleItemResponse MapToSaleItemResponse(this SaleItem saleItem)
    {
        return new SaleItemResponse
        {
            SaleItemId = saleItem.SaleItemId,
            ItemId = saleItem.ItemId,
            ProductName = saleItem.Item?.ProductName,
            Quantity = saleItem.Quantity,
            UnitPrice = saleItem.UnitPrice,
            LineTotal = saleItem.LineTotal
        };
    }

    public static IEnumerable<SaleResponse> MapToSaleResponse(this IEnumerable<Sale> sales)
    {
        return sales.Select(sale => sale.MapToSaleResponse());
    }
}
