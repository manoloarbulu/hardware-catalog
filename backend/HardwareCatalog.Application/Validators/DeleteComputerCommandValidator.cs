using HardwareCatalog.Application.Commands;
using FluentValidation;

namespace HardwareCatalog.Application.Validators;

/// <summary>
/// Validator for DeleteComputerCommand.
/// </summary>
public class DeleteComputerCommandValidator : AbstractValidator<DeleteComputerCommand>
{
    public DeleteComputerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Computer ID is required.");
    }
}
