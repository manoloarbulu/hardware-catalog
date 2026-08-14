using HardwareCatalog.Application.Dtos;
using MediatR;

namespace HardwareCatalog.Application.Queries;

/// <summary>
/// Query to retrieve a Computer by ID.
/// </summary>
public class GetComputerByIdQuery : IRequest<ComputerDto?>
{
    public required Guid Id { get; set; }
}
