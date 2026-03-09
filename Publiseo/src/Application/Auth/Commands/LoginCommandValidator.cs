using FluentValidation;

namespace Application.Auth.Commands;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login é obrigatório.");
        RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha é obrigatória.");
    }
}
