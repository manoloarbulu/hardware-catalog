using HardwareCatalog.Application.Queries;
using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HardwareCatalog.Infrastructure.Persistence;

namespace HardwareCatalog.Application.Handlers;

/// <summary>
/// Handler for GetComputerByIdQuery.
/// </summary>
public class GetComputerByIdQueryHandler : IRequestHandler<GetComputerByIdQuery, ComputerDto?>
{
    private readonly ApplicationDbContext _dbContext;

    public GetComputerByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ComputerDto?> Handle(GetComputerByIdQuery request, CancellationToken cancellationToken)
    {
        var computer = await _dbContext.Computers
            .Include(c => c.ComputerProducts)
            .ThenInclude(cp => cp.Product)
            .ThenInclude(p => p!.Brand)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        return computer == null ? null : MapToDto(computer);
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
                        BrandId = cp.Product.BrandId,
                        Model = cp.Product.Model,
                        BrandName = cp.Product.Brand?.Name
                    } : null
                })
                .ToList()
        };
    }
}
