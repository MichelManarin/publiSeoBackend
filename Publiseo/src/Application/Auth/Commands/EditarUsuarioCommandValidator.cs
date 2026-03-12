using FluentValidation;

namespace Application.Auth.Commands;

public sealed class EditarUsuarioCommandValidator : AbstractValidator<EditarUsuarioCommand>
{
    public EditarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório.").MaximumLength(100);
        RuleFor(x => x.Sobrenome).NotEmpty().WithMessage("Sobrenome é obrigatório.").MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.")
            .MaximumLength(256)
            .EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(x => x.Telefone).NotEmpty().WithMessage("Telefone é obrigatório.").MaximumLength(30);
        RuleFor(x => x.Endereco).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Endereco));
        RuleFor(x => x.Cidade).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Cidade));
        RuleFor(x => x.Estado).MaximumLength(2).WithMessage("Estado deve ser a sigla da UF (2 caracteres).").When(x => !string.IsNullOrWhiteSpace(x.Estado));
        RuleFor(x => x.CodigoPostal).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.CodigoPostal));
        RuleFor(x => x.Pais).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Pais));
    }
}
