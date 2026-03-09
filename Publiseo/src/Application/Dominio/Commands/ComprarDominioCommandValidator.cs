using FluentValidation;

namespace Application.Dominio.Commands;

public sealed class ComprarDominioCommandValidator : AbstractValidator<ComprarDominioCommand>
{
    public ComprarDominioCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
        RuleFor(x => x.Dominio)
            .NotEmpty().WithMessage("O domínio é obrigatório.")
            .MaximumLength(253).WithMessage("Domínio muito longo.")
            .Must(SerDominioValido).WithMessage("Formato de domínio inválido (ex.: exemplo.com).");
        RuleFor(x => x.Period).InclusiveBetween(1, 10).WithMessage("Período deve ser entre 1 e 10 anos.");
    }

    private static bool SerDominioValido(string dominio)
    {
        if (string.IsNullOrWhiteSpace(dominio)) return false;
        var parts = dominio.Trim().Split('.', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 && parts.All(p => p.Length > 0 && p.Length <= 63);
    }
}
