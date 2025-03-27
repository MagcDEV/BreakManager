using Break.Application.Database;
using Break.Application.Repositories;
using Break.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Break.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        string connectionString,
        IConfiguration configuration
    )
    {
        services.AddDbContext<BreakAppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
