using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Enums;
using MediatR;

namespace HardwareCatalog.Application.Commands;

/// <summary>
/// Command to create a new Computer.
/// </summary>
public class CreateComputerCommand : IRequest<ComputerDto>
{
    public required ComputerType Type { get; set; }
    public required decimal Weight { get; set; }
    public required WeightUnit WeightUnit { get; set; }
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public string? Manufacturer { get; set; }
    public required List<CreateComputerProductDto> Products { get; set; } = new();
}

/// <summary>
/// DTO for products to add to a computer during creation.
/// </summary>
public class CreateComputerProductDto
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}
