using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Application.Conversor.Commands;

[ExcludeFromCodeCoverage]
public sealed class RegistrarConversorLeadCommandValidator : AbstractValidator<RegistrarConversorLeadCommand>
{
    public RegistrarConversorLeadCommandValidator()
    {
        RuleFor(x => x.BlogExternalId).NotEmpty();
        RuleFor(x => x.NomeCompleto).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Telefone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Respostas).NotNull();
    }
}
