using FluentValidation;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email deve ser válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória");
    }
}

