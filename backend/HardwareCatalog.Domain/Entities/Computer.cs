using HardwareCatalog.Domain.Enums;

namespace HardwareCatalog.Domain.Entities;

/// <summary>
/// Represents a computer system composed of multiple hardware products.
/// </summary>
public class Computer
{
    public Guid Id { get; set; }
    public required DateTime CreationDate { get; set; }
    public required ComputerType Type { get; set; }
    public required decimal Weight { get; set; }
    public required WeightUnit WeightUnit { get; set; }
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public string? Manufacturer { get; set; }

    // Navigation property
    public ICollection<ComputerProduct> ComputerProducts { get; set; } = new List<ComputerProduct>();
}
