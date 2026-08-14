# Skill: Implementing CQRS with MediatR

**Pattern Definition:**

1. **Request:** Define a `public record RequestNameCommand(Type Property) : IRequest<ReturnType>;`
2. **Handler:** Create a class implementing `IRequestHandler<RequestNameCommand, ReturnType>`.
3. **Validation:** Create a class inheriting from `AbstractValidator<RequestNameCommand>` using FluentValidation.
4. **Location:** Place these inside the `Application` project, grouped by feature (e.g., `Application/Features/Computers/Commands/CreateComputer/`).

**Example Snippet:**

```csharp
public record CreateComputerCommand(List<ComputerProductDto> Products) : IRequest<Guid>;

public class CreateComputerCommandHandler : IRequestHandler<CreateComputerCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateComputerCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Guid> Handle(CreateComputerCommand request, CancellationToken cancellationToken)
    {
        // Implementation here
    }
}
```
