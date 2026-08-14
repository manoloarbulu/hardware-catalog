using HardwareCatalog.Application.Dtos;
using MediatR;

namespace HardwareCatalog.Application.Queries;

/// <summary>
/// Query to search Products by natural language query.
/// </summary>
public class SearchProductsQuery : IRequest<List<ProductDto>>
{
    public required string Query { get; set; }
}
