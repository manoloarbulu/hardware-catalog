using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using HardwareCatalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HardwareCatalog.Infrastructure.Seeding;

/// <summary>
/// Service for seeding initial data into the database.
/// </summary>
public class DataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public DataSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        if (!await _dbContext.Products.AnyAsync())
        {
            var brands = SeedBrands();
            await _dbContext.Brands.AddRangeAsync(brands);
            await _dbContext.SaveChangesAsync();

            var products = SeedProducts(brands);
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();
        }

        if (!await _dbContext.Computers.AnyAsync())
        {
            var products = await _dbContext.Products
                .Where(product => product.Model == "i7-6700K" || product.Model == "GTX 1080" || product.Model == "DDR5-16GB" || product.Model == "WD-1TB-SSD" || product.Model == "500W" || product.Model == "USB-3.0")
                .ToListAsync();

            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Type = ComputerType.Desktop,
                Weight = 12.5m,
                WeightUnit = WeightUnit.Kilograms,
                Description = "Seeded workstation configuration",
                SerialNumber = "HW-DEMO-001",
                ManufactureDate = DateTime.UtcNow.Date,
                Manufacturer = "Hardware Catalog"
            };

            foreach (var product in products)
            {
                computer.ComputerProducts.Add(new ComputerProduct
                {
                    ComputerId = computer.Id,
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            await _dbContext.Computers.AddAsync(computer);
            await _dbContext.SaveChangesAsync();
        }
    }

    private static List<Brand> SeedBrands()
    {
        return new List<Brand>
        {
            new() { Id = Guid.NewGuid(), Name = "Intel" },
            new() { Id = Guid.NewGuid(), Name = "AMD" },
            new() { Id = Guid.NewGuid(), Name = "NVIDIA" },
            new() { Id = Guid.NewGuid(), Name = "Kingston" },
            new() { Id = Guid.NewGuid(), Name = "Western Digital" },
            new() { Id = Guid.NewGuid(), Name = "Seagate" },
            new() { Id = Guid.NewGuid(), Name = "Corsair" },
            new() { Id = Guid.NewGuid(), Name = "MSI" },
            new() { Id = Guid.NewGuid(), Name = "Dell" },
            new() { Id = Guid.NewGuid(), Name = "HP" },
            new() { Id = Guid.NewGuid(), Name = "Lenovo" },
            new() { Id = Guid.NewGuid(), Name = "IBM" }
        };
    }

    private static List<Product> SeedProducts(List<Brand> brands)
    {
        var intelBrand = brands.First(b => b.Name == "Intel");
        var amdBrand = brands.First(b => b.Name == "AMD");
        var nvidiaBrand = brands.First(b => b.Name == "NVIDIA");
        var kingstonBrand = brands.First(b => b.Name == "Kingston");
        var wdBrand = brands.First(b => b.Name == "Western Digital");
        var seagateBrand = brands.First(b => b.Name == "Seagate");
        var corsairBrand = brands.First(b => b.Name == "Corsair");
        var msiBrand = brands.First(b => b.Name == "MSI");

        var products = new List<Product>
        {
            // Processors - Intel
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "Intel® Core™ i5-6400", UnitOfMeasure = UnitOfMeasure.Units, BrandId = intelBrand.Id, Model = "i5-6400" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "Intel® Celeron™ N3050", UnitOfMeasure = UnitOfMeasure.Units, BrandId = intelBrand.Id, Model = "N3050" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "Intel Core i7-6700K 4GHz", UnitOfMeasure = UnitOfMeasure.Units, BrandId = intelBrand.Id, Model = "i7-6700K" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "Intel Core i7 Extreme Edition 3 GHz", UnitOfMeasure = UnitOfMeasure.Units, BrandId = intelBrand.Id, Model = "i7 Extreme" },

            // Processors - AMD
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "AMD FX 4300", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "FX-4300" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "AMD Athlon Quad-Core APU Athlon 5150", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "Athlon 5150" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "AMD FX 8-Core Black Edition FX-8350", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "FX-8350" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Processor, Name = "AMD FX 8-Core Black Edition FX-8370", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "FX-8370" },

            // Graphics Cards - NVIDIA
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "NVIDIA GeForce GTX 1080", UnitOfMeasure = UnitOfMeasure.Units, BrandId = nvidiaBrand.Id, Model = "GTX 1080" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "NVIDIA GeForce GTX 960", UnitOfMeasure = UnitOfMeasure.Units, BrandId = nvidiaBrand.Id, Model = "GTX 960" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "NVIDIA GeForce GTX 770", UnitOfMeasure = UnitOfMeasure.Units, BrandId = nvidiaBrand.Id, Model = "GTX 770" },

            // Graphics Cards - AMD
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "Radeon R7360", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "R7360" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "Radeon RX 480", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "RX 480" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "Radeon R9 380", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "R9 380" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.GraphicCard, Name = "AMD FirePro W4100", UnitOfMeasure = UnitOfMeasure.Units, BrandId = amdBrand.Id, Model = "FirePro W4100" },

            // Memory - Kingston
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Memory, Name = "Kingston 8 GB DDR5", UnitOfMeasure = UnitOfMeasure.GB, BrandId = kingstonBrand.Id, Model = "DDR5-8GB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Memory, Name = "Kingston 16 GB DDR5", UnitOfMeasure = UnitOfMeasure.GB, BrandId = kingstonBrand.Id, Model = "DDR5-16GB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Memory, Name = "Kingston 32 GB DDR5", UnitOfMeasure = UnitOfMeasure.GB, BrandId = kingstonBrand.Id, Model = "DDR5-32GB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Memory, Name = "Kingston 512 MB", UnitOfMeasure = UnitOfMeasure.MB, BrandId = kingstonBrand.Id, Model = "512MB" },

            // Storage - Western Digital
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Western Digital 1TB SSD", UnitOfMeasure = UnitOfMeasure.TB, BrandId = wdBrand.Id, Model = "WD-1TB-SSD" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Western Digital 2TB SSD", UnitOfMeasure = UnitOfMeasure.TB, BrandId = wdBrand.Id, Model = "WD-2TB-SSD" },

            // Storage - Seagate
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 2TB HDD Barracuda", UnitOfMeasure = UnitOfMeasure.TB, BrandId = seagateBrand.Id, Model = "Barracuda 2TB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 3TB HDD Barracuda", UnitOfMeasure = UnitOfMeasure.TB, BrandId = seagateBrand.Id, Model = "Barracuda 3TB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 4TB HDD Barracuda", UnitOfMeasure = UnitOfMeasure.TB, BrandId = seagateBrand.Id, Model = "Barracuda 4TB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 512GB SDD", UnitOfMeasure = UnitOfMeasure.GB, BrandId = seagateBrand.Id, Model = "SSD-512GB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 750GB SDD", UnitOfMeasure = UnitOfMeasure.GB, BrandId = seagateBrand.Id, Model = "SSD-750GB" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.Storage, Name = "Seagate 256GB SDD", UnitOfMeasure = UnitOfMeasure.GB, BrandId = seagateBrand.Id, Model = "SSD-256GB" },

            // Power Supplies - Corsair
            new() { Id = Guid.NewGuid(), Category = ProductCategory.PowerSupply, Name = "Corsair 500W PSU", UnitOfMeasure = UnitOfMeasure.Watts, BrandId = corsairBrand.Id, Model = "500W" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.PowerSupply, Name = "Corsair 508 W PSU", UnitOfMeasure = UnitOfMeasure.Watts, BrandId = corsairBrand.Id, Model = "508W" },

            // Power Supplies - MSI
            new() { Id = Guid.NewGuid(), Category = ProductCategory.PowerSupply, Name = "MSI 1000 W PSU", UnitOfMeasure = UnitOfMeasure.Watts, BrandId = msiBrand.Id, Model = "1000W" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.PowerSupply, Name = "MSI 450 W PSU", UnitOfMeasure = UnitOfMeasure.Watts, BrandId = msiBrand.Id, Model = "450W" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.PowerSupply, Name = "MSI 750 W PSU", UnitOfMeasure = UnitOfMeasure.Watts, BrandId = msiBrand.Id, Model = "750W" },

            // External Ports
            new() { Id = Guid.NewGuid(), Category = ProductCategory.ExternalPorts, Name = "USB 3.0", UnitOfMeasure = UnitOfMeasure.Units, BrandId = kingstonBrand.Id, Model = "USB-3.0" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.ExternalPorts, Name = "USB 2.0", UnitOfMeasure = UnitOfMeasure.Units, BrandId = kingstonBrand.Id, Model = "USB-2.0" },
            new() { Id = Guid.NewGuid(), Category = ProductCategory.ExternalPorts, Name = "USB C", UnitOfMeasure = UnitOfMeasure.Units, BrandId = kingstonBrand.Id, Model = "USB-C" }
        };

        return products;
    }
}
