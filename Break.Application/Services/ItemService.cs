using Break.Application.Models;
using Break.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Services;

// Using primary constructor (C# 12+) for dependency injection
public class ItemService(IItemRepository itemRepository) : IItemService
{
    // Reverted: AddItemAsync now directly calls the repository with the provided Item object
    public Task<Item> AddItemAsync(Item item)
    {
        // The repository handles adding and saving the Item entity
        return itemRepository.AddItemAsync(item);
    }

    // Removed the private MapToItem helper method. Mapping will occur in the endpoint.
    // Removed the CreateItemAsync(CreateItemRequest request) method.

    public Task<bool> DeleteItemAsync(int itemId)
    {
        return itemRepository.DeleteItemAsync(itemId);
    }

    public Task<Item?> GetItemAsync(int itemId)
    {
        // Use AsNoTracking for read-only queries if GetItemAsync in repo doesn't already
        return itemRepository.GetItemAsync(itemId);
    }

    public Task<Item?> GetItemByBarcodeAsync(string barcode)
    {
        // Use AsNoTracking for read-only queries if GetItemByBarcodeAsync in repo doesn't already
        return itemRepository.GetItemByBarcodeAsync(barcode);
    }

    public async Task<PagedList<Item>> GetItemsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = itemRepository.GetItemsQuery(); // Assumes repo returns IQueryable
        var totalCount = await query.CountAsync(cancellationToken);

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        // Apply AsNoTracking in the service if the repository doesn't guarantee it
        var items = await query
            .AsNoTracking() // Good practice for read-only list queries
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var metadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: items.Count,
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize)
        );

        return new PagedList<Item>(items, metadata);
    }

    public async Task<Item?> UpdateItemAsync(int itemId, UpdateItemRequest request)
    {
        // Fetch potentially tracked entity for update
        var existingItem = await itemRepository.GetItemAsync(itemId);

        if (existingItem is null)
        {
            return null;
        }

        // Apply updates
        existingItem.ProductCode = request.ProductCode;
        existingItem.Barcode = request.Barcode;
        existingItem.ProductName = request.ProductName;
        existingItem.ProductDescription = request.ProductDescription;
        existingItem.ProductCategory = request.ProductCategory;
        existingItem.ReorderQuantity = request.ReorderQuantity;
        existingItem.UnitPrice = request.UnitPrice;
        existingItem.QuantityInStock = request.QuantityInStock;
        existingItem.MinimumStockLevel = request.MinimumStockLevel;
        existingItem.MaximumStockLevel = request.MaximumStockLevel;
        existingItem.LastUpdated = DateTime.UtcNow; // Update timestamp

        // Repository handles saving the tracked entity changes
        await itemRepository.UpdateItemAsync(existingItem);

        return existingItem;
    }
}
