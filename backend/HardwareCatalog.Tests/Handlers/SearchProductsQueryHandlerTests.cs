using FluentAssertions;
using HardwareCatalog.Application.Queries;
using HardwareCatalog.Application.Handlers;
using HardwareCatalog.Domain.Enums;
using HardwareCatalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HardwareCatalog.Tests.Handlers;

public class SearchProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_WithKeywordMatch_ShouldReturnMatchingProducts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"SearchProductsTest_{Guid.NewGuid()}")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            // Seed test data
            var brand = new HardwareCatalog.Domain.Entities.Brand { Id = Guid.NewGuid(), Name = "TestBrand" };
            var product1 = new HardwareCatalog.Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "16GB DDR5 Memory",
                Category = ProductCategory.Memory,
                UnitOfMeasure = UnitOfMeasure.GB,
                Model = "DDR5-4800",
                BrandId = brand.Id
            };
            var product2 = new HardwareCatalog.Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Intel Processor",
                Category = ProductCategory.Processor,
                UnitOfMeasure = UnitOfMeasure.Units,
                Model = "i7-13700K",
                BrandId = brand.Id
            };

            context.Brands.Add(brand);
            context.Products.Add(product1);
            context.Products.Add(product2);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var handler = new SearchProductsQueryHandler(context);
            var query = new SearchProductsQuery { Query = "memory" };
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Contain("Memory");
        }
    }

    [Fact]
    public async Task Handle_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"SearchProductsTestNoMatches_{Guid.NewGuid()}")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var brand = new HardwareCatalog.Domain.Entities.Brand { Id = Guid.NewGuid(), Name = "TestBrand" };
            var product = new HardwareCatalog.Domain.Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Category = ProductCategory.Memory,
                UnitOfMeasure = UnitOfMeasure.GB,
                Model = "Model",
                BrandId = brand.Id
            };

            context.Brands.Add(brand);
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var handler = new SearchProductsQueryHandler(context);
            var query = new SearchProductsQuery { Query = "nonexistent" };
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task Handle_WithStorageCapacityComparison_ShouldReturnOnlyLargerStorageProducts()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"SearchProductsCapacityTest_{Guid.NewGuid()}")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var brand = new HardwareCatalog.Domain.Entities.Brand { Id = Guid.NewGuid(), Name = "TestBrand" };
            context.Brands.Add(brand);
            context.Products.AddRange(
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "1TB SSD", Category = ProductCategory.Storage, UnitOfMeasure = UnitOfMeasure.TB, Model = "SSD-1TB", BrandId = brand.Id },
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "2TB SSD", Category = ProductCategory.Storage, UnitOfMeasure = UnitOfMeasure.TB, Model = "SSD-2TB", BrandId = brand.Id },
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "32 GB DDR5", Category = ProductCategory.Memory, UnitOfMeasure = UnitOfMeasure.GB, Model = "DDR5-32GB", BrandId = brand.Id });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new SearchProductsQueryHandler(context);
            var result = await handler.Handle(new SearchProductsQuery { Query = "show me disk with more than 1TB" }, CancellationToken.None);

            result.Should().ContainSingle();
            result.Single().Name.Should().Be("2TB SSD");
            result.Single().Category.Should().Be(ProductCategory.Storage);
        }
    }

    [Fact]
    public async Task Handle_WithSearchIntentAndBrand_ShouldIgnoreIntentAndMatchBrandWithinCategory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"SearchProductsBrandTest_{Guid.NewGuid()}")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var amd = new HardwareCatalog.Domain.Entities.Brand { Id = Guid.NewGuid(), Name = "AMD" };
            var intel = new HardwareCatalog.Domain.Entities.Brand { Id = Guid.NewGuid(), Name = "Intel" };
            context.Brands.AddRange(amd, intel);
            context.Products.AddRange(
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "AMD Ryzen", Category = ProductCategory.Processor, UnitOfMeasure = UnitOfMeasure.Units, Model = "Ryzen 7", BrandId = amd.Id },
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "Intel Core", Category = ProductCategory.Processor, UnitOfMeasure = UnitOfMeasure.Units, Model = "Core i7", BrandId = intel.Id },
                new HardwareCatalog.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "AMD Radeon", Category = ProductCategory.GraphicCard, UnitOfMeasure = UnitOfMeasure.Units, Model = "RX 7800", BrandId = amd.Id });
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var handler = new SearchProductsQueryHandler(context);
            var result = await handler.Handle(new SearchProductsQuery { Query = "return AMD processors" }, CancellationToken.None);

            result.Should().ContainSingle();
            result.Single().Name.Should().Be("AMD Ryzen");
            result.Single().Category.Should().Be(ProductCategory.Processor);
        }
    }
}
