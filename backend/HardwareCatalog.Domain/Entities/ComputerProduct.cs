namespace HardwareCatalog.Domain.Entities;

/// <summary>
/// Junction/Bridge entity representing the many-to-many relationship between Computer and Product.
/// Tracks the quantity of each product in a computer configuration.
/// </summary>
public class ComputerProduct
{
    public Guid ComputerId { get; set; }
    public Guid ProductId { get; set; }
    public required int Quantity { get; set; } // Must be > 0

    // Navigation properties
    public Computer? Computer { get; set; }
    public Product? Product { get; set; }
}
