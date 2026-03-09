using FluentValidation;

namespace Application.Dominio.Queries;

public sealed class SugerirDominiosQueryValidator : AbstractValidator<SugerirDominiosQuery>
{
    public SugerirDominiosQueryValidator()
    {
        RuleFor(x => x.Query)
            .NotEmpty().WithMessage("O parâmetro query (domínio ou palavras-chave) é obrigatório.")
            .MaximumLength(253).WithMessage("Query muito longa.");
        RuleFor(x => x.Country).MaximumLength(2).When(x => !string.IsNullOrEmpty(x.Country));
        RuleFor(x => x.Limit).InclusiveBetween(1, 100).When(x => x.Limit.HasValue);
        RuleFor(x => x.WaitMs).InclusiveBetween(1, 30000).When(x => x.WaitMs.HasValue);
    }
}
