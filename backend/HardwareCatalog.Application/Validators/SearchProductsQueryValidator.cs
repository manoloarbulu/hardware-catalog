using HardwareCatalog.Application.Queries;
using FluentValidation;

namespace HardwareCatalog.Application.Validators;

/// <summary>
/// Validator for SearchProductsQuery.
/// </summary>
public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsQueryValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty()
            .WithMessage("Search query is required.")
            .MinimumLength(2)
            .WithMessage("Search query must be at least 2 characters long.");
    }
}
