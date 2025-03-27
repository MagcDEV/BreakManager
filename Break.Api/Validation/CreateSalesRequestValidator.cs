using Break.Contracts.Requests;
using FluentValidation;

namespace Break.Api.Validation;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required");

        RuleForEach(x => x.Items).SetValidator(new SaleItemRequestValidator());
    }
}
