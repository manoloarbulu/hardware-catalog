using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HardwareCatalog.Infrastructure.Persistence;

/// <summary>
/// Factory for creating ApplicationDbContext instances at design time.
/// This is used by Entity Framework Core tools for migrations.
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.UseSqlServer("Server=MANOLOLAPTOP;Database=ProductsDemo;Trusted_Connection=true;TrustServerCertificate=true;");
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
