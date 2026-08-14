using MediatR;

namespace HardwareCatalog.Application.Commands;

/// <summary>
/// Command to delete a Computer by ID.
/// </summary>
public class DeleteComputerCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}
