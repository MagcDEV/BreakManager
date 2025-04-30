using Break.Application.Database;
using Break.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Repositories;

public class ItemRepository(BreakAppDbContext dbContext) : IItemRepository
{
    public async Task<Item> AddItemAsync(Item item)
    {
        var addedItem = await dbContext.Items.AddAsync(item);
        await dbContext.SaveChangesAsync();
        return addedItem.Entity;
    }

    public async Task<bool> DeleteItemAsync(int itemId)
    {
        var item = await dbContext.Items.FindAsync(itemId);
        if (item != null)
        {
            dbContext.Items.Remove(item);
            await dbContext.SaveChangesAsync();
        }

        return item != null;
    }

    public async Task<Item?> GetItemAsync(int itemId)
    {
        return await dbContext.Items.FindAsync(itemId);
    }

    public async Task<Item?> GetItemByBarcodeAsync(string barcode)
    {
        // Query the database for an item matching the barcode.
        // Use FirstOrDefaultAsync as barcode should ideally be unique.
        // Use AsNoTracking for read performance.
        return await dbContext.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Barcode == barcode);
    }

    public IQueryable<Item> GetItemsQuery()
    {
        // Apply AsNoTracking for better read performance
        // Add default ordering for consistent pagination
        return dbContext.Items
            .AsNoTracking()
            .OrderBy(item => item.ProductName); // Or order by ItemId or another suitable property
    }

    public async Task<List<Item>> GetItemsByIdsAsync(IEnumerable<int> itemIds)
    {
        // Ensure itemIds is not null or empty before querying
        if (itemIds == null || !itemIds.Any())
        {
            return []; // Return empty list using collection expression (C# 12)
        }

        // Use Where + Contains to filter by IDs directly in the database
        // IMPORTANT: We need to track these items as their QuantityInStock will be modified.
        // So, DO NOT use AsNoTracking() here.
        return await dbContext.Items
            .Where(item => itemIds.Contains(item.ItemId))
            .ToListAsync();
    }

    public async Task<Item> UpdateItemAsync(Item item)
    {
        dbContext.Items.Update(item);
        await dbContext.SaveChangesAsync();
        return item;
    }
}
