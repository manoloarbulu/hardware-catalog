using HardwareCatalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HardwareCatalog.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public BrandsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<BrandResponse>>> GetAll()
    {
        return Ok(await _dbContext.Brands.OrderBy(brand => brand.Name)
            .Select(brand => new BrandResponse { Id = brand.Id, Name = brand.Name })
            .ToListAsync());
    }
}

public class BrandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
