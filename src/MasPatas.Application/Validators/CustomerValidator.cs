using FluentValidation;
using MasPatas.Application.DTOs;

namespace MasPatas.Application.Validators;

public class CustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
