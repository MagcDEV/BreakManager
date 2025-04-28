using Break.Application.Models;

namespace Break.Application.Services;

public interface IItemService
{
    Task<Item?> GetItemAsync(int itemId);
    Task<PagedList<Item>> GetItemsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Item> AddItemAsync(Item item);
    Task<Item?> UpdateItemAsync(int itemId, UpdateItemRequest request);
    Task<bool> DeleteItemAsync(int itemId);
}
