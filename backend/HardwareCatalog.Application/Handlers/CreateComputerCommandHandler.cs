using HardwareCatalog.Application.Commands;
using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HardwareCatalog.Infrastructure.Persistence;

namespace HardwareCatalog.Application.Handlers;

/// <summary>
/// Handler for CreateComputerCommand.
/// </summary>
public class CreateComputerCommandHandler : IRequestHandler<CreateComputerCommand, ComputerDto>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateComputerCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ComputerDto> Handle(CreateComputerCommand request, CancellationToken cancellationToken)
    {
        if (request.Products.Count != request.Products.Select(item => item.ProductId).Distinct().Count())
            throw new InvalidOperationException("A computer cannot contain the same component more than once.");

        var products = await _dbContext.Products
            .Where(product => request.Products.Select(item => item.ProductId).Contains(product.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != request.Products.Select(item => item.ProductId).Distinct().Count())
            throw new InvalidOperationException("One or more selected products do not exist.");

        EnsureRequiredCategories(products);

        var computer = new Computer
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.UtcNow,
            Type = request.Type,
            Weight = request.Weight,
            WeightUnit = request.WeightUnit,
            Description = request.Description,
            SerialNumber = request.SerialNumber,
            ManufactureDate = request.ManufactureDate,
            Manufacturer = request.Manufacturer
        };

        // Add products to the computer
        foreach (var productCmd in request.Products)
        {
            computer.ComputerProducts.Add(new ComputerProduct
            {
                ComputerId = computer.Id,
                ProductId = productCmd.ProductId,
                Quantity = productCmd.Quantity
            });
        }

        _dbContext.Computers.Add(computer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDto(computer);
    }

    private static void EnsureRequiredCategories(IEnumerable<Product> products)
    {
        var requiredCategories = new[]
        {
            ProductCategory.Processor,
            ProductCategory.Memory,
            ProductCategory.Storage,
            ProductCategory.GraphicCard,
            ProductCategory.PowerSupply,
            ProductCategory.ExternalPorts
        };

        var missingCategories = requiredCategories.Where(category => !products.Any(product => product.Category == category));
        if (missingCategories.Any())
            throw new InvalidOperationException($"A computer requires: {string.Join(", ", missingCategories)}.");
    }

    private static ComputerDto MapToDto(Computer computer)
    {
        return new ComputerDto
        {
            Id = computer.Id,
            CreationDate = computer.CreationDate,
            Type = computer.Type,
            Weight = computer.Weight,
            WeightUnit = computer.WeightUnit,
            Description = computer.Description,
            SerialNumber = computer.SerialNumber,
            ManufactureDate = computer.ManufactureDate,
            Manufacturer = computer.Manufacturer,
            Products = computer.ComputerProducts
                .Select(cp => new ComputerProductDto
                {
                    ProductId = cp.ProductId,
                    Quantity = cp.Quantity,
                    Product = cp.Product != null ? new ProductDto
                    {
                        Id = cp.Product.Id,
                        Category = cp.Product.Category,
                        Name = cp.Product.Name,
                        UnitOfMeasure = cp.Product.UnitOfMeasure,
                        Value = cp.Product.Value,
                        BrandId = cp.Product.BrandId,
                        Model = cp.Product.Model,
                        BrandName = cp.Product.Brand?.Name
                    } : null
                })
                .ToList()
        };
    }
}
