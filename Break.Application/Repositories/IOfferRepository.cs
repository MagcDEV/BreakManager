using Break.Application.Models;

namespace Break.Application.Repositories;

public interface IOfferRepository
{
    Task<IEnumerable<Offer>> GetActiveOffersAsync();
    Task<Offer?> GetOfferAsync(int offerId);
}
