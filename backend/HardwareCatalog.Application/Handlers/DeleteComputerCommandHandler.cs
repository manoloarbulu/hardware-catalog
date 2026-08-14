using HardwareCatalog.Application.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HardwareCatalog.Infrastructure.Persistence;

namespace HardwareCatalog.Application.Handlers;

/// <summary>
/// Handler for DeleteComputerCommand.
/// </summary>
public class DeleteComputerCommandHandler : IRequestHandler<DeleteComputerCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteComputerCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteComputerCommand request, CancellationToken cancellationToken)
    {
        var computer = await _dbContext.Computers
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (computer == null)
            return false;

        _dbContext.Computers.Remove(computer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
