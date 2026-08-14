using HardwareCatalog.Domain.Enums;

namespace HardwareCatalog.Application.Dtos;

/// <summary>
/// Data Transfer Object for Product.
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public ProductCategory Category { get; set; }
    public string Name { get; set; } = string.Empty;
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public int Value { get; set; }
    public Guid BrandId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string? BrandName { get; set; }
}
