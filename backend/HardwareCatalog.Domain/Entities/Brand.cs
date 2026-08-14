namespace HardwareCatalog.Domain.Entities;

/// <summary>
/// Represents a hardware brand/manufacturer.
/// </summary>
public class Brand
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
