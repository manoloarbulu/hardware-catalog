using HardwareCatalog.Application.Queries;
using FluentValidation;

namespace HardwareCatalog.Application.Validators;

/// <summary>
/// Validator for GetComputerByIdQuery.
/// </summary>
public class GetComputerByIdQueryValidator : AbstractValidator<GetComputerByIdQuery>
{
    public GetComputerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Computer ID is required.");
    }
}
