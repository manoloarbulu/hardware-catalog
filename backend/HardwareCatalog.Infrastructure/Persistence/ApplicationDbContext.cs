using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HardwareCatalog.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core DbContext for the Hardware Catalog.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Computer> Computers => Set<Computer>();
    public DbSet<ComputerProduct> ComputerProducts => Set<ComputerProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Brand entity
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasMany(e => e.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.UnitOfMeasure).IsRequired();
            entity.HasOne(e => e.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.ComputerProducts)
                .WithOne(cp => cp.Product)
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Computer entity
        modelBuilder.Entity<Computer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreationDate).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Weight).IsRequired().HasPrecision(10, 2);
            entity.Property(e => e.WeightUnit).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.SerialNumber).HasMaxLength(200);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.HasMany(e => e.ComputerProducts)
                .WithOne(cp => cp.Computer)
                .HasForeignKey(cp => cp.ComputerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ComputerProduct (junction) entity
        modelBuilder.Entity<ComputerProduct>(entity =>
        {
            entity.HasKey(e => new { e.ComputerId, e.ProductId });
            entity.Property(e => e.Quantity).IsRequired();
            entity.HasOne(e => e.Computer)
                .WithMany(c => c.ComputerProducts)
                .HasForeignKey(e => e.ComputerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Product)
                .WithMany(p => p.ComputerProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
