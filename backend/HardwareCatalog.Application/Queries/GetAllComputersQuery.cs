using HardwareCatalog.Application.Dtos;
using MediatR;

namespace HardwareCatalog.Application.Queries;

/// <summary>
/// Query to retrieve all Computers.
/// </summary>
public class GetAllComputersQuery : IRequest<List<ComputerDto>>
{
}
