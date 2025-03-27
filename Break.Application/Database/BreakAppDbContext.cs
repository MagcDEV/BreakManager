using Break.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Break.Application.Database;

public class BreakAppDbContext : DbContext
{
    public BreakAppDbContext(DbContextOptions<BreakAppDbContext> options)
        : base(options) { }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<OfferCondition> OfferConditions => Set<OfferCondition>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Item configurations
        modelBuilder.Entity<Item>().HasIndex(i => i.ProductCode).IsUnique();

        modelBuilder.Entity<Item>().HasIndex(i => i.Barcode).IsUnique();

        // Sale relationships
        modelBuilder
            .Entity<Sale>()
            .HasMany(s => s.SaleItems)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Offer relationships
        modelBuilder
            .Entity<Offer>()
            .HasMany(o => o.OfferConditions)
            .WithOne(oc => oc.Offer)
            .HasForeignKey(oc => oc.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-many between Offer and Item
        modelBuilder
            .Entity<Offer>()
            .HasMany(o => o.Items)
            .WithMany(i => i.Offers)
            .UsingEntity(j => j.ToTable("OfferItems"));

        // OfferCondition relationships
        modelBuilder
            .Entity<OfferCondition>()
            .HasOne(oc => oc.Item)
            .WithMany()
            .HasForeignKey(oc => oc.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // User configurations
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.HasIndex(e => e.Username).IsUnique();

            entity.HasIndex(e => e.Email).IsUnique();

            entity
                .Property(e => e.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList())
                );
        });
    }
}
