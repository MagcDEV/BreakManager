using Break.Contracts.Requests;
using FluentValidation;

namespace Break.Api.Validation;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Barcode).NotEmpty().WithMessage("Barcod3 is required");
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Price must be greater than 0");
        RuleFor(x => x.QuantityInStock).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        RuleFor(x => x.MaximumStockLevel).GreaterThan(1).WithMessage("Max Quantity must be greater than 0");
        RuleFor(x => x.MinimumStockLevel).GreaterThan(1).WithMessage("Min Quantity must be greater than 0");
    }
}
