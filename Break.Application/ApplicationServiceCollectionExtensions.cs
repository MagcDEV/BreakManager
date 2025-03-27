using Break.Application.Database;
using Break.Application.Repositories;
using Break.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Break.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<BreakAppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }
}
