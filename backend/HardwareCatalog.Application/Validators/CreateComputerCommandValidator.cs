using HardwareCatalog.Application.Commands;
using FluentValidation;

namespace HardwareCatalog.Application.Validators;

/// <summary>
/// Validator for CreateComputerCommand.
/// </summary>
public class CreateComputerCommandValidator : AbstractValidator<CreateComputerCommand>
{
    public CreateComputerCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Computer type is required and must be valid.");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");

        RuleFor(x => x.WeightUnit)
            .IsInEnum()
            .WithMessage("Weight unit is required and must be valid.");

        RuleFor(x => x.Products)
            .NotEmpty()
            .WithMessage("A computer must have at least one product.")
            .Must(products => products.All(p => p.Quantity > 0))
            .WithMessage("Each product quantity must be greater than 0.")
            .Must(products => products.Select(product => product.ProductId).Distinct().Count() == products.Count)
            .WithMessage("A computer cannot contain the same component more than once.");

        RuleForEach(x => x.Products)
            .ChildRules(product =>
            {
                product.RuleFor(p => p.ProductId)
                    .NotEmpty()
                    .WithMessage("Product ID is required.");

                product.RuleFor(p => p.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Product quantity must be greater than 0.");
            });
    }
}
