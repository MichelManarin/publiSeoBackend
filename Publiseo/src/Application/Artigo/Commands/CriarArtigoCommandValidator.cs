using FluentValidation;

namespace Application.Artigo.Commands;

public sealed class CriarArtigoCommandValidator : AbstractValidator<CriarArtigoCommand>
{
    public CriarArtigoCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog é obrigatório.");
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
        RuleFor(x => x.Titulo).NotEmpty().WithMessage("Título é obrigatório.").MaximumLength(500);
        RuleFor(x => x.MetaDescription).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.MetaDescription));
        RuleFor(x => x.Conteudo).NotEmpty().WithMessage("Conteúdo é obrigatório.");
    }
}
