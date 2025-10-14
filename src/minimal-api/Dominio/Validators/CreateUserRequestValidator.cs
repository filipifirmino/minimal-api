using FluentValidation;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email deve ser válido")
            .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres")
            .MaximumLength(50).WithMessage("A senha deve ter no máximo 50 caracteres");
    }
}

