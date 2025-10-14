using FluentValidation;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Validators;

public class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequestDto>
{
    public CreateVehicleRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("A marca é obrigatória")
            .MaximumLength(100).WithMessage("A marca deve ter no máximo 100 caracteres");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("O modelo é obrigatório")
            .MaximumLength(100).WithMessage("O modelo deve ter no máximo 100 caracteres");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("O ano é obrigatório")
            .Matches(@"^\d{4}$").WithMessage("O ano deve ter 4 dígitos")
            .Must(BeAValidYear).WithMessage("O ano deve estar entre 1900 e o próximo ano");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("A cor é obrigatória")
            .MaximumLength(50).WithMessage("A cor deve ter no máximo 50 caracteres");

        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("A placa é obrigatória")
            .MaximumLength(20).WithMessage("A placa deve ter no máximo 20 caracteres")
            .Matches(@"^[A-Z]{3}-?\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$")
            .WithMessage("A placa deve estar no formato ABC-1234 ou ABC1D23 (Mercosul)");
    }

    private bool BeAValidYear(string year)
    {
        if (int.TryParse(year, out int yearNumber))
        {
            return yearNumber >= 1900 && yearNumber <= DateTime.Now.Year + 1;
        }
        return false;
    }
}

