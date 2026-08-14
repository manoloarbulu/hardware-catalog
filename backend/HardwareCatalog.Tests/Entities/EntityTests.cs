using FluentAssertions;
using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using Xunit;

namespace HardwareCatalog.Tests.Entities;

public class ComputerEntityTests
{
    [Fact]
    public void CreateComputer_WithValidData_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var creationDate = DateTime.UtcNow;
        var type = ComputerType.Desktop;
        var weight = 12.5m;
        var weightUnit = WeightUnit.Kilograms;

        // Act
        var computer = new Computer
        {
            Id = id,
            CreationDate = creationDate,
            Type = type,
            Weight = weight,
            WeightUnit = weightUnit,
            Description = "Test Computer",
            Manufacturer = "TestBrand"
        };

        // Assert
        computer.Id.Should().Be(id);
        computer.CreationDate.Should().Be(creationDate);
        computer.Type.Should().Be(type);
        computer.Weight.Should().Be(weight);
        computer.WeightUnit.Should().Be(weightUnit);
        computer.Description.Should().Be("Test Computer");
        computer.Manufacturer.Should().Be("TestBrand");
    }

    [Fact]
    public void Computer_ShouldBeAbleToHaveProducts()
    {
        // Arrange
        var computer = new Computer
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            Type = ComputerType.Laptop,
            Weight = 2.5m,
            WeightUnit = WeightUnit.Kilograms
        };

        var product1 = new ComputerProduct
        {
            ComputerId = computer.Id,
            ProductId = Guid.NewGuid(),
            Quantity = 1
        };

        var product2 = new ComputerProduct
        {
            ComputerId = computer.Id,
            ProductId = Guid.NewGuid(),
            Quantity = 2
        };

        computer.ComputerProducts = new List<ComputerProduct> { product1, product2 };

        // Act & Assert
        computer.ComputerProducts.Should().HaveCount(2);
        computer.ComputerProducts.Should().Contain(product1);
        computer.ComputerProducts.Should().Contain(product2);
    }

    [Theory]
    [InlineData(ComputerType.Laptop)]
    [InlineData(ComputerType.Desktop)]
    [InlineData(ComputerType.Server)]
    [InlineData(ComputerType.BladeServer)]
    public void Computer_ShouldSupportAllComputerTypes(ComputerType computerType)
    {
        // Arrange & Act
        var computer = new Computer
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            Type = computerType,
            Weight = 5m,
            WeightUnit = WeightUnit.Kilograms
        };

        // Assert
        computer.Type.Should().Be(computerType);
    }
}

public class ProductEntityTests
{
    [Fact]
    public void CreateProduct_WithValidData_ShouldInitializeProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var category = ProductCategory.Memory;
        var unitOfMeasure = UnitOfMeasure.GB;

        // Act
        var product = new Product
        {
            Id = id,
            Name = "Test Memory",
            Category = category,
            UnitOfMeasure = unitOfMeasure,
            BrandId = brandId,
            Model = "DDR5-4800"
        };

        // Assert
        product.Id.Should().Be(id);
        product.Name.Should().Be("Test Memory");
        product.Category.Should().Be(category);
        product.UnitOfMeasure.Should().Be(unitOfMeasure);
        product.BrandId.Should().Be(brandId);
        product.Model.Should().Be("DDR5-4800");
    }

    [Theory]
    [InlineData(ProductCategory.Memory)]
    [InlineData(ProductCategory.Storage)]
    [InlineData(ProductCategory.Processor)]
    [InlineData(ProductCategory.GraphicCard)]
    [InlineData(ProductCategory.PowerSupply)]
    [InlineData(ProductCategory.ExternalPorts)]
    public void Product_ShouldSupportAllCategories(ProductCategory category)
    {
        // Arrange & Act
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Category = category,
            UnitOfMeasure = UnitOfMeasure.Units,
            BrandId = Guid.NewGuid(),
            Model = "Model"
        };

        // Assert
        product.Category.Should().Be(category);
    }
}

public class ComputerProductEntityTests
{
    [Fact]
    public void CreateComputerProduct_WithValidQuantity_ShouldWork()
    {
        // Arrange & Act
        var computerProduct = new ComputerProduct
        {
            ComputerId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 5
        };

        // Assert
        computerProduct.Quantity.Should().Be(5);
    }

    [Fact]
    public void ComputerProduct_ShouldHaveRequiredProperties()
    {
        // Arrange
        var computerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var quantity = 3;

        // Act
        var computerProduct = new ComputerProduct
        {
            ComputerId = computerId,
            ProductId = productId,
            Quantity = quantity
        };

        // Assert
        computerProduct.ComputerId.Should().Be(computerId);
        computerProduct.ProductId.Should().Be(productId);
        computerProduct.Quantity.Should().Be(quantity);
    }
}
