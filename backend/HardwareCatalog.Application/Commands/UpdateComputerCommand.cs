using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Enums;
using MediatR;

namespace HardwareCatalog.Application.Commands;

/// <summary>
/// Command to update an existing Computer.
/// </summary>
public class UpdateComputerCommand : IRequest<ComputerDto>
{
    public required Guid Id { get; set; }
    public required ComputerType Type { get; set; }
    public required decimal Weight { get; set; }
    public required WeightUnit WeightUnit { get; set; }
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public string? Manufacturer { get; set; }
    public required List<UpdateComputerProductDto> Products { get; set; } = new();
}

/// <summary>
/// DTO for products to update in a computer.
/// </summary>
public class UpdateComputerProductDto
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}
