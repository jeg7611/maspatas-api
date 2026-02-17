using FluentValidation;
using MasPatas.Application.DTOs;

namespace MasPatas.Application.Validators;

public class InventoryMovementValidator : AbstractValidator<AddInventoryMovementRequest>
{
    public InventoryMovementValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Type).Must(t => t is "IN" or "OUT" || t is "in" or "out")
            .WithMessage("Type must be IN or OUT.");
    }
}
