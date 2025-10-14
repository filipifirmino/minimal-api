using FluentValidation;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email deve ser válido")
            .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("O status deve ser válido (Active ou Inactive)");
    }
}

