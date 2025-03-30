using Break.Application.Database;
using Break.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Repositories;

public class SaleRepository(BreakAppDbContext dbContext) : ISaleRepository
{
    public async Task<Sale> SaveSaleAsync(Sale sale)
    {
        dbContext.Sales.Add(sale);
        await dbContext.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale?> GetSaleByIdAsync(int saleId)
    {
        return await dbContext
            .Sales.Include(s => s.SaleItems)
            .ThenInclude(si => si.Item)
            .FirstOrDefaultAsync(s => s.SaleId == saleId);
    }

    public async Task<List<Sale>> GetAllSalesAsync()
    {
        return await dbContext
            .Sales.Include(s => s.SaleItems)
            .ThenInclude(si => si.Item)
            .ToListAsync();
    }

    public async Task<bool> DeleteSaleAsync(int saleId)
    {
        var sale = await dbContext.Sales.FindAsync(saleId);
        if (sale == null)
        {
            return false;
        }

        dbContext.Sales.Remove(sale);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
