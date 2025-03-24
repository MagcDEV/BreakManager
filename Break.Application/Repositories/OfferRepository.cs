using Break.Application.Database;
using Break.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Repositories;

public class OfferRepository(BreakAppDbContext dbContext) : IOfferRepository
{
    public async Task<IEnumerable<Offer>> GetActiveOffersAsync()
    {
        var currentDate = DateTime.UtcNow;
        var offers = await dbContext.Offers
            .Include(o => o.OfferConditions)
            .Where(o => o.IsActive && o.StartDate <= currentDate && o.EndDate >= currentDate)
            .ToListAsync();
            
        // Load items related to offer conditions explicitly if needed
        foreach (var offer in offers)
        {
            await dbContext.Entry(offer).Collection(o => o.OfferConditions!).Query().Include(oc => oc.Item).LoadAsync();
        }
            
        return offers;
    }

    public async Task<Offer?> GetOfferAsync(int offerId)
    {
        return await dbContext
            .Offers.Include(o => o.OfferConditions)
            .FirstOrDefaultAsync(o => o.OfferId == offerId);
    }
}
