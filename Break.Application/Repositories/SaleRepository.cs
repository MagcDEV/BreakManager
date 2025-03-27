using Break.Application.Database;
using Break.Application.Models;

namespace Break.Application.Repositories;

public class SaleRepository(BreakAppDbContext dbContext) : ISaleRepository
{
    public async Task<Sale> SaveSaleAsync(Sale sale)
    {
        dbContext.Sales.Add(sale);
        await dbContext.SaveChangesAsync();
        return sale;
    }
}
