using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Application.BlogIntegracao.Commands;

[ExcludeFromCodeCoverage]
public sealed class ExcluirBlogIntegracaoCommandValidator : AbstractValidator<ExcluirBlogIntegracaoCommand>
{
    public ExcluirBlogIntegracaoCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.BlogId).NotEmpty();
        RuleFor(x => x.IntegracaoId).NotEmpty();
    }
}
