using Break.Application.Models;

namespace Break.Application.Repositories;

public interface IOfferRepository
{
    Task<IEnumerable<Offer>> GetActiveOffersAsync();
    Task<Offer?> GetOfferAsync(int offerId);
    Task<Offer> AddOfferAsync(Offer offer);
    Task<Offer> UpdateOfferAsync(Offer offer);
    Task<bool> DeleteOfferAsync(int offerId);
}
