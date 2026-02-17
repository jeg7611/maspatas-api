using FluentValidation;
using MasPatas.Application.DTOs;

namespace MasPatas.Application.Validators;

public class SaleValidator : AbstractValidator<RegisterSaleRequest>
{
    public SaleValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}
