using Break.Application.Models;
using Break.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Services;

public class SaleService(
    IOfferRepository offerRepository,
    ISaleRepository saleRepository,
    IItemRepository itemRepository
) : ISaleService
{
    public async Task<Sale> CreateSaleAsync(
        List<(int ItemId, int Quantity)> items,
        string? couponCode = null
    )
    {
        // Validate input
        if (items == null || !items.Any())
            throw new ArgumentException("Sale must contain at least one item", nameof(items));

        // Load item details from database
        var itemIds = items.Select(i => i.ItemId).ToList();
        var dbItems = await itemRepository.GetItemsAsync();
        dbItems = dbItems.Where(i => itemIds.Contains(i.ItemId)).ToList();

        if (dbItems.Count() != itemIds.Distinct().Count())
            throw new ArgumentException("One or more items not found");

        // Check if there's enough stock for all items
        foreach (var (itemId, quantity) in items)
        {
            var dbItem = dbItems.First(i => i.ItemId == itemId);
            if (dbItem.QuantityInStock < quantity)
                throw new InvalidOperationException(
                    $"Not enough stock for item {dbItem.ProductName}. Available: {dbItem.QuantityInStock}, Requested: {quantity}"
                );
        }
        // Create new sale
        var sale = new Sale { SaleDate = DateTime.UtcNow };

        // Add items to sale
        foreach (var (itemId, quantity) in items)
        {
            var dbItem = dbItems.First(i => i.ItemId == itemId);
            var saleItem = new SaleItem
            {
                Sale = sale,
                ItemId = itemId,
                Item = dbItem,
                Quantity = quantity,
                UnitPrice = dbItem.UnitPrice,
                LineTotal = dbItem.UnitPrice * quantity,
            };

            sale.SaleItems.Add(saleItem);

            // Update inventory quantity
            dbItem.QuantityInStock -= quantity;
            dbItem.LastUpdated = DateTime.UtcNow;
        }

        // Calculate subtotal
        sale.SubTotal = sale.SaleItems.Sum(si => si.LineTotal);

        // Apply offers and calculate discounts
        await ApplyOffersAsync(sale, couponCode);

        // Calculate final total
        sale.Total = sale.SubTotal - sale.DiscountAmount;

        // Save updated items to database
        foreach (var item in dbItems)
        {
            await itemRepository.UpdateItemAsync(item);
        }

        // Save to database
        await saleRepository.SaveSaleAsync(sale);

        return sale;
    }

    public async Task<Sale?> GetSaleByIdAsync(int saleId)
    {
        return await saleRepository.GetSaleByIdAsync(saleId);
    }

    public async Task<List<Sale>> GetAllSalesAsync()
    {
        return await saleRepository.GetAllSalesAsync();
    }

    public async Task<bool> DeleteSaleAsync(int saleId)
    {
        return await saleRepository.DeleteSaleAsync(saleId);
    }

    public async Task<decimal> CalculateDiscountAsync(
        List<(int ItemId, int Quantity, decimal UnitPrice)> items,
        string? couponCode = null
    )
    {
        if (items == null || !items.Any())
            return 0;

        // Create temporary sale object to evaluate discounts without saving
        var sale = new Sale();

        foreach (var (itemId, quantity, unitPrice) in items)
        {
            var saleItem = new SaleItem
            {
                Sale = sale,
                ItemId = itemId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                LineTotal = unitPrice * quantity,
            };

            sale.SaleItems.Add(saleItem);
        }

        sale.SubTotal = sale.SaleItems.Sum(si => si.LineTotal);

        // Apply offers to calculate potential discount
        await ApplyOffersAsync(sale, couponCode);

        return sale.DiscountAmount;
    }

    private async Task ApplyOffersAsync(Sale sale, string? couponCode)
    {
        // Get all active offers
        var activeOffers = await offerRepository.GetActiveOffersAsync();

        // Filter by coupon code if provided
        if (!string.IsNullOrEmpty(couponCode))
        {
            activeOffers = activeOffers.Where(o => o.CouponCode == couponCode).ToList();
        }

        foreach (var offer in activeOffers)
        {
            if (IsOfferEligible(offer, sale.SaleItems.ToList(), sale.SubTotal))
            {
                var discountAmount = CalculateDiscount(
                    offer,
                    sale.SaleItems.ToList(),
                    sale.SubTotal
                );

                if (discountAmount > 0)
                {
                    sale.AppliedOffers.Add(
                        new AppliedOffer
                        {
                            Sale = sale,
                            OfferId = offer.OfferId,
                            Offer = offer,
                            DiscountAmount = discountAmount,
                        }
                    );

                    sale.DiscountAmount += discountAmount;
                }
            }
        }
    }

    public async Task<Sale?> ConfirmSaleAsync(int saleId)
    {
        var sale = await saleRepository.GetSaleByIdAsync(saleId);
        if (sale == null)
            return null;

        // Only draft sales can be confirmed
        if (sale.Status != SaleStatus.Draft)
            throw new InvalidOperationException($"Cannot confirm sale with status {sale.Status}");

        // Verify stock availability again before confirming
        foreach (var saleItem in sale.SaleItems)
        {
            var item = await itemRepository.GetItemAsync(saleItem.ItemId);
            if (item == null || item.QuantityInStock < saleItem.Quantity)
                throw new InvalidOperationException(
                    $"Not enough stock for item {item?.ProductName ?? saleItem.ItemId.ToString()}. Available: {item?.QuantityInStock ?? 0}, Requested: {saleItem.Quantity}"
                );
        }

        // Update sale status
        sale.Status = SaleStatus.Confirmed;
        await saleRepository.UpdateSaleAsync(sale);
        return sale;
    }

    public async Task<Sale?> CancelSaleAsync(int saleId)
    {
        var sale = await saleRepository.GetSaleByIdAsync(saleId);
        if (sale == null)
            return null;

        // If the sale is already confirmed, we need to return items to inventory
        if (sale.Status == SaleStatus.Confirmed)
        {
            foreach (var saleItem in sale.SaleItems)
            {
                var item = await itemRepository.GetItemAsync(saleItem.ItemId);
                if (item != null)
                {
                    item.QuantityInStock += saleItem.Quantity;
                    await itemRepository.UpdateItemAsync(item);
                }
            }
        }

        // Update sale status
        sale.Status = SaleStatus.Canceled;
        await saleRepository.UpdateSaleAsync(sale);
        return sale;
    }


    private bool IsOfferEligible(Offer offer, List<SaleItem> saleItems, decimal subtotal)
    {
        if (offer.OfferConditions == null || !offer.OfferConditions.Any())
            return true; // No conditions means always eligible

        return offer.OfferConditions.All(condition => IsSatisfied(condition, saleItems, subtotal));
    }

    private bool IsSatisfied(OfferCondition condition, List<SaleItem> saleItems, decimal subtotal)
    {
        return condition.ConditionType switch
        {
            ConditionType.TotalAmount => subtotal >= (condition.MinimumAmount ?? 0)
                && (condition.MaximumAmount == null || subtotal <= condition.MaximumAmount),

            ConditionType.ItemQuantity => condition.ItemId.HasValue
                && saleItems.Any(si =>
                    si.ItemId == condition.ItemId
                    && si.Quantity >= (condition.MinimumQuantity ?? 0)
                    && (
                        condition.MaximumQuantity == null
                        || si.Quantity <= condition.MaximumQuantity
                    )
                ),

            _ => false,
        };
    }

    private decimal CalculateDiscount(Offer offer, List<SaleItem> saleItems, decimal subtotal)
    {
        return offer.DiscountType switch
        {
            DiscountType.Percentage => subtotal * (offer.DiscountValue / 100m),
            DiscountType.FixedAmount => offer.DiscountValue,
            _ => 0,
        };
    }
}
