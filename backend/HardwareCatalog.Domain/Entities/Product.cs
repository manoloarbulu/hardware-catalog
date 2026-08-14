using HardwareCatalog.Domain.Enums;

namespace HardwareCatalog.Domain.Entities;

/// <summary>
/// Represents a hardware product (component).
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    public required ProductCategory Category { get; set; }
    public required string Name { get; set; }
    public required UnitOfMeasure UnitOfMeasure { get; set; }
    public Guid BrandId { get; set; }
    public required string Model { get; set; }

    // Navigation properties
    public Brand? Brand { get; set; }
    public ICollection<ComputerProduct> ComputerProducts { get; set; } = new List<ComputerProduct>();
}
