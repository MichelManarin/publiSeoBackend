using FluentValidation;

namespace Application.Blog.Commands;

public sealed class CriarBlogCommandValidator : AbstractValidator<CriarBlogCommand>
{
    public CriarBlogCommandValidator()
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
        RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome do blog é obrigatório.").MaximumLength(300);
        RuleFor(x => x.Nicho).NotEmpty().WithMessage("Nicho é obrigatório.").MaximumLength(200);
        RuleFor(x => x.PalavrasChave)
            .NotNull().WithMessage("Palavras-chave são obrigatórias.")
            .Must(p => p != null && p.Count <= 5).WithMessage("Informe no máximo 5 palavras-chave.");
        RuleForEach(x => x.PalavrasChave).NotEmpty().MaximumLength(100).When(x => x.PalavrasChave != null);
        RuleFor(x => x.UrlSlug).MaximumLength(300).When(x => !string.IsNullOrEmpty(x.UrlSlug));
        RuleFor(x => x.Descricao).MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Descricao));
        RuleFor(x => x.AutorPadraoNome).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.AutorPadraoNome));
        RuleFor(x => x.ObjetivoFinal).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.ObjetivoFinal));
        RuleFor(x => x.DescricaoProdutoVinculado).MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.DescricaoProdutoVinculado));
    }
}
