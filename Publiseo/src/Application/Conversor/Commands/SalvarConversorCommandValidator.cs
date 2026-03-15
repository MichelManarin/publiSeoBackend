using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Application.Conversor.Commands;

[ExcludeFromCodeCoverage]
public sealed class SalvarConversorCommandValidator : AbstractValidator<SalvarConversorCommand>
{
    public SalvarConversorCommandValidator()
    {
        RuleFor(x => x.BlogId).NotEmpty();
        RuleFor(x => x.UsuarioId).NotEmpty();
        RuleFor(x => x.Request).NotNull();
        When(x => x.Request != null, () =>
        {
            RuleFor(x => x.Request!.TextoBotaoInicial).MaximumLength(200).When(x => x.Request!.TextoBotaoInicial != null);
            RuleFor(x => x.Request!.MensagemFinalizacao).MaximumLength(2000).When(x => x.Request!.MensagemFinalizacao != null);
            RuleFor(x => x.Request!.WhatsAppNumero).MaximumLength(20).When(x => x.Request!.WhatsAppNumero != null);
            RuleFor(x => x.Request!.WhatsAppTextoPreDefinido).MaximumLength(1000).When(x => x.Request!.WhatsAppTextoPreDefinido != null);
        });
    }
}
