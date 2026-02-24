using FluentValidation;
using MasPatas.Application.DTOs;

namespace MasPatas.Application.Validators;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty().WithMessage("Sale must have at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Quantity).GreaterThan(0);
            item.RuleFor(i => i.Price).GreaterThan(0);
        });
    }
}

public class RegisterPaymentCommandValidator : AbstractValidator<RegisterPaymentCommand>
{
    public RegisterPaymentCommandValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
        RuleFor(x => x.PaymentMethod).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class CancelSaleCommandValidator : AbstractValidator<CancelSaleCommand>
{
    public CancelSaleCommandValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
