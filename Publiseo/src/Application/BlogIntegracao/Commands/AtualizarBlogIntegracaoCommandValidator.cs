using Domain.Enums;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class AtualizarBlogIntegracaoCommandValidator : AbstractValidator<AtualizarBlogIntegracaoCommand>
{
    public AtualizarBlogIntegracaoCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.BlogId).NotEmpty();
        RuleFor(x => x.IntegracaoId).NotEmpty();
        RuleFor(x => x.Tipo)
            .Must(t => t == null || Enum.TryParse<BlogIntegracaoTipo>(t, ignoreCase: true, out _))
            .WithMessage("Tipo inválido. Valores permitidos: GoogleSiteVerification.")
            .When(x => x.Tipo != null);
    }
}
