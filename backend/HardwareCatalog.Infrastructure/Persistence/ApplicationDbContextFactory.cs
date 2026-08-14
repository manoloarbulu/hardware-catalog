using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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

        var currentDirectory = Directory.GetCurrentDirectory();
        var webApiDirectory = new[]
        {
            currentDirectory,
            Path.Combine(currentDirectory, "HardwareCatalog.WebApi"),
            Path.Combine(currentDirectory, "backend", "HardwareCatalog.WebApi")
        }.FirstOrDefault(directory => File.Exists(Path.Combine(directory, "appsettings.Development.json")))
            ?? throw new DirectoryNotFoundException("Unable to locate the HardwareCatalog.WebApi configuration directory.");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(webApiDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
