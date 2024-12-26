using FluentValidation;
using RentalCar.Model.Application.Commands.Request;

namespace RentalCar.Model.Application.Validators;

public class UpdateModelValidator : AbstractValidator<UpdateModelRequest>
{
    public UpdateModelValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty().WithMessage("Informe o código");

        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Informe o nome");

        RuleFor(m => m.IdCategory)
            .NotEmpty().WithMessage("Informa a categoria");

        RuleFor(m => m.IdManufacturer)
            .NotEmpty().WithMessage("Informa o fabricante");

        RuleFor(m => m.Year)
            .NotEmpty().WithMessage("Informa o ano de fabricação")
            .GreaterThanOrEqualTo(2000).WithMessage("O ano de fabrico tem que ser maior ou igual a 2000");

        RuleFor(m => m.Motor)
            .NotEmpty().WithMessage("Informa o motor")
            .Must(CustomValidator.ValidTransmission).WithMessage("Informa um motor válido");

        RuleFor(m => m.Transmission)
            .NotEmpty().WithMessage("Informa a transmissão")
            .Must(CustomValidator.ValidTransmission).WithMessage("Informa uma transmissão válida");
    }
}

