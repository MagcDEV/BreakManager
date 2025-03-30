using Break.Application.Models;

namespace Break.Application.Services;

public interface ISaleService
{
    Task<Sale> CreateSaleAsync(List<(int ItemId, int Quantity)> items, string? couponCode = null);
    Task<decimal> CalculateDiscountAsync(List<(int ItemId, int Quantity, decimal UnitPrice)> items, string? couponCode = null);
    Task<Sale?> GetSaleByIdAsync(int saleId);
    Task<List<Sale>> GetAllSalesAsync();
    Task<bool> DeleteSaleAsync(int saleId);
}
