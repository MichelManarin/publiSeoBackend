using FluentValidation;

namespace Application.Dominio.Queries;

public sealed class VerificarDisponibilidadeDominioQueryValidator : AbstractValidator<VerificarDisponibilidadeDominioQuery>
{
    public VerificarDisponibilidadeDominioQueryValidator()
    {
        RuleFor(x => x.Dominio)
            .NotEmpty().WithMessage("O domínio é obrigatório.")
            .MaximumLength(253).WithMessage("Domínio muito longo.")
            .Must(SerDominioValido).WithMessage("Formato de domínio inválido (ex.: exemplo.com).");
    }

    private static bool SerDominioValido(string dominio)
    {
        if (string.IsNullOrWhiteSpace(dominio)) return false;
        var parts = dominio.Trim().Split('.', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 && parts.All(p => p.Length > 0 && p.Length <= 63);
    }
}
