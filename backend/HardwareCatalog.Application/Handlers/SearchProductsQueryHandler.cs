using HardwareCatalog.Application.Queries;
using HardwareCatalog.Application.Dtos;
using HardwareCatalog.Domain.Entities;
using HardwareCatalog.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HardwareCatalog.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace HardwareCatalog.Application.Handlers;

/// <summary>
/// Handler for SearchProductsQuery.
/// Implements basic keyword extraction and LINQ-based search.
/// </summary>
public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, List<ProductDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public SearchProductsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var query = request.Query.Trim().ToLowerInvariant();
        var category = ParseCategory(query);
        var capacityFilter = ParseCapacityFilter(query);

        var products = await _dbContext.Products
            .Include(p => p.Brand)
            .ToListAsync(cancellationToken);

        IEnumerable<Product> results = products;
        if (category.HasValue)
            results = results.Where(product => product.Category == category.Value);

        if (capacityFilter is not null)
            results = results.Where(product => MatchesCapacity(product, capacityFilter));

        var keywords = ExtractSearchKeywords(query, capacityFilter is not null);
        foreach (var keyword in keywords)
        {
            results = results.Where(product =>
                product.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                product.Model.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                product.Brand?.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true);
        }

        return results
            .Select(MapToDto)
            .ToList();
    }

    private static ProductCategory? ParseCategory(string query)
    {
        if (Regex.IsMatch(query, @"\b(disks?|hard\s*disks?|drives?|storage|ssds?|hdds?)\b")) return ProductCategory.Storage;
        if (Regex.IsMatch(query, @"\b(memory|memories|ram)\b")) return ProductCategory.Memory;
        if (Regex.IsMatch(query, @"\b(processors?|cpus?)\b")) return ProductCategory.Processor;
        if (Regex.IsMatch(query, @"\b(power\s*supplies|power\s*supply|powersupplies|powersupply|psus?)\b")) return ProductCategory.PowerSupply;
        if (Regex.IsMatch(query, @"\b(external\s*ports?|externalports?|ports?|usb)\b")) return ProductCategory.ExternalPorts;
        if (Regex.IsMatch(query, @"\b(graphic\s*cards?|graphics?|gpu|video\s*cards?)\b")) return ProductCategory.GraphicCard;
        return null;
    }

    private static CapacityFilter? ParseCapacityFilter(string query)
    {
        var comparisonMatch = Regex.Match(query, @"\b(?<comparison>more than|greater than|over|above|at least|minimum of)\s+(?<amount>\d+(?:\.\d+)?)\s*(?<unit>tb|gb|mb)\b");
        if (comparisonMatch.Success)
        {
            var amount = decimal.Parse(comparisonMatch.Groups["amount"].Value);
            var unit = ParseUnitOfMeasure(comparisonMatch.Groups["unit"].Value);
            var inclusive = comparisonMatch.Groups["comparison"].Value is "at least" or "minimum of";
            return new CapacityFilter(amount, unit, inclusive, true);
        }

        var exactMatch = Regex.Match(query, @"\b(?<amount>\d+(?:\.\d+)?)\s*(?<unit>tb|gb|mb)\b");
        if (!exactMatch.Success) return null;

        return new CapacityFilter(
            decimal.Parse(exactMatch.Groups["amount"].Value),
            ParseUnitOfMeasure(exactMatch.Groups["unit"].Value),
            true,
            false);
    }

    private static bool MatchesCapacity(Product product, CapacityFilter filter)
    {
        if (!filter.IsComparison)
            return product.UnitOfMeasure == filter.Unit && product.Value == filter.Amount;

        var productCapacity = ConvertToGb(product.Value, product.UnitOfMeasure);
        var filterCapacity = ConvertToGb(filter.Amount, filter.Unit);
        return filter.Inclusive ? productCapacity >= filterCapacity : productCapacity > filterCapacity;
    }

    private static UnitOfMeasure ParseUnitOfMeasure(string unit) => unit.ToLowerInvariant() switch
    {
        "tb" => UnitOfMeasure.TB,
        "gb" => UnitOfMeasure.GB,
        "mb" => UnitOfMeasure.MB,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, "Unsupported capacity unit.")
    };

    private static decimal ConvertToGb(decimal amount, UnitOfMeasure unit) => unit switch
    {
        UnitOfMeasure.TB => amount * 1024,
        UnitOfMeasure.MB => amount / 1024,
        _ => amount
    };

    private static IEnumerable<string> ExtractSearchKeywords(string query, bool capacityParsed)
    {
        var ignoredWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "show", "return", "find", "list", "get", "display", "give", "bring", "me", "with", "and", "the", "a", "an",
            "more", "than", "greater", "over", "above", "at", "least", "minimum", "of", "disk", "disks", "hard",
            "drive", "drives", "storage", "ssd", "ssds", "hdd", "hdds", "memory", "memories", "ram", "processor",
            "processors", "cpu", "cpus", "power", "supply", "supplies", "powersupply", "powersupplies", "psu", "psus",
            "port", "ports", "externalport", "externalports", "usb", "graphic", "graphics", "graphiccard", "graphiccards",
            "gpu", "video", "card", "cards"
        };

        return Regex.Matches(query, @"[a-z]+")
            .Select(match => match.Value)
            .Where(word => !ignoredWords.Contains(word) && !(capacityParsed && Regex.IsMatch(word, @"^(tb|gb|mb)$")))
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private sealed record CapacityFilter(decimal Amount, UnitOfMeasure Unit, bool Inclusive, bool IsComparison);

    private static ProductDto MapToDto(HardwareCatalog.Domain.Entities.Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Category = product.Category,
            Name = product.Name,
            UnitOfMeasure = product.UnitOfMeasure,
            Value = product.Value,
            BrandId = product.BrandId,
            Model = product.Model,
            BrandName = product.Brand?.Name
        };
    }
}
