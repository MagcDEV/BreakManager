using Break.Application.Database;
using Break.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Repositories;

public class ItemRepository(BreakAppDbContext dbContext) : IItemRepository
{
    public async Task<bool> AddItemAsync(Item item)
    {
        var addedItem = await dbContext.Items.AddAsync(item);
        await dbContext.SaveChangesAsync();
        return addedItem.State == EntityState.Added;
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

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        return await dbContext.Items.ToListAsync();
    }

    public async Task<Item> UpdateItemAsync(Item item)
    {
        dbContext.Items.Update(item);
        await dbContext.SaveChangesAsync();
        return item;
    }
}
