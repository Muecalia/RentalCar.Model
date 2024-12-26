using FluentValidation;
using RentalCar.Model.Application.Commands.Request;

namespace RentalCar.Model.Application.Validators;

public class DeleteModelValidator : AbstractValidator<DeleteModelRequest>
{
    public DeleteModelValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Informe o código");
    }
}

