using Break.Application.Models;

namespace Break.Application.Repositories;

public interface ISaleRepository
{
    Task<Sale> SaveSaleAsync(Sale sale);
    Task<Sale?> GetSaleByIdAsync(int saleId);
    Task<List<Sale>> GetAllSalesAsync();
    Task<bool> DeleteSaleAsync(int saleId);
    Task<Sale> UpdateSaleAsync(Sale sale);
}
