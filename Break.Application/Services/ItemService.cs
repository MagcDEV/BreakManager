using Break.Application.Models;
using Break.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Services;

public class ItemService(IItemRepository itemRepository) : IItemService
{
    public Task<Item> AddItemAsync(Item item)
    {
        return itemRepository.AddItemAsync(item);
    }

    public Task<bool> DeleteItemAsync(int itemId)
    {
        return itemRepository.DeleteItemAsync(itemId);
    }

    public Task<Item?> GetItemAsync(int itemId)
    {
        return itemRepository.GetItemAsync(itemId);
    }

    public async Task<PagedList<Item>> GetItemsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        // 1. Get the base query from the repository
        var query = itemRepository.GetItemsQuery();

        // 2. Get the total count for pagination metadata (before skipping/taking)
        var totalCount = await query.CountAsync(cancellationToken);

        // 3. Apply pagination (Skip and Take)
        // Ensure pageNumber and pageSize are valid (could also be validated earlier)
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize; // Consider using constants


        var items = await query
            .Skip((pageNumber - 1) * pageSize) // Corrected skip logic
            .Take(pageSize)
            .ToListAsync(cancellationToken); // Execute the query

        // 4. Create the pagination metadata
        var metadata = new PaginationMetadata(
            CurrentPage: pageNumber,
            PageSize: items.Count, // Use actual count in case it's the last page
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize)
        );

        // 5. Return the PagedList container
        return new PagedList<Item>(items, metadata);
    }

    public async Task<Item?> UpdateItemAsync(int itemId, UpdateItemRequest request)
    {
        // 1. Fetch the existing item using the repository
        // Note: GetItemAsync might use FindAsync which doesn't support Include.
        // If you need related data, the repository might need a different Get method.
        // For simple updates, GetItemAsync is likely sufficient.
        var existingItem = await itemRepository.GetItemAsync(itemId);

        // 2. Check if the item exists
        if (existingItem is null)
        {
            return null; // Indicate not found
        }

        // 3. Apply updates from the request DTO to the existing entity
        //    (Manual mapping or use a library like AutoMapper/Mapster)
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

        // 4. Update the timestamp
        existingItem.LastUpdated = DateTime.UtcNow;

        // 5. Call the repository to persist changes
        //    The repository's UpdateItemAsync should handle saving the tracked entity.
        await itemRepository.UpdateItemAsync(existingItem);

        // 6. Return the updated item
        return existingItem;
    }
}
