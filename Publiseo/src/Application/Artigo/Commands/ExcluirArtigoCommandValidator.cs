using FluentValidation;

namespace Application.Artigo.Commands;

public sealed class ExcluirArtigoCommandValidator : AbstractValidator<ExcluirArtigoCommand>
{
    public ExcluirArtigoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id do artigo é obrigatório.");
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Usuário é obrigatório.");
    }
}
