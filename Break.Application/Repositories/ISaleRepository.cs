using Break.Application.Models;

namespace Break.Application.Repositories;

public interface ISaleRepository
{
    Task<Sale> SaveSaleAsync(Sale sale);
}
