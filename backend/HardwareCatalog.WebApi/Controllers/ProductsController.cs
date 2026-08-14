using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Application.Queries;
using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using HardwareCatalog.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HardwareCatalog.WebApi.Controllers;

/// <summary>
/// API controller for Product operations and search.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public ProductsController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> GetAll([FromQuery] ProductCategory? category, [FromQuery] Guid? brandId)
    {
        var products = _dbContext.Products.Include(product => product.Brand).AsQueryable();
        if (category.HasValue) products = products.Where(product => product.Category == category.Value);
        if (brandId.HasValue) products = products.Where(product => product.BrandId == brandId.Value);

        return Ok(await products.OrderBy(product => product.Name).Select(product => new ProductDto
        {
            Id = product.Id,
            Category = product.Category,
            Name = product.Name,
            UnitOfMeasure = product.UnitOfMeasure,
            Value = product.Value,
            BrandId = product.BrandId,
            Model = product.Model,
            BrandName = product.Brand!.Name
        }).ToListAsync());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductDto>> Create([FromBody] ProductRequest request)
    {
        if (request.Value <= 0)
            return BadRequest(new { error = "Value must be greater than 0." });

        var product = new Product { Id = Guid.NewGuid(), Name = request.Name, Category = request.Category, UnitOfMeasure = request.UnitOfMeasure, Value = request.Value, BrandId = request.BrandId, Model = request.Model };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { product.Id }, await MapProduct(product.Id));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] ProductRequest request)
    {
        if (request.Value <= 0)
            return BadRequest(new { error = "Value must be greater than 0." });

        var product = await _dbContext.Products.FindAsync(id);
        if (product is null) return NotFound();
        if (product.Category != request.Category)
            return BadRequest(new { error = "A product category cannot be changed after the product is created." });

        product.Name = request.Name;
        product.UnitOfMeasure = request.UnitOfMeasure;
        product.Value = request.Value;
        product.BrandId = request.BrandId;
        product.Model = request.Model;
        await _dbContext.SaveChangesAsync();
        return Ok(await MapProduct(id));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product is null) return NotFound();
        if (await _dbContext.ComputerProducts.AnyAsync(component => component.ProductId == id))
            return Conflict(new { error = "This product cannot be deleted because it is used by a computer." });

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Search for products using natural language query.
    /// </summary>
    /// <param name="query">The search query (e.g., "Show me 1TB storage drives")</param>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<ProductDto>>> Search([FromQuery] string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { error = "Search query is required" });

        try
        {
            var results = await _mediator.Send(new SearchProductsQuery { Query = query });
            return Ok(results);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
    }

    private async Task<ProductDto> MapProduct(Guid id)
    {
        var product = await _dbContext.Products.Include(item => item.Brand).SingleAsync(item => item.Id == id);
        return new ProductDto { Id = product.Id, Category = product.Category, Name = product.Name, UnitOfMeasure = product.UnitOfMeasure, Value = product.Value, BrandId = product.BrandId, Model = product.Model, BrandName = product.Brand?.Name };
    }
}

public class ProductRequest
{
    public required ProductCategory Category { get; set; }
    public required string Name { get; set; }
    public required UnitOfMeasure UnitOfMeasure { get; set; }
    public required int Value { get; set; }
    public required Guid BrandId { get; set; }
    public required string Model { get; set; }
}
