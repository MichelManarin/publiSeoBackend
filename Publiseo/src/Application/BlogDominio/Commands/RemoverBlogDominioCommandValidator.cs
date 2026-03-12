using FluentValidation;

namespace Application.BlogDominio.Commands;

public sealed class RemoverBlogDominioCommandValidator : AbstractValidator<RemoverBlogDominioCommand>
{
    public RemoverBlogDominioCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
        RuleFor(x => x.BlogDominioId).NotEmpty().WithMessage("Id do domínio é obrigatório.");
    }
}
