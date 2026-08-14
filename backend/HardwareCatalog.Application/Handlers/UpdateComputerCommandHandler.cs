using HardwareCatalog.Application.Commands;
using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HardwareCatalog.Infrastructure.Persistence;

namespace HardwareCatalog.Application.Handlers;

/// <summary>
/// Handler for UpdateComputerCommand.
/// </summary>
public class UpdateComputerCommandHandler : IRequestHandler<UpdateComputerCommand, ComputerDto>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateComputerCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ComputerDto> Handle(UpdateComputerCommand request, CancellationToken cancellationToken)
    {
        if (request.Products.Count != request.Products.Select(item => item.ProductId).Distinct().Count())
            throw new InvalidOperationException("A computer cannot contain the same component more than once.");

        var products = await _dbContext.Products
            .Where(product => request.Products.Select(item => item.ProductId).Contains(product.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != request.Products.Select(item => item.ProductId).Distinct().Count())
            throw new InvalidOperationException("One or more selected products do not exist.");

        EnsureRequiredCategories(products);

        var computer = await _dbContext.Computers
            .Include(c => c.ComputerProducts)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (computer == null)
            throw new InvalidOperationException($"Computer with ID {request.Id} not found.");

        // Update computer properties
        computer.Type = request.Type;
        computer.Weight = request.Weight;
        computer.WeightUnit = request.WeightUnit;
        computer.Description = request.Description;
        computer.SerialNumber = request.SerialNumber;
        computer.ManufactureDate = request.ManufactureDate;
        computer.Manufacturer = request.Manufacturer;

        // Update products
        // Remove existing products
        _dbContext.ComputerProducts.RemoveRange(computer.ComputerProducts);

        // Add new products
        foreach (var productCmd in request.Products)
        {
            computer.ComputerProducts.Add(new ComputerProduct
            {
                ComputerId = computer.Id,
                ProductId = productCmd.ProductId,
                Quantity = productCmd.Quantity
            });
        }

        _dbContext.Computers.Update(computer);
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
