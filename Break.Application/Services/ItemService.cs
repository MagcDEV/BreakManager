using Break.Application.Models;
using Break.Application.Repositories;

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

    public Task<IEnumerable<Item>> GetItemsAsync()
    {
        return itemRepository.GetItemsAsync();
    }

    public Task<Item> UpdateItemAsync(Item item)
    {
        return itemRepository.UpdateItemAsync(item);
    }
}
