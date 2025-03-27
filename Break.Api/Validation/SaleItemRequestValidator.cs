using Break.Contracts.Requests;
using FluentValidation;

namespace Break.Api.Validation;

public class SaleItemRequestValidator : AbstractValidator<SaleItemRequest>
{
    public SaleItemRequestValidator()
    {
        RuleFor(x => x.ItemId).NotNull().WithMessage("Item ID is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}
