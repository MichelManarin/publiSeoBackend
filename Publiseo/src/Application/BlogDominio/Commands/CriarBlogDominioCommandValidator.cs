using FluentValidation;

namespace Application.BlogDominio.Commands;

public sealed class CriarBlogDominioCommandValidator : AbstractValidator<CriarBlogDominioCommand>
{
    public CriarBlogDominioCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog é obrigatório.");
        RuleFor(x => x.NomeDominio)
            .NotEmpty().WithMessage("Nome do domínio é obrigatório.")
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
