using HardwareCatalog.Domain.Enums;

namespace HardwareCatalog.Application.Dtos;

/// <summary>
/// Data Transfer Object for ComputerProduct (quantity and product details).
/// </summary>
public class ComputerProductDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public ProductDto? Product { get; set; }
}
