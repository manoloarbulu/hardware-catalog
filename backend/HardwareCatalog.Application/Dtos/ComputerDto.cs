using HardwareCatalog.Domain.Enums;

namespace HardwareCatalog.Application.Dtos;

/// <summary>
/// Data Transfer Object for Computer.
/// </summary>
public class ComputerDto
{
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public ComputerType Type { get; set; }
    public decimal Weight { get; set; }
    public WeightUnit WeightUnit { get; set; }
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public string? Manufacturer { get; set; }
    public List<ComputerProductDto> Products { get; set; } = new();
}
