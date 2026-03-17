using Domain.Enums;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class CriarBlogIntegracaoCommandValidator : AbstractValidator<CriarBlogIntegracaoCommand>
{
    public CriarBlogIntegracaoCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.BlogId).NotEmpty();
        RuleFor(x => x.Tipo)
            .NotEmpty()
            .Must(t => Enum.TryParse<BlogIntegracaoTipo>(t, ignoreCase: true, out _))
            .WithMessage("Tipo inválido. Valores permitidos: GoogleSiteVerification.");
        RuleFor(x => x.Valor).NotNull();
    }
}
