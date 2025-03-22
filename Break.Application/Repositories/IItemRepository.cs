using Break.Application.Models;

namespace Break.Application.Repositories;

public interface IItemRepository
{
    Task<Item?> GetItemAsync(int itemId);
    Task<IEnumerable<Item>> GetItemsAsync();
    Task<Item> AddItemAsync(Item item);
    Task<Item> UpdateItemAsync(Item item);
    Task<bool> DeleteItemAsync(int itemId);
}
